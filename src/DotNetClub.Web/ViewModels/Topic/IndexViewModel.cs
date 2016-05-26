using DotNetClub.Core.Model.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Web.ViewModels.Topic
{
    public class IndexViewModel
    {
        public Core.Entity.Topic Topic { get; set; }

        public List<CommentItemModel> CommentList { get; set; }

        public bool CanOperate { get; set; }

        public bool IsCollected { get; set; }
    }
}
