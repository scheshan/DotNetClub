using DotNetClub.Core.Model.Category;
using DotNetClub.Core.Model.Topic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Web.ViewModels.Topic
{
    public class IndexViewModel
    {
        public TopicModel Topic { get; set; }

        public List<CommentItemModel> CommentList { get; set; }

        public bool CanOperate { get; set; }

        public bool IsCollected { get; set; }
    }
}
