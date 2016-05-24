using DotNetClub.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DotNetClub.Core.Entity;

namespace DotNetClub.Core.Service
{
    public class UserVoteService
    {
        private Data.ClubContext DbContext { get; set; }

        private ClientManager ClientManager { get; set; }

        public UserVoteService(Data.ClubContext dbContext, ClientManager clientManager)
        {
            this.DbContext = dbContext;
            this.ClientManager = clientManager;
        }

        public async Task<OperationResult<bool?>> Vote(int commentID)
        {
            var comment = await this.DbContext.Comments.SingleOrDefaultAsync(t => t.ID == commentID && !t.IsDelete);
            if (comment == null)
            {
                return OperationResult<bool?>.Failure("评论不存在");
            }

            bool up;

            var entity = await this.DbContext.UserVotes.SingleOrDefaultAsync(t => t.CommentID == commentID && t.UserID == this.ClientManager.CurrentUser.ID);
            if (entity == null)
            {
                entity = new UserVote
                {
                    CommentID = commentID,
                    CreateDate = DateTime.Now,
                    UserID = this.ClientManager.CurrentUser.ID
                };
                this.DbContext.Add(entity);
                up = true;
                comment.Ups++;
            }
            else
            {
                this.DbContext.UserVotes.Remove(entity);
                up = false;
                comment.Ups--;
            }

            await this.DbContext.SaveChangesAsync();

            return new OperationResult<bool?>(up);
        }

        public async Task<List<UserVote>> QueryByCommentAndUser(int[] commentID, int userID)
        {
            return await this.DbContext.UserVotes.Where(t => commentID.Contains(t.CommentID) && t.UserID == userID).ToListAsync();
        }
    }
}
