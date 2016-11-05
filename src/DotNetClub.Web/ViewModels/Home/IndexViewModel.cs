using DotNetClub.Core.Model.Category;
using DotNetClub.Core.Model.Topic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Share.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Web.ViewModels.Home
{
    public class IndexViewModel
    {
        public PagedResult<TopicModel> TopicList { get; set; }

        public List<SelectListItem> TabList { get; set; }

        public string Tab { get; set; }
    }
}
