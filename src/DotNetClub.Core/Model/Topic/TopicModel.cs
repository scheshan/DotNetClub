using DotNetClub.Core.Model.Category;
using DotNetClub.Core.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Core.Model.Topic
{
    public class TopicModel
    {
        public long ID { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public CategoryModel Category { get; set; }

        public UserBasicModel CreateUser { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public bool IsTop { get; set; }

        public bool IsRecommand { get; set; }

        public bool IsLock { get; set; }

        public UserBasicModel LastReplyUser { get; set; }

        public DateTime? LastReplyDate { get; set; }

        public long Visits { get; set; }

        public long Comments { get; set; }
    }
}
