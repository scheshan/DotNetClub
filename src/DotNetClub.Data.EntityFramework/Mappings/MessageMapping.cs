using DotNetClub.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetClub.Data.EntityFramework.Mappings
{
    public sealed class MessageMapping
    {
        public static void Map(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Message");

            builder.HasKey(t => t.ID);

            builder.Property(t => t.ID).UseSqlServerIdentityColumn().ValueGeneratedOnAdd();
        }
    }
}
