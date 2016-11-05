using DotNetClub.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using DotNetClub.Core.Model.Topic;
using Share.Infrastructure.UnitOfWork;
using Share.Infrastructure.Model;
using Share.Infrastructure.Extensions;
using DotNetClub.Domain.Repository;
using DotNetClub.Domain.Consts;

namespace DotNetClub.Core.Service
{
    public class TopicService : ServiceBase
    {
        private CategoryService CategoryService { get; set; }

        public TopicService(IServiceProvider serviceProvider, CategoryService categoryService)
            : base(serviceProvider)
        {
            this.CategoryService = categoryService;
        }

        /// <summary>
        /// 创建主题
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Result<long>> Add(SaveTopicModel model)
        {
            if (this.CategoryService.Get(model.Category) == null)
            {
                return Result<long>.ErrorResult("版块不存在");
            }

            using (var uw = this.CreateUnitOfWork())
            {
                var entity = new Topic
                {
                    Category = model.Category,
                    Content = model.Content,
                    CreateDate = DateTime.Now,
                    CreateUser = SecurityManager.CurrentUser.ID,
                    Title = model.Title
                };

                await uw.InsertAsync(entity);

                return Result.SuccessResult(entity.ID);
            }
        }

        /// <summary>
        /// 获取指定ID的主题
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TopicModel> Get(long id)
        {
            using (var uw = this.CreateUnitOfWork())
            {
                var entity = await uw.GetAsync<Topic>(t => t.ID == id);

                if (entity == null)
                {
                    return null;
                }

                return (await this.Transform(entity)).First();
            }
        }

        /// <summary>
        /// 编辑主题
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Result<long>> Edit(long id, SaveTopicModel model)
        {
            if (this.CategoryService.Get(model.Category) == null)
            {
                return Result<long>.ErrorResult("版块不存在");
            }

            using (var uw = this.CreateUnitOfWork())
            {
                var entity = await uw.GetAsync<Topic>(t => t.ID == id);

                if (entity == null)
                {
                    return Result<long>.ErrorResult("主题不存在");
                }
                if (!this.SecurityManager.CanOperateTopic(entity))
                {
                    return Result<long>.ErrorResult("无权操作");
                }

                entity.Title = model.Title;
                entity.Category = model.Category;
                entity.Content = model.Content;
                entity.UpdateDate = DateTime.Now;

                await uw.UpdateAsync(entity);

                return Result.SuccessResult(entity.ID);
            }
        }

        /// <summary>
        /// 删除主题
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Result> Delete(long id)
        {
            using (var uw = this.CreateUnitOfWork())
            {
                var entity = await uw.GetAsync<Topic>(t => t.ID == id && !t.IsDelete);
                if (entity == null)
                {
                    return Result.ErrorResult("主题不存在");
                }

                if (!this.SecurityManager.CanOperateTopic(entity))
                {
                    return Result.ErrorResult("无权操作");
                }

                entity.IsDelete = true;
                await uw.UpdateAsync(entity);

                return Result.SuccessResult();
            }
        }

        /// <summary>
        /// 设置主题精华
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Result<bool>> SetRecommand(long id)
        {
            if (!this.SecurityManager.IsAdmin)
            {
                return Result<bool>.ErrorResult("无权操作");
            }

            using (var uw = this.CreateUnitOfWork())
            {
                var entity = await uw.GetAsync<Topic>(t => t.ID == id && !t.IsDelete);

                if (entity == null)
                {
                    return Result<bool>.ErrorResult("无权操作");
                }

                entity.IsRecommand = !entity.IsRecommand;
                await uw.UpdateAsync(entity);

                return Result.SuccessResult(entity.IsRecommand);
            }
        }

        /// <summary>
        /// 设置主题置顶
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Result<bool>> SetTop(long id)
        {
            if (!this.SecurityManager.IsAdmin)
            {
                return Result<bool>.ErrorResult("无权操作");
            }

            using (var uw = this.CreateUnitOfWork())
            {
                var entity = await uw.GetAsync<Topic>(t => t.ID == id && !t.IsDelete);

                if (entity == null)
                {
                    return Result<bool>.ErrorResult("无权操作");
                }

                entity.IsTop = !entity.IsTop;
                await uw.UpdateAsync(entity);

                return Result.SuccessResult(entity.IsRecommand);
            }
        }

