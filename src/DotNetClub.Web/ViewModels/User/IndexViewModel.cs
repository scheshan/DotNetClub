using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Web.ViewModels.User
{
    public class IndexViewModel
    {
        public Core.Model.User.UserModel User { get; set; }

        public int CollectCount { get; set; }

        public List<Core.Entity.Topic> RecentCreatedTopicList { get; set; }

        public List<Core.Entity.Topic> RecentCommentedTopicList { get; set; }
    }
}
