using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DotNetClub.Domain.Enums;
using DotNetClub.Domain.Entity;
using DotNetClub.Core.Model.Message;
using Share.Infrastructure.Extensions;
using Share.Infrastructure.UnitOfWork;
using AutoMapper;
using DotNetClub.Core.Model.User;
using DotNetClub.Core.Model.Topic;
using DotNetClub.Domain.Repository;
using Share.Infrastructure.Redis;
using StackExchange.Redis;
using DotNetClub.Domain.Consts;

namespace DotNetClub.Core.Service
{
    public class MessageService : ServiceBase
    {
        public MessageService(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {

        }

        /// <summary>
        /// 添加一条消息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task Add(AddMessageModel model)
        {
            long messageID;

            using (var uw = this.CreateUnitOfWork())
            {
                var entity = new Message
                {
                    CommentID = model.Comment,
                    CreateDate = DateTime.Now,
                    FromUserID = model.FromUser,
                    TopicID = model.Topic,
                    ToUserID = model.ToUser,
                    Type = model.Type
                };

                await uw.InsertAsync(entity);

                messageID = entity.ID;
            }

            var redis = this.RedisProvider.GetDatabase();
            string key = RedisKeys.GetUserMessageCacheKey(model.ToUser);

            await redis.SetAddAsync(key, messageID);
        }

        /// <summary>
        /// 查询未读消息数目
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public long QueryUnreadCount(long userID)
        {
            var redis = this.RedisProvider.GetDatabase();
            string key = RedisKeys.GetUserMessageCacheKey(userID);

            return redis.SetLength(key);
        }

        /// <summary>
        /// 查询用户未读消息列表
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<List<MessageModel>> QueryUnread(long userID)
        {
            using (var uw = this.CreateUnitOfWork())
            {
                var entityList = await uw.QueryAsync<Message>(t => t.ToUserID == userID && t.IsRead == false);

                return await this.Transform(entityList.OrderByDescending(t => t.ID).ToArray());
            }
        }

        /// <summary>
        /// 查询用户历史消息列表
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<List<MessageModel>> QueryHistory(long userID, int count)
        {
            using (var uw = this.CreateUnitOfWork())
            {
                var entityList = await uw.CreateRepository<IMessageRepository>().QueryHistory(userID, count);

                return await this.Transform(entityList.ToArray());
            }
        }

        /// <summary>
        /// 将消息标记为已读
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="idList"></param>
        /// <returns></returns>
        public async Task MarkAsRead(long userID, long[] idList)
        {
            if (idList.Length == 0)
            {
                return;
            }

            using (var uw = this.CreateUnitOfWork())
            {
                await uw.CreateRepository<IMessageRepository>().MarkAsRead(userID, idList);
            }

            var redis = this.RedisProvider.GetDatabase();
            RedisValue[] values = idList.Select(t => (RedisValue)t).ToArray();
            string key = RedisKeys.GetUserMessageCacheKey(userID);

            await redis.SetRemoveAsync(key, values);
        }

        private async Task<List<MessageModel>> Transform(params Message[] entityList)
        {
            if (entityList.IsEmptyCollection())
            {
                return new List<MessageModel>();
            }

            var userIDList = entityList.Where(t => t.FromUserID.HasValue).Select(t => t.FromUserID.Value).ToList();
            var topicIDList = entityList.Where(t => t.TopicID.HasValue).Select(t => t.TopicID.Value).ToList();

            List<User> userList = new List<User>();
            List<Topic> topicList = new List<Topic>();

            var redis = this.RedisProvider.GetDatabase();

            using (var uw = this.CreateUnitOfWork())
            {
                if (userIDList.Any())
                {
                    var fields = userIDList.Select(t => (RedisValue)t).ToArray();
                    userList = redis.JsonHashGet<User>(RedisKeys.User, fields);
                }
                if (topicIDList.Any())
                {
                    topicList = await uw.QueryAsync<Topic>(t => topicIDList.Contains(t.ID));
                }
            }

            var result = entityList.Select(entity =>
            {
                var model = new MessageModel
                {
                    CreateDate = entity.CreateDate,
                    ID = entity.ID,
                    IsRead = entity.IsRead,
                    Type = entity.Type
                };

                if (entity.FromUserID.HasValue)
                {
                    var user = userList.SingleOrDefault(u => u.ID == entity.FromUserID.Value);
                    model.FromUser = Mapper.Map<UserBasicModel>(user);
                }
                if (entity.TopicID.HasValue)
                {
                    var topic = topicList.SingleOrDefault(t => t.ID == entity.TopicID.Value);
                    model.Topic = Mapper.Map<TopicBasicModel>(topic);
                }

                return model;
            });

            return result.ToList();
        }
    }
}
