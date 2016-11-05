using DotNetClub.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetClub.Data.EntityFramework.Mappings
{
    public sealed class UserCollectMapping
    {
        public static void Map(EntityTypeBuilder<UserCollect> builder)
        {
            builder.ToTable("UserCollect");

            builder.HasKey(t => new { t.UserID, t.TopicID });
        }
    }
}
