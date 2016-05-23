using DotNetClub.Core.Service;
using DotNetClub.Web.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Web.Controllers
{
    public class HomeController : Controller
    {
        private static readonly string TAB_RECOMMAND = "recommand";

        private CategoryService CategoryService { get; set; }

        private TopicService TopicService { get; set; }

        public HomeController(CategoryService categoryService, TopicService topicService)
        {
            this.CategoryService = categoryService;
            this.TopicService = topicService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(string tab, int page = 1)
        {
            if (page < 1)
            {
                page = 1;
            }

            var vm = new IndexViewModel();

            vm.TabList = new List<SelectListItem>();
            vm.TabList.Add(new SelectListItem { Text = "全部", Value = "", Selected = string.IsNullOrWhiteSpace(tab) });
            vm.TabList.Add(new SelectListItem { Text = "精华", Value = TAB_RECOMMAND, Selected = string.Equals(tab, TAB_RECOMMAND, StringComparison.OrdinalIgnoreCase) });

            var categoryList = this.CategoryService.All();
            foreach (var categoryModel in categoryList)
            {
                vm.TabList.Add(new SelectListItem { Text = categoryModel.Name, Value = categoryModel.Key, Selected = string.Equals(categoryModel.Key, tab, StringComparison.OrdinalIgnoreCase) });
            }

            string category = null;
            bool? recommand = null;

            if (tab == TAB_RECOMMAND)
            {
                recommand = true;
            }
            else if (!string.IsNullOrWhiteSpace(tab))
            {
                category = tab;
            }

            var topicList = await this.TopicService.Query(category, recommand, page, 20);

            vm.TopicList = topicList;

            return this.View(vm);
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return this.View();
        }
    }
}
