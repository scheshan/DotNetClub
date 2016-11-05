using Share.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Domain.Entity
{
    public class TopicCollect : IEntity
    {
        public long UserID { get; set; }

        public long TopicID { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
