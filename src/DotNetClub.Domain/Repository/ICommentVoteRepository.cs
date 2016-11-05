using DotNetClub.Domain.Entity;
using DotNetClub.Domain.Model;
using Share.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Domain.Repository
{
    public interface ICommentVoteRepository : IRepository<CommentVote>
    {
        Task<List<CommentVotes>> QueryCommentVotes(long[] idList);
    }
}
