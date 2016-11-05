using DotNetClub.Core.Model.Category;
using DotNetClub.Core.Model.Topic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Web.ViewModels.Topic
{
    public class PostViewModel
    {
        public SelectList CategoryList { get; set; }
        
        public SaveTopicModel Model { get; set; }

        public bool IsNew { get; set; }
    }
}
