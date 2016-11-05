using DotNetClub.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Share.Infrastructure.Model;
using Share.Infrastructure.UnitOfWork;
using DotNetClub.Domain.Entity;

namespace DotNetClub.Core.Service
{
    public class TopicCollectService : ServiceBase
    {
        public TopicCollectService(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {

        }

        public async Task<Result<bool>> Collect(int topicID)
        {
            using (var uw = this.CreateUnitOfWork())
            {
                var topic = await uw.GetAsync<Topic>(t => t.ID == topicID && !t.IsDelete);

                if (topic == null)
                {
                    return Result<bool>.ErrorResult("该主题不存在");
                }

                var entity = await uw.GetAsync<TopicCollect>(t => t.TopicID == topicID && t.UserID == SecurityManager.CurrentUser.ID);

                bool result = false;

                if (entity == null)
                {
                    entity = new TopicCollect
                    {
                        CreateDate = DateTime.Now,
                        TopicID = topicID,
                        UserID = SecurityManager.CurrentUser.ID
                    };
                    await uw.InsertAsync(entity);

                    result = true;
                }
                else
                {
                    await uw.DeleteAsync(entity);
                }

                return Result.SuccessResult(result);
            }
        }

        public async Task<long> GetCollectCount(long userID)
        {
            using (var uw = this.CreateUnitOfWork())
            {
                return await uw.CountAsync<TopicCollect>(t => t.UserID == userID);
            }
        }

        public async Task<bool> IsCollected(long topicID, long userID)
        {
            using (var uw = this.CreateUnitOfWork())
            {
                return await uw.ExistAsync<TopicCollect>(t => t.TopicID == topicID && t.UserID == userID);
            }
        }
    }
}
