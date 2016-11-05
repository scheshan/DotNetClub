using DotNetClub.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetClub.Data.EntityFramework.Mappings
{
    public sealed class UserMapping
    {
        public static void Map(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(t => t.ID);

            builder.Property(t => t.ID).UseSqlServerIdentityColumn().ValueGeneratedOnAdd();
            builder.Property(t => t.UserName).IsRequired().HasMaxLength(20);
            builder.Property(t => t.Password).IsRequired().HasMaxLength(100);
            builder.Property(t => t.Email).IsRequired().HasMaxLength(100);
            builder.Property(t => t.Location).HasMaxLength(100);
            builder.Property(t => t.Signature).HasMaxLength(200);
            builder.Property(t => t.WebSite).HasMaxLength(100);
        }
    }
}
