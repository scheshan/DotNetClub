using DotNetClub.Domain.Entity;
using Share.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Domain.Repository
{
    public interface IMessageRepository : IRepository<Message>
    {
        Task<List<Message>> QueryHistory(long userID, int count);

        Task MarkAsRead(long userID, long[] messageIDList);
    }
}
