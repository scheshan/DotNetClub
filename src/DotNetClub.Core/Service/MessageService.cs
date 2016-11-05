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

namespace DotNetClub.Core.Service
{
    public class MessageService : ServiceBase
    {
        public MessageService(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {

        }

        public async Task<List<MessageModel>> QueryUnreadMessageList(long userID)
        {
            using (var uw = this.CreateUnitOfWork())
            {
                var entityList = await uw.QueryAsync<Message>(t => t.ToUserID == userID && t.IsRead == false);

                return await this.Transform(entityList.ToArray());
            }
        }

        public async Task<List<MessageModel>> QueryHistory(long userID, int count)
        {
            using (var uw = this.CreateUnitOfWork())
            {
                var entityList = await uw.CreateRepository<IMessageRepository>().QueryHistory(userID, count);

                return await this.Transform(entityList.ToArray());
            }
        }

        public async Task MarkAsRead(long[] idList)
        {
            if (idList.Length == 0)
            {
                return;
            }

            using (var uw = this.CreateUnitOfWork())
            {
                await uw.CreateRepository<IMessageRepository>().MarkAsRead(SecurityManager.CurrentUser.ID, idList);
            }
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

            using (var uw = this.CreateUnitOfWork())
            {
                if (userIDList.Any())
                {
                    userList = await uw.QueryAsync<User>(t => userIDList.Contains(t.ID));
                }
                if (topicIDList.Any())
                {
                    topicList = await uw.QueryAsync<Topic>(t => topicIDList.Contains(t.ID);
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
