using DotNetClub.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetClub.Data.EntityFramework.Mappings
{
    public sealed class CommentVoteMapping
    {
        public static void Map(EntityTypeBuilder<CommentVote> builder)
        {
            builder.ToTable("CommentVote");

            builder.HasKey(t => new { t.UserID, t.CommentID });
        }
    }
}
