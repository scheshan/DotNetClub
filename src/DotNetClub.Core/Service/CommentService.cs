using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DotNetClub.Domain.Entity;
using Shared.Infrastructure.Model;
using Shared.Infrastructure.UnitOfWork;
using DotNetClub.Core.Model.Comment;
using DotNetClub.Domain.Enums;
using Shared.Infrastructure.Extensions;
using DotNetClub.Core.Redis;
using StackExchange.Redis;
using DotNetClub.Domain.Consts;
using AutoMapper;
using DotNetClub.Core.Model.User;
using DotNetClub.Domain.Model;
using DotNetClub.Domain.Repository;
using DotNetClub.Core.Model.Message;

namespace DotNetClub.Core.Service
{
    public class CommentService : ServiceBase
    {
        private MessageService MessageService { get; set; }

        public CommentService(IServiceProvider serviceProvider, MessageService messageService)
            : base(serviceProvider)
        {
            this.MessageService = messageService;
        }

        public async Task<Result<long>> Add(AddCommentModel model)
        {
            using (var uw = this.CreateUnitOfWork())
            {
                var topic = await uw.GetAsync<Topic>(t => t.ID == model.TopicID && !t.IsDelete);

                if (topic == null)
                {
                    return Result<long>.ErrorResult("该主题不存在");
                }
                else if (topic.IsLock)
                {
                    return Result<long>.ErrorResult("该主题已被锁定");
                }

                if (model.ReplyTo.HasValue)
                {
                    var replyToComment = await uw.GetAsync<Comment>(t => t.ID == model.ReplyTo.Value && !t.IsDelete);
                    if (replyToComment == null)
                    {
                        return Result<long>.ErrorResult("该评论不存在");
                    }
                    if (replyToComment.TopicID != model.TopicID)
                    {
                        return Result<long>.ErrorResult("错误的请求");
                    }
                }

                using (var tran = uw.BeginTransaction())
                {
                    var entity = new Comment
                    {
                        Content = model.Content,
                        CreateDate = DateTime.Now,
                        CreateUser = this.SecurityManager.CurrentUser.ID,
                        IsDelete = false,
                        ReplyID = model.ReplyTo,
                        TopicID = model.TopicID
                    };
                    await uw.InsertAsync(entity);

                    topic.LastReplyDate = DateTime.Now;
                    topic.LastReplyUserID = this.SecurityManager.CurrentUser.ID;
                    await uw.UpdateAsync(topic);

                    #region 发送回复主题的消息
                    {
                        if (this.SecurityManager.CurrentUser.ID != topic.CreateUser)
                        {
                            var addMessageModel = new AddMessageModel
                            {
                                Comment = entity.ID,
                                FromUser = this.SecurityManager.CurrentUser.ID,
                                Topic = topic.ID,
                                ToUser = topic.CreateUser,
                                Type = MessageType.Comment
                            };

                            await this.MessageService.Add(addMessageModel);
                        }
                    }
                    #endregion

                    #region 发送@的评论
                    {
                        var atUserList = Utility.AtHelper.FetchUsers(model.Content);
                        if (atUserList.Count > 0)
                        {
                            var userList = await uw.QueryAsync<User>(t => atUserList.Contains(t.UserName));
                            foreach (var user in userList)
                            {
                                if (user.ID == this.SecurityManager.CurrentUser.ID || user.ID == topic.CreateUser) //过滤@自己，过滤重复@作者
                                {
                                    continue;
                                }

                                var addMessageModel = new AddMessageModel
                                {
                                    Comment = entity.ID,
                                    FromUser = this.SecurityManager.CurrentUser.ID,
                                    Topic = topic.ID,
                                    ToUser = user.ID,
                                    Type = MessageType.At
                                };
                                await this.MessageService.Add(addMessageModel);
                            }
                        }
                    }
                    #endregion

                    tran.Commit();

                    return Result.SuccessResult(entity.ID);
                }
            }
        }

        public async Task<List<CommentModel>> QueryByTopic(long topicID)
        {
            using (var uw = this.CreateUnitOfWork())
            {
                var entityList = await uw.QueryAsync<Comment>(t => !t.IsDelete && t.TopicID == topicID);
                return await this.Transform(entityList.ToArray());
            }
        }

        public async Task<Result> Delete(long id)
        {
            using (var uw = this.CreateUnitOfWork())
            {
                var comment = await uw.GetAsync<Comment>(t => t.ID == id && !t.IsDelete);

                if (comment == null)
                {
                    return Result.ErrorResult("评论不存在");
                }
                if (!this.SecurityManager.CanOperateComment(comment))
                {
                    return Result.ErrorResult("无权操作");
                }

                comment.IsDelete = true;
                await uw.UpdateAsync(comment);
            }

            return Result.SuccessResult();
        }

        public async Task<List<CommentModel>> Transform(params Comment[] entityList)
        {
            if (entityList.IsEmptyCollection())
            {
                return new List<CommentModel>();
            }

            var redis = this.RedisProvider.GetDatabase();

            var idList = entityList.Select(t => t.ID).ToArray();
            var userIDList = entityList.Select(t => t.CreateUser).Distinct().ToList();
            var fields = userIDList.Select(t => (RedisValue)t).ToArray();
            List<User> userList = redis.JsonHashGet<User>(RedisKeys.User, fields);
            List<CommentVotes> commentVotesList;
            List<CommentVote> commentVoteList = new List<CommentVote>();

            using (var uw = this.CreateUnitOfWork())
            {
                commentVotesList = await uw.CreateRepository<ICommentVoteRepository>().QueryCommentVotes(idList);

                if (SecurityManager.IsLogin)
                {
                    commentVoteList = await uw.QueryAsync<CommentVote>(t => idList.Contains(t.CommentID) && t.UserID == SecurityManager.CurrentUser.ID);
                }
            }

            var result = entityList.Select(entity =>
            {
                var model = new CommentModel
                {
                    Content = entity.Content,
                    CreateDate = entity.CreateDate,
                    ID = entity.ID
                };

                var user = userList.SingleOrDefault(u => u.ID == entity.CreateUser);
                model.CreateUser = Mapper.Map<UserBasicModel>(user);

                var commentVotes = commentVotesList.SingleOrDefault(t => t.CommentID == entity.ID);
                if (commentVotes != null)
                {
                    model.Votes = commentVotes.Votes;
                }

                model.Voted = commentVoteList.Any(t => t.CommentID == entity.ID);

                return model;
            });

            foreach (var entity in entityList)
            {
                if (entity.ReplyID.HasValue)
                {
                    var model = result.SingleOrDefault(t => t.ID == entity.ID);
                    var replyToModel = result.SingleOrDefault(t => t.ID == entity.ReplyID.Value);

                    if (replyToModel != null)
                    {
                        model.ReplyTo = replyToModel;
                    }
                    else
                    {
                        model.ReplyToIsDelete = true;
                    }
                }
            }

            return result.ToList();
        }
    }
}
