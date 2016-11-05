using Share.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Domain.Entity
{
    public class Topic : IEntity
    {
        public long ID { get; set; }

        public string Category { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public long CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public bool IsLock { get; set; }

        public bool IsRecommand { get; set; }

        public bool IsTop { get; set; }

        public bool IsDelete { get; set; }

        public DateTime? LastReplyDate { get; set; }

        public long? LastReplyUserID { get; set; }
    }
}
