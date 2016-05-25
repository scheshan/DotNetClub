using DotNetClub.Core.Model;
using DotNetClub.Core.Model.Category;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Web.ViewModels.Home
{
    public class IndexViewModel
    {
        public PagedResult<Core.Entity.Topic> TopicList { get; set; }

        public List<SelectListItem> TabList { get; set; }

        public string Tab { get; set; }
    }
}
