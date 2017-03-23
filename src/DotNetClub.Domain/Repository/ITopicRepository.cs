using DotNetClub.Domain.Entity;
using DotNetClub.Domain.Model;
using Shared.Infrastructure.Model;
using Shared.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Domain.Repository
{
    public interface ITopicRepository : IRepository<Topic>
    {
        Task<List<Topic>> QueryByUser(long userID, int count, long[] exclude);

        Task<PagedResult<Topic>> QueryByUser(long userID, int pageIndex, int pageSize);

        Task<List<Topic>> QueryByUserComment(long userID, int count);

        Task<PagedResult<Topic>> QueryByUserComment(long userID, int pageIndex, int pageSize);

        Task<List<Topic>> QueryNoComment(int count);

        Task<PagedResult<Topic>> QueryByUserCollect(long userID, int pageIndex, int pageSize);

        Task<PagedResult<Topic>> Query(int pageIndex, int pageSize, string category = null, string keywords = null, bool? isRecommand = null, bool? isTop = null);
    }
}
