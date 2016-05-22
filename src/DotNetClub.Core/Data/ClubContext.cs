using DotNetClub.Core.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetClub.Core.Data.Mappings;

namespace DotNetClub.Core.Data
{
    public class ClubContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Topic> Topics { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public ClubContext(DbContextOptions<ClubContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(UserMapping.Map);
            modelBuilder.Entity<Topic>(TopicMapping.Map);
            modelBuilder.Entity<Comment>(CommentMapping.Map);
        }
    }
}
