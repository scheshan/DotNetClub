using DotNetClub.Core.Model.Topic;
using DotNetClub.Core.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Web.ViewModels.User
{
    public class IndexViewModel
    {
        public UserModel User { get; set; }

        public long CollectCount { get; set; }

        public List<TopicModel> RecentCreatedTopicList { get; set; }

        public List<TopicModel> RecentCommentedTopicList { get; set; }
    }
}
