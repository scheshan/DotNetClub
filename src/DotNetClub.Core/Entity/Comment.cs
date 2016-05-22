using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Core.Entity
{
    public class Comment
    {
        public int ID { get; set; }

        public int TopicID { get; set; }

        public int? ReplyID { get; set; }

        public string Content { get; set; }

        public bool IsDelete { get; set; }

        public int Ups { get; set; }

        public int CreateUserID { get; set; }

        public DateTime CreateDate { get; set; }

        public virtual Topic Topic { get; set; }

        public virtual User CreateUser { get; set; }
    }
}
