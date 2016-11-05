using DotNetClub.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetClub.Data.EntityFramework.Mappings
{
    public sealed class TopicCollectMapping
    {
        public static void Map(EntityTypeBuilder<TopicCollect> builder)
        {
            builder.ToTable("TopicCollect");

            builder.HasKey(t => new { t.UserID, t.TopicID });
        }
    }
}
