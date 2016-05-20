using DotNetClub.Core.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Web.Controllers
{
    public class HomeController : Controller
    {
        private CategoryService CategoryService { get; set; }

        public HomeController(CategoryService categoryService)
        {
            this.CategoryService = categoryService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var categoryList = this.CategoryService.All();
            return this.View();
        }
    }
}
