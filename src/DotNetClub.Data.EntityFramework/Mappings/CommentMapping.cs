using DotNetClub.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetClub.Data.EntityFramework.Mappings
{
    public sealed class CommentMapping
    {
        public static void Map(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comment");

            builder.HasKey(t => t.ID);
            builder.Property(t => t.ID).UseSqlServerIdentityColumn().ValueGeneratedOnAdd();

            builder.Property(t => t.Content).IsRequired();
        }
    }
}
