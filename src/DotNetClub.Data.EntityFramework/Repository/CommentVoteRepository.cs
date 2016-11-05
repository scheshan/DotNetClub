using DotNetClub.Domain.Entity;
using DotNetClub.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Share.Infrastructure.UnitOfWork.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetClub.Domain.Model;

namespace DotNetClub.Data.EntityFramework.Repository
{
    public class CommentVoteRepository : RepositoryBase<CommentVote>, ICommentVoteRepository
    {
        public CommentVoteRepository(DbContext context)
            : base(context)
        {

        }

        public async Task<List<CommentVotes>> QueryCommentVotes(long[] idList)
        {
            var query = this.Set.Where(t => idList.Contains(t.CommentID))
                .GroupBy(t => t.CommentID)
                .Select(t => new CommentVotes
                {
                    CommentID = t.Key,
                    Votes = t.LongCount()
                });

            return await query.ToListAsync();
        }
    }
}
