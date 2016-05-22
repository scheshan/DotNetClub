using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Web.ViewModels.User
{
    public class IndexViewModel
    {
        public Core.Entity.User User { get; set; }

        public List<Core.Entity.Topic> RecentTopicList { get; set; }
    }
}
