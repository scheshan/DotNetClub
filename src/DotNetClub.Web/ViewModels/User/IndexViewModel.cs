using DotNetClub.Core.Model.Topic;
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

        public List<TopicModel> RecentCreatedTopicList { get; set; }

        public List<TopicModel> RecentCommentedTopicList { get; set; }
    }
}
