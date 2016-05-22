using DotNetClub.Core.Entity;
using DotNetClub.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DotNetClub.Core.Service
{
    public class TopicService
    {
        private Data.ClubContext DbContext { get; set; }

        private CategoryService CategoryService { get; set; }

        public TopicService(Data.ClubContext dbContext, CategoryService categoryService)
        {
            this.DbContext = dbContext;
            this.CategoryService = categoryService;
        }

        public async Task<OperationResult<int?>> Add(string category, string title, string content, int createUser)
        {
            if (this.CategoryService.Get(category) == null)
            {
                return OperationResult<int?>.Failure("版块不存在");
            }

            var entity = new Topic
            {
                Category = category,
                Content = content,
                CreateDate = DateTime.Now,
                CreateUserID = createUser,
                Title = title,
                UpdateDate = DateTime.Now
            };
            this.DbContext.Add(entity);
            await this.DbContext.SaveChangesAsync();

            return new OperationResult<int?>(entity.ID);
        }

        public async Task<Topic> Get(int id)
        {
            var result = await this.CreateDefaultQuery()
                .SingleOrDefaultAsync(t => t.ID == id);

            this.FillModel(result);

            return result;
        }

        public async Task<List<Topic>> QueryRecentCreatedTopicList(int count, int userID, params int[] exclude)
        {
            var query = this.CreateDefaultQuery()
                .Where(t => t.CreateUserID == userID)
                .OrderByDescending(t => t.ID)
                .Take(count);

            if (exclude != null && exclude.Length > 0)
            {
                query = query.Where(t => !exclude.Contains(t.ID));
            }

            var result = await query.ToListAsync();

            this.FillModel(result.ToArray());

            return result;
        }

        public async Task<List<Topic>> QueryRecentCommentedTopicList(int count, int userID)
        {
            var commentedTopicIDList = await this.DbContext.Comments.Where(t => t.CreateUserID == userID && !t.IsDelete)
                .OrderByDescending(t => t.ID)
                .Select(t => t.TopicID)
                .Distinct()
                .Take(count)
                .ToListAsync();

            var topicList = await this.CreateDefaultQuery()
                .Where(t => commentedTopicIDList.Contains(t.ID))
                .ToListAsync();

            topicList = topicList.OrderBy(t => commentedTopicIDList.IndexOf(t.ID)).ToList();

            return topicList;
        }

        public async Task<PagedResult<Topic>> QueryCreatedTopicList(int userID, int pageIndex, int pageSize)
        {
            var query = this.CreateDefaultQuery()
                .Where(t => t.CreateUserID == userID)
                .OrderByDescending(t => t.ID);

            int total = query.Count();

            var topicList = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            this.FillModel(topicList.ToArray());

            return new PagedResult<Topic>(topicList, pageIndex, pageSize, total);
        }

        public async Task<PagedResult<Topic>> QueryCommentedTopicList(int userID, int pageIndex, int pageSize)
        {
            var topicIDQuery = this.DbContext.Comments.Where(t => t.CreateUserID == userID && !t.IsDelete)
                .OrderByDescending(t => t.ID)
                .Select(t => t.TopicID)
                .Distinct();

            int total = topicIDQuery.Count();

            var topicIDList = await topicIDQuery.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var topicList = await this.CreateDefaultQuery()
                .Where(t => topicIDList.Contains(t.ID))
                .ToListAsync();

            topicList = topicList.OrderBy(t => topicIDList.IndexOf(t.ID)).ToList();

            return new PagedResult<Topic>(topicList, pageIndex, pageSize, total);
        }

        public async Task<List<Topic>> QueryNoCommentTopicList(int count)
        {
            var query = this.CreateDefaultQuery()
                .Where(t => t.ReplyCount == 0)
                .OrderByDescending(t => t.ID)
                .Take(count);

            var result = await query.ToListAsync();

            this.FillModel(result.ToArray());

            return result;
        }

        public async Task<PagedResult<Topic>> Query(string category, bool? recommand, int pageIndex, int pageSize)
        {
            var query = this.CreateDefaultQuery();
            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(t => t.Category == category);
            }
            if (recommand.HasValue)
            {
                query = query.Where(t => t.Recommand == recommand.Value);
            }

            int total = query.Count();

            query = query.OrderByDescending(t => t.ID)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);

            var topicList = await query.ToListAsync();

            this.FillModel(topicList.ToArray());

            return new PagedResult<Topic>(topicList, pageIndex, pageSize, total);
        }

        public async Task IncreaseVisitCount(int topicID)
        {
            string sql = $"UPDATE Topic SET VisitCount=VisitCount+1 WHERE ID = {topicID}";
            await this.DbContext.Database.ExecuteSqlCommandAsync(sql);
        }

        /// <summary>
        /// 创建默认的查询对象
        /// </summary>
        /// <returns></returns>
        private IQueryable<Topic> CreateDefaultQuery()
        {
            return this.DbContext.Topics.Where(t => !t.IsDelete)
                .Include(t => t.LastReplyUser)
                .Include(t => t.CreateUser)
                .AsQueryable();
        }

        /// <summary>
        /// 填充实体里的数据
        /// </summary>
        /// <param name="topicList"></param>
        private void FillModel(params Topic[] topicList)
        {
            if (topicList != null && topicList.Length > 0)
            {
                var categoryList = this.CategoryService.All();

                foreach (var topic in topicList)
                {
                    topic.CategoryModel = categoryList.SingleOrDefault(t => t.Key == topic.Category);
                }
            }
        }
    }
}
