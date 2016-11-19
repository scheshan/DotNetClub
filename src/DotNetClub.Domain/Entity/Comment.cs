using Shared.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Domain.Entity
{
    public class Comment : IEntity
    {
        public long ID { get; set; }

        public long TopicID { get; set; }

        public long? ReplyID { get; set; }

        public string Content { get; set; }

        public bool IsDelete { get; set; }

        public long CreateUser { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
