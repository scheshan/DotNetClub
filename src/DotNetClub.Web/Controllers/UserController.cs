using DotNetClub.Core.Service;
using DotNetClub.Web.ViewModels.User;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Web.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        private UserService UserService { get; set; }

        private TopicService TopicService { get; set; }

        public UserController(UserService userService, TopicService topicService)
        {
            this.UserService = userService;
            this.TopicService = topicService;
        }

        [HttpGet("{userName}")]
        public async Task<IActionResult> Index(string userName)
        {
            var user = await this.UserService.Get(userName);

            if (user == null)
            {
                return this.NotFound();
            }

            ViewBag.User = user;

            var vm = new IndexViewModel();
            vm.User = user;
            vm.RecentCreatedTopicList = await this.TopicService.QueryRecentCreatedTopicList(10, user.ID);
            vm.RecentCommentedTopicList = await this.TopicService.QueryRecentCommentedTopicList(10, user.ID);

            return this.View(vm);
        }

        [HttpGet("{userName}/topics")]
        public async Task<IActionResult> Topics(string userName, int page = 1)
        {
            var user = await this.UserService.Get(userName);
            if (user == null)
            {
                return this.NotFound();
            }
            ViewBag.User = user;

            if (page < 1)
            {
                page = 1;
            }        

            var topicResult = await this.TopicService.QueryCreatedTopicList(user.ID, page, 20);

            return this.View(topicResult);
        }

        [HttpGet("{userName}/comments")]
        public async Task<IActionResult> Comments(string userName, int page = 1)
        {
            var user = await this.UserService.Get(userName);
            if (user == null)
            {
                return this.NotFound();
            }
            ViewBag.User = user;

            if (page < 1)
            {
                page = 1;
            }

            var topicResult = await this.TopicService.QueryCommentedTopicList(user.ID, page, 20);

            return this.View(topicResult);
        }
    }
}
