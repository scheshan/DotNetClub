using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DotNetClub.Core.Enums;
using DotNetClub.Core.Entity;

namespace DotNetClub.Core.Service
{
    public class MessageService
    {
        private Data.ClubContext DbContext { get; set; }

        private ClientManager ClientManager { get; set; }

        public MessageService(Data.ClubContext dbContext, ClientManager clientManager)
        {
            this.DbContext = dbContext;
            this.ClientManager = clientManager;
        }

        public async Task<List<Message>> QueryUnreadMessageList(int userID)
        {
            return await this.CreateDefaultQuery()
                .Where(t => !t.IsRead && t.ToUserID == userID)
                .OrderByDescending(t => t.ID)
                .ToListAsync();
        }

        public async Task<List<Message>> QueryHistoryMessgaeList(int userID, int count)
        {
            return await this.CreateDefaultQuery()
                .Where(t => t.IsRead && t.ToUserID == userID)
                .OrderByDescending(t => t.ID)
                .ToListAsync();
        }

        public async Task MarkAsRead(int[] idList)
        {
            if (idList.Length == 0)
            {
                return;
            }

            var entityList = this.DbContext.Messages.Where(t => idList.Contains(t.ID) && t.ToUserID == this.ClientManager.CurrentUser.ID).ToList();
            foreach (var entity in entityList)
            {
                entity.IsRead = true;
            }

            await this.DbContext.SaveChangesAsync();
        }

        private IQueryable<Message> CreateDefaultQuery()
        {
            return this.DbContext.Messages.Include(t => t.Topic).Include(t => t.FromUser);
        }
    }
}
