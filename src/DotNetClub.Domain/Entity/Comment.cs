using Share.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Domain.Entity
{
    public class Comment : IEntity
    {
        public int ID { get; set; }

        public int TopicID { get; set; }

        public int? ReplyID { get; set; }

        public string Content { get; set; }

        public bool IsDelete { get; set; }

        public int Ups { get; set; }

        public int CreateUserID { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
