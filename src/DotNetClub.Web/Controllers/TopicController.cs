using DotNetClub.Core;
using DotNetClub.Core.Service;
using DotNetClub.Web.ViewModels.Topic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Web.Controllers
{
    [Route("topic")]
    public class TopicController : Controller
    {
        private CategoryService CategoryService { get; set; }

        private TopicService TopicService { get; set; }

        private CommentService CommentService { get; set; }

        private ClientManager ClientManager { get; set; }

        public TopicController(CategoryService categoryService, TopicService topicService, CommentService commentService, ClientManager clientManager)
        {
            this.CategoryService = categoryService;
            this.TopicService = topicService;
            this.ClientManager = clientManager;
            this.CommentService = commentService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Index(int id)
        {
            var topic = await this.TopicService.Get(id);

            if (topic == null)
            {
                return this.NotFound();
            }

            await this.TopicService.IncreaseVisitCount(id);

            var vm = new IndexViewModel();
            vm.Topic = topic;
            vm.CommentList = await this.CommentService.QueryByTopic(id);

            return this.View(vm);
        }

        [HttpGet("new")]
        [Filters.RequireLogin]
        public IActionResult New()
        {
            var vm = new NewViewModel();

            vm.CategoryList = new SelectList(this.CategoryService.All(), "Key", "Name");        
            vm.Model = new NewModel();

            return this.View(vm);
        }

        [HttpPost("new")]
        [Filters.RequireLogin]
        public async Task<IActionResult> New(NewModel model)
        {
            var vm = new NewViewModel();
            vm.CategoryList = new SelectList(this.CategoryService.All(), "Key", "Name");
            vm.Model = model;

            if (!ModelState.IsValid)
            {
                vm.ErrorMessage = Core.Resource.Messages.ModelStateNotValid;
                return this.View(vm);
            }
            
            var result = await this.TopicService.Add(model.Category, model.Title, model.Content, this.ClientManager.CurrentUser.ID);

            if (result.Success)
            {
                return this.RedirectToAction("Index", "Topic", new { id = result.Data.Value });
            }
            else
            {
                vm.ErrorMessage = result.ErrorMessage;
                return this.View(vm);
            }
        }
    }
}
