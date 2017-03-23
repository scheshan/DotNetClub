using DotNetClub.Domain.Entity;
using DotNetClub.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.UnitOfWork.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Infrastructure.Extensions;

namespace DotNetClub.Data.EntityFramework.Repository
{
    public class MessageRepository : RepositoryBase<Message>, IMessageRepository
    {
        public MessageRepository(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {

        }

        public async Task MarkAsRead(long userID, long[] messageIDList)
        {
            if (messageIDList.IsEmptyCollection())
            {
                return;
            }

            string sql = @"UPDATE [Message] SET IsRead=1 WHERE ToUserID={0} AND ID IN ({1})";

            string idStr = string.Join(",", messageIDList);

            sql = string.Format(sql, userID.ToString(), idStr);

            await this.Context.Database.ExecuteSqlCommandAsync(sql);
        }

        public async Task<List<Message>> QueryHistory(long userID, int count)
        {
            var query = this.Set.Where(t => t.IsRead == true && t.ToUserID == userID);

            return await query.OrderByDescending(t => t.ID).Take(count).ToListAsync();
        }
    }
}
