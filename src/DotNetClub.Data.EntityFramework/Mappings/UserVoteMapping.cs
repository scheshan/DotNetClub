using DotNetClub.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetClub.Data.EntityFramework.Mappings
{
    public sealed class UserVoteMapping
    {
        public static void Map(EntityTypeBuilder<UserVote> builder)
        {
            builder.ToTable("UserVote");

            builder.HasKey(t => new { t.UserID, t.CommentID });
        }
    }
}
