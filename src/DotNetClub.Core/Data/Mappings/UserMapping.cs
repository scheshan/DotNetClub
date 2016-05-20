using DotNetClub.Core.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DotNetClub.Core.Data.Mappings
{
    public sealed class UserMapping
    {
        public static void Map(EntityTypeBuilder<User> builder)
        {
            builder.ForSqlServerToTable("User");
            builder.HasKey(t => t.ID);

            builder.Property(t => t.ID).UseSqlServerIdentityColumn();
            builder.Property(t => t.UserName).IsRequired().HasMaxLength(20);
            builder.Property(t => t.Password).IsRequired().HasMaxLength(500);
            builder.Property(t => t.Email).IsRequired().HasMaxLength(100);
            builder.Property(t => t.DisplayName).IsRequired().HasMaxLength(20);
            builder.Property(t => t.Location).HasMaxLength(200);
            builder.Property(t => t.Signature).HasMaxLength(500);
            builder.Property(t => t.WebSite).HasMaxLength(200);
        }
    }
}
