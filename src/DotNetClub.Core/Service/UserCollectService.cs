using DotNetClub.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DotNetClub.Core.Entity;

namespace DotNetClub.Core.Service
{
    public class UserCollectService
    {
        private ClientManager ClientManager { get; set; }

        private Data.ClubContext DbContext { get; set; }

        public UserCollectService(ClientManager clientManager, Data.ClubContext dbContext)
        {
            this.ClientManager = clientManager;
            this.DbContext = dbContext;
        }

        public async Task<OperationResult<bool?>> Collect(int topicID)
        {
            var topic = await this.DbContext.Topics.SingleOrDefaultAsync(t => t.ID == topicID && !t.IsDelete);
            if (topic == null)
            {
                return OperationResult<bool?>.Failure("该主题不存在");
            }

            bool isCollect;

            var entity = await this.DbContext.UserCollects.SingleOrDefaultAsync(t => t.UserID == this.ClientManager.CurrentUser.ID && t.TopicID == topicID);

            if (entity == null)
            {
                isCollect = true;
                topic.CollectCount++;

                entity = new UserCollect
                {
                    CreateDate = DateTime.Now,
                    TopicID = topicID,
                    UserID = this.ClientManager.CurrentUser.ID
                };
                this.DbContext.UserCollects.Add(entity);
            }
            else
            {
                isCollect = false;
                topic.CollectCount--;

                this.DbContext.UserCollects.Remove(entity);
            }

            await this.DbContext.SaveChangesAsync();

            return new OperationResult<bool?>(isCollect);
        }

        public async Task<int> GetCollectCount(long userID)
        {
            var query = this.DbContext.UserCollects.Include(t => t.Topic).Where(t => t.UserID == userID && !t.Topic.IsDelete);

            return await query.CountAsync();
        }

        public async Task<bool> IsCollected(long topicID, long userID)
        {
            return await this.DbContext.UserCollects.AnyAsync(t => t.TopicID == topicID && t.UserID == userID);
        }
    }
}
