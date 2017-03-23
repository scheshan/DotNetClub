using DotNetClub.Domain.Entity;
using DotNetClub.Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Extensions;
using Shared.Infrastructure.UnitOfWork.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Infrastructure.Model;
using System;

namespace DotNetClub.Data.EntityFramework.Repository
{
    public class TopicRepository : RepositoryBase<Topic>, ITopicRepository
    {
        public TopicRepository(DbContext context)
            : base(context)
        {

        }

        public async Task<PagedResult<Topic>> Query(int pageIndex, int pageSize, string category, string keywords, bool? isRecommand, bool? isTop)
        {
            var query = this.CreateDefaultQuery();

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(t => t.Category == category);
            }
            if (!string.IsNullOrWhiteSpace(keywords))
            {
                query = query.Where(t => t.Title.Contains(keywords) || t.Content.Contains(keywords));
            }
            if (isRecommand.HasValue)
            {
                query = query.Where(t => t.IsRecommand == isRecommand.Value);
            }
            if (isTop.HasValue)
            {
                query = query.Where(t => t.IsTop == isTop.Value);
            }

            var total = await query.CountAsync();

            query = query.OrderByDescending(t => t.ID).Skip((pageIndex - 1) * pageSize).Take(pageSize);

            var entityList = await query.ToListAsync();

            return PagedResult<Topic>.SuccessResult(entityList, pageIndex, pageSize, total);
        }

        public async Task<PagedResult<Topic>> QueryByUser(long userID, int pageIndex, int pageSize)
        {
            var query = this.CreateDefaultQuery().Where(t => t.CreateUser == userID);

            var total = await query.CountAsync();

            query = query.OrderByDescending(t => t.ID).Skip((pageIndex - 1) * pageSize).Take(pageSize);

            var entityList = await query.ToListAsync();

            return PagedResult<Topic>.SuccessResult(entityList, pageIndex, pageSize, total);
        }

        public async Task<List<Topic>> QueryByUser(long userID, int count, long[] exclude)
        {
            var query = this.CreateDefaultQuery().Where(t => t.CreateUser == userID);

            if (!exclude.IsEmptyCollection())
            {
                query = query.Where(t => !exclude.Contains(t.ID));
            }

            return await query.OrderByDescending(t => t.ID).Take(count).ToListAsync();
        }

        public async Task<PagedResult<Topic>> QueryByUserCollect(long userID, int pageIndex, int pageSize)
        {
            var query = from topic in this.CreateDefaultQuery()
                        join tc in this.Context.Set<TopicCollect>() on topic.ID equals tc.TopicID
                        where tc.UserID == userID
                        orderby tc.CreateDate descending
                        select topic;

            var total = await query.CountAsync();

            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            var entityList = await query.ToListAsync();

            return PagedResult<Topic>.SuccessResult(entityList, pageIndex, pageSize, total);
        }

        public async Task<List<Topic>> QueryByUserComment(long userID, int count)
        {
            var query = from comment in this.Context.Set<Comment>()
                        join topic in this.CreateDefaultQuery() on comment.TopicID equals topic.ID
                        where comment.CreateUser == userID
                        orderby comment.CreateDate descending
                        select topic;

            return await query.Take(count).ToListAsync();
        }

        public async Task<PagedResult<Topic>> QueryByUserComment(long userID, int pageIndex, int pageSize)
        {
            var query = from comment in this.Context.Set<Comment>()
                        join topic in this.CreateDefaultQuery() on comment.TopicID equals topic.ID
                        where comment.CreateUser == userID
                        orderby comment.CreateDate descending
                        select topic;

            var total = await query.CountAsync();

            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            var entityList = await query.ToListAsync();

            return PagedResult<Topic>.SuccessResult(entityList, pageIndex, pageSize, total);
        }

        public async Task<List<Topic>> QueryNoComment(int count)
        {
            string sql = string.Format(@"
select top({0}) A.* from Topic A
left join Comment B on A.ID = B.TopicID
where B.ID is NULL
", count.ToString());

            return await this.Set.FromSql(sql).ToListAsync();
        }

        private IQueryable<Topic> CreateDefaultQuery()
        {
            return this.Set.Where(t => !t.IsDelete);
        }
    }
}
