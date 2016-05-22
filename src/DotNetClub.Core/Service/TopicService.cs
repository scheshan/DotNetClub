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
            var result = await this.DbContext.Topics.Include(t => t.CreateUser).SingleOrDefaultAsync(t => t.ID == id && !t.IsDelete);

            this.FillModel(result);

            return result;
        }

        public async Task<List<Topic>> QueryByUser(int count, int createUser, params int[] exclude)
        {
            var query = this.DbContext.Topics.Where(t => !t.IsDelete && t.CreateUserID == createUser)
                .Include(t => t.CreateUser)
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

        public async Task<PagedResult<Topic>> QueryCreatedTopicList(int createUser, int pageIndex, int pageSize)
        {
            var query = this.DbContext.Topics.Where(t => !t.IsDelete && t.CreateUserID == createUser)
                .Include(t => t.CreateUser)
                .OrderByDescending(t => t.ID);

            int total = query.Count();

            var topicList = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            this.FillModel(topicList.ToArray());

            return new PagedResult<Topic>(topicList, pageIndex, pageSize, total);
        }

        public async Task<List<Topic>> QueryNoCommentTopicList(int count)
        {
            var query = this.DbContext.Topics.Where(t => !t.IsDelete && t.ReplyCount == 0)
                .Include(t => t.CreateUser)
                .OrderByDescending(t => t.ID)
                .Take(count);

            var result = await query.ToListAsync();

            this.FillModel(result.ToArray());

            return result;
        }

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
