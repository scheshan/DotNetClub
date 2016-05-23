using DotNetClub.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DotNetClub.Core.Entity;

namespace DotNetClub.Core.Service
{
    public class CommentService
    {
        private Data.ClubContext DbContext { get; set; }

        public CommentService(Data.ClubContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public async Task<OperationResult<int?>> Add(int topicID, string content, int? replyTo, int userID)
        {
            var topic = await this.DbContext.Topics.SingleOrDefaultAsync(t => t.ID == topicID && !t.IsDelete);

            if (topic == null)
            {
                return OperationResult<int?>.Failure("该主题不存在");
            }

            var entity = new Comment
            {
                Content = content,
                CreateDate = DateTime.Now,
                CreateUserID = userID,
                IsDelete = false,
                ReplyID = replyTo,
                TopicID = topicID
            };
            topic.ReplyCount++;
            topic.LastReplyDate = DateTime.Now;
            topic.LastReplyUserID = userID;

            this.DbContext.Add(entity);

            await this.DbContext.SaveChangesAsync();

            return new OperationResult<int?>(entity.ID);    
        }

        public async Task<List<Comment>> QueryByTopic(int topicID)
        {
            var query = this.DbContext.Comments.Where(t => !t.IsDelete)
                .Where(t => t.TopicID == topicID)
                .Include(t => t.CreateUser)
                .OrderBy(t => t.ID);

            return await query.ToListAsync();
        }

        public async Task<Comment> Get(int id)
        {
            return await this.DbContext.Comments.SingleOrDefaultAsync(t => t.ID == id && !t.IsDelete);
        }

        public async Task Delete(int id)
        {
            string sql = $"UPDATE Comment SET IsDelete=1 WHERE ID={id}";
            await this.DbContext.Database.ExecuteSqlCommandAsync(sql);
        }
    }
}