        /// <summary>
        /// 设置主题锁定状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Result<bool>> SetLock(long id)
        {
            if (!this.SecurityManager.IsAdmin)
            {
                return Result<bool>.ErrorResult("无权操作");
            }

            using (var uw = this.CreateUnitOfWork())
            {
                var entity = await uw.GetAsync<Topic>(t => t.ID == id && !t.IsDelete);

                if (entity == null)
                {
                    return Result<bool>.ErrorResult("无权操作");
                }

                entity.IsLock = !entity.IsLock;
                await uw.UpdateAsync(entity);

                return Result.SuccessResult(entity.IsRecommand);
            }
        }

        /// <summary>
        /// 查询用户创建的主题，返回指定数量的数据
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="count"></param>
        /// <param name="exclude">可以排除不需要返回的文章ID</param>
        /// <returns></returns>
        public async Task<List<TopicModel>> QueryByUser(long userID, int count, params long[] exclude)
        {
            using (var uw = this.CreateUnitOfWork())
            {
                var entityList = await uw.CreateRepository<ITopicRepository>().QueryByUser(userID, count, exclude);

                return await this.Transform(entityList.ToArray());
            }
        }

        /// <summary>
        /// 查询用户创建的主题，返回分页数据
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<PagedResult<TopicModel>> QueryByUser(long userID, int pageIndex, int pageSize)
        {
            using (var uw = this.CreateUnitOfWork())
            {
                var result = await uw.CreateRepository<ITopicRepository>().QueryByUser(userID, pageIndex, pageSize);

                var modelList = await this.Transform(result.Data.ToArray());

                return PagedResult<TopicModel>.SuccessResult(modelList, result.PageIndex, result.PageSize, result.Total);
            }
        }

        /// <summary>
        /// 查询用户最新评论的主题
        /// </summary>
        /// <param name="count"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<List<TopicModel>> QueryByUserComment(long userID, int count)
        {
            using (var uw = this.CreateUnitOfWork())
            {
                var entityList = await uw.CreateRepository<ITopicRepository>().QueryByUserComment(userID, count);

                return await this.Transform(entityList.ToArray());
            }
        }

        /// <summary>
        /// 查询用户最新评论的主题，返回分页数据
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<PagedResult<TopicModel>> QueryByUserComment(long userID, int pageIndex, int pageSize)
        {
            using (var uw = this.CreateUnitOfWork())
            {
                var result = await uw.CreateRepository<ITopicRepository>().QueryByUserComment(userID, pageIndex, pageSize);

                var modelList = await this.Transform(result.Data.ToArray());

                return PagedResult<TopicModel>.SuccessResult(modelList, result.PageIndex, result.PageSize, result.Total);
            }
        }

        public async Task<List<TopicModel>> QueryNoComment(int count)
        {
            using (var uw = this.CreateUnitOfWork())
            {
                var entityList = await uw.CreateRepository<ITopicRepository>().QueryNoComment(count);

                return await this.Transform(entityList.ToArray());
            }
        }

        public async Task<PagedResult<TopicModel>> Query(int pageIndex, int pageSize, string category = null, string keywords = null, bool? isRecommand = null, bool? isTop = null)
        {
            using (var uw = this.CreateUnitOfWork())
            {
                var result = await uw.CreateRepository<ITopicRepository>().Query(pageIndex, pageSize, category, keywords, isRecommand, isTop);

                var modelList = await this.Transform(result.Data.ToArray());

                return PagedResult<TopicModel>.SuccessResult(modelList, result.PageIndex, result.PageSize, result.Total);
            }
        }

        public async Task<PagedResult<TopicModel>> QueryByUserCollect(long userID, int pageIndex, int pageSize)
        {
            using (var uw = this.CreateUnitOfWork())
            {
                var result = await uw.CreateRepository<ITopicRepository>().QueryByUserCollect(userID, pageIndex, pageSize);

                var modelList = await this.Transform(result.Data.ToArray());

                return PagedResult<TopicModel>.SuccessResult(modelList, result.PageIndex, result.PageSize, result.Total);
            }
        }

        public void IncreaseVisit(long topicID)
        {
            var redis = this.RedisProvider.GetDatabase();
            redis.HashIncrement(RedisKeys.TopicVisit, topicID, 1);
        }

        public async Task<List<TopicModel>> Transform(params Topic[] entityList)
        {
            if (entityList.IsEmptyCollection())
            {
                return new List<TopicModel>();
            }

            return new List<TopicModel>();
        }
    }
}
