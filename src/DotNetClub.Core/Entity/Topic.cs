using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Core.Entity
{
    public class Topic
    {
        public int ID { get; set; }

        public string Category { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public int CreateUserID { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public DateTime? LastReplyDate { get; set; }

        public bool Lock { get; set; }

        public bool Recommand { get; set; }

        public bool Top { get; set; }

        public bool IsDelete { get; set; }

        public int VisitCount { get; set; }

        public int ReplyCount { get; set; }

        public int CollectCount { get; set; }

        public int? LastReplyUserID { get; set; }

        public virtual User CreateUser { get; set; }

        public virtual User LastReplyUser { get; set; }

        public Model.Category.CategoryModel CategoryModel { get; set; }
    }
}
