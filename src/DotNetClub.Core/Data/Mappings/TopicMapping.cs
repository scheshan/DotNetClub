using DotNetClub.Core.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DotNetClub.Core.Data.Mappings
{
    public sealed class TopicMapping
    {
        public static void Map(EntityTypeBuilder<Topic> builder)
        {
            builder.ForSqlServerToTable("Topic");

            builder.HasKey(t => t.ID);
            builder.Property(t => t.ID).UseSqlServerIdentityColumn();

            builder.Property(t => t.Title).IsRequired().HasMaxLength(100);
            builder.Property(t => t.Content).IsRequired();
            builder.Property(t => t.Category).IsRequired().HasMaxLength(50);

            builder.HasOne(t => t.CreateUser).WithMany().HasForeignKey(t => t.CreateUserID);
            builder.HasOne(t => t.LastReplyUser).WithMany().HasForeignKey(t => t.LastReplyUserID);

            builder.Ignore(t => t.CategoryModel);
        }
    }
}
