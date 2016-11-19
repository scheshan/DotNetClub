using DotNetClub.Domain.Entity;
using DotNetClub.Domain.Model;
using Shared.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Domain.Repository
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<List<TopicComments>> QueryTopicComments(long[] idList);
    }
}
