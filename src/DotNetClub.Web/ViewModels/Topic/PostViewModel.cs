using DotNetClub.Core.Model.Category;
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
        
        public PostModel Model { get; set; }

        public bool IsNew { get; set; }
    }
}
