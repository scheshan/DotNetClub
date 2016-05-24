using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DotNetClub.Core.Entity;

namespace DotNetClub.Core.Data.Mappings
{
    public sealed class UserCollectMapping
    {
        public static void Map(EntityTypeBuilder<UserCollect> builder)
        {
            builder.ForSqlServerToTable("UserCollect");

            builder.HasKey(t => new { t.UserID, t.TopicID });
        }
    }
}
