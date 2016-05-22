using DotNetClub.Core.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DotNetClub.Core.Data.Mappings
{
    public sealed class CommentMapping
    {
        public static void Map(EntityTypeBuilder<Comment> builder)
        {
            builder.ForSqlServerToTable("Comment");

            builder.HasKey(t => t.ID);
            builder.Property(t => t.ID).UseSqlServerIdentityColumn();

            builder.Property(t => t.Content).IsRequired();

            builder.HasOne(t => t.Topic).WithMany().HasForeignKey(t => t.TopicID).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(t => t.CreateUser).WithMany().HasForeignKey(t => t.CreateUserID).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
