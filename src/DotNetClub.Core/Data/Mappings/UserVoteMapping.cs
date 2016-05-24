using DotNetClub.Core.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DotNetClub.Core.Data.Mappings
{
    public sealed class UserVoteMapping
    {
        public static void Map(EntityTypeBuilder<UserVote> builder)
        {
            builder.ForSqlServerToTable("UserVote");

            builder.HasKey(t => new { t.UserID, t.CommentID });
        }
    }
}
