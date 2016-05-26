using DotNetClub.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DotNetClub.Core.Entity;

namespace DotNetClub.Core.Service
{
    public class CommentService
    {
        private Data.ClubContext DbContext { get; set; }

        private ClientManager ClientManager { get; set; }

        private MessageService MessageService { get; set; }

        public CommentService(Data.ClubContext dbContext, ClientManager clientManager, MessageService messageService)
        {
            this.DbContext = dbContext;
            this.ClientManager = clientManager;
            this.MessageService = messageService;
        }

        public async Task<OperationResult<Comment>> Add(int topicID, string content, int? replyTo)
        {
            var topic = await this.DbContext.Topics.SingleOrDefaultAsync(t => t.ID == topicID && !t.IsDelete);

            if (topic == null)
            {
                return OperationResult<Comment>.Failure("该主题不存在");
            }
            else if (topic.Lock)
            {
                return OperationResult<Comment>.Failure("该主题已被锁定");
            }

            if (replyTo.HasValue)
            {
                var replyToComment = await this.DbContext.Comments.SingleOrDefaultAsync(t => t.ID == replyTo.Value && !t.IsDelete);
                if (replyToComment == null)
                {
                    return OperationResult<Comment>.Failure("该评论不存在");
                }
                if (replyToComment.TopicID != topicID)
                {
                    return OperationResult<Comment>.Failure("错误的请求");
                }
            }

            var entity = new Comment
            {
                Content = content,
                CreateDate = DateTime.Now,
                CreateUserID = this.ClientManager.CurrentUser.ID,
                IsDelete = false,
                ReplyID = replyTo,
                TopicID = topicID
            };
            topic.ReplyCount++;
            topic.LastReplyDate = DateTime.Now;
            topic.LastReplyUserID = this.ClientManager.CurrentUser.ID;
            this.DbContext.Add(entity);

            #region 发送回复主题的消息
            {
                if (this.ClientManager.CurrentUser.ID != topic.CreateUserID)
                {
                    var message = new Message
                    {
                        Comment = entity,
                        CreateDate = DateTime.Now,
                        FromUserID = this.ClientManager.CurrentUser.ID,
                        IsRead = false,
                        TopicID = topic.ID,
                        ToUserID = topic.CreateUserID,
                        Type = Enums.MessageType.Comment
                    };
                    this.DbContext.Messages.Add(message);
                }
            }
            #endregion

            #region 发送@的评论
            {
                var atUserList = Utility.AtHelper.FetchUsers(content);
                if (atUserList.Count > 0)
                {
                    var userList = await this.DbContext.Users.Where(t => atUserList.Contains(t.UserName)).ToListAsync();
                    foreach (var user in userList)
                    {
                        if (user.ID == this.ClientManager.CurrentUser.ID || user.ID == topic.CreateUserID) //过滤@自己，过滤重复@作者
                        {
                            continue;
                        }
                        var message = new Message
                        {
                            Comment = entity,
                            CreateDate = DateTime.Now,
                            FromUserID = this.ClientManager.CurrentUser.ID,
                            IsRead = false,
                            TopicID = topic.ID,
                            ToUserID = user.ID,
                            Type = Enums.MessageType.At
                        };
                        this.DbContext.Messages.Add(message);
                    }
                }
            }
            #endregion

            await this.DbContext.SaveChangesAsync();

            return new OperationResult<Comment>(entity);
        }

        public async Task<List<Comment>> QueryByTopic(int topicID)
        {
            var query = this.DbContext.Comments.Where(t => !t.IsDelete)
                .Where(t => t.TopicID == topicID)
                .Include(t => t.CreateUser)
                .OrderBy(t => t.ID);

            return await query.ToListAsync();
        }

        public async Task<Comment> Get(int id)
        {
            return await this.DbContext.Comments.SingleOrDefaultAsync(t => t.ID == id && !t.IsDelete);
        }

        public async Task<OperationResult<Comment>> Edit(int id, string content)
        {
            var comment = await this.DbContext.Comments.SingleOrDefaultAsync(t => t.ID == id && !t.IsDelete);
            if (comment == null)
            {
                return OperationResult<Comment>.Failure("评论不存在");
            }
            if (!this.ClientManager.CanOperateComment(comment))
            {
                return OperationResult<Comment>.Failure("无权操作");
            }

            comment.Content = content;
            await this.DbContext.SaveChangesAsync();

            return new OperationResult<Comment>(comment);
        }

        public async Task<OperationResult<Comment>> Delete(int id)
        {
            var comment = await this.DbContext.Comments.Include(t => t.Topic).SingleOrDefaultAsync(t => t.ID == id && !t.IsDelete);

            if (comment == null)
            {
                return OperationResult<Comment>.Failure("评论不存在");
            }
            if (!this.ClientManager.CanOperateComment(comment))
            {
                return OperationResult<Comment>.Failure("无权操作");
            }

            if (comment != null)
            {
                comment.IsDelete = true;
                comment.Topic.ReplyCount = comment.Topic.ReplyCount - 1;

                await this.DbContext.SaveChangesAsync();
            }

            return new OperationResult<Comment>(comment);
        }
    }
}
