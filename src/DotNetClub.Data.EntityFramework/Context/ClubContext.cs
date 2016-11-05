using DotNetClub.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Share.Infrastructure.UnitOfWork.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Data.EntityFramework.Context
{
    public class ClubContext : EntityFrameworkContext
    {
        private IConfiguration Configuration { get; set; }

        public ClubContext(DbContextOptions options, IConfiguration configuration)
            : base(options)
        {
            this.Configuration = configuration;
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(this.Configuration["ConnectionString"], builder =>
            {
                builder.UseRowNumberForPaging();
                builder.MigrationsAssembly("DotNetBluc.Web");
            });
        }
    }
}
