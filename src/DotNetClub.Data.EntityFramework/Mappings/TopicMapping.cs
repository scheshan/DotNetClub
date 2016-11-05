using DotNetClub.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetClub.Data.EntityFramework.Mappings
{
    public sealed class TopicMapping
    {
        public static void Map(EntityTypeBuilder<Topic> builder)
        {
            builder.ToTable("Topic");

            builder.HasKey(t => t.ID);
            builder.Property(t => t.ID).UseSqlServerIdentityColumn().ValueGeneratedOnAdd();

            builder.Property(t => t.Title).IsRequired().HasMaxLength(100);
            builder.Property(t => t.Content).IsRequired();
            builder.Property(t => t.Category).IsRequired().HasMaxLength(50);
        }
    }
}
