using DotNetClub.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Data.EntityFramework.Context
{
    public class ClubContext : DbContext
    {
        public ClubContext(DbContextOptions<ClubContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Mappings.CommentMapping.Map(modelBuilder.Entity<Comment>());
            Mappings.CommentVoteMapping.Map(modelBuilder.Entity<CommentVote>());
            Mappings.MessageMapping.Map(modelBuilder.Entity<Message>());
            Mappings.TopicCollectMapping.Map(modelBuilder.Entity<TopicCollect>());
            Mappings.TopicMapping.Map(modelBuilder.Entity<Topic>());
            Mappings.UserMapping.Map(modelBuilder.Entity<User>());
        }
    }
}
