using DotNetClub.Domain.Entity;
using DotNetClub.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.UnitOfWork.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetClub.Domain.Model;

namespace DotNetClub.Data.EntityFramework.Repository
{
    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        public CommentRepository(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {

        }

        public async Task<List<TopicComments>> QueryTopicComments(long[] idList)
        {
            var query = this.Set.Where(t => !t.IsDelete && idList.Contains(t.TopicID))
                .GroupBy(t => t.TopicID)
                .Select(t => new TopicComments
                {
                    TopicID = t.Key,
                    Comments = t.LongCount()
                });

            return await query.ToListAsync();
        }
    }
}
