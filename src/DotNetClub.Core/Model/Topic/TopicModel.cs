using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Core.Model.Topic
{
    public class TopicModel
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public Category.CategoryModel Category { get; set; }

        public User.UserBaseModel CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public DateTime? LastReplyDate { get; set; }
    }
}
