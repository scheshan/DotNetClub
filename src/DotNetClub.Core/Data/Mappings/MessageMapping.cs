using DotNetClub.Core.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DotNetClub.Core.Data.Mappings
{
    public sealed class MessageMapping
    {
        public static void Map(EntityTypeBuilder<Message> builder)
        {
            builder.ForSqlServerToTable("Message");

            builder.HasKey(t => t.ID);

            builder.HasOne(t => t.Topic).WithMany().HasForeignKey(t => t.TopicID);
            builder.HasOne(t => t.Comment).WithMany().HasForeignKey(t => t.CommentID);
            builder.HasOne(t => t.FromUser).WithMany().HasForeignKey(t => t.FromUserID);
            builder.HasOne(t => t.ToUser).WithMany().HasForeignKey(t => t.ToUserID);
        }
    }
}
