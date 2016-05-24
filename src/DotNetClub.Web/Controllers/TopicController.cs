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
    public class TopicController : Base.ControllerBase
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
            vm.CommentList = new List<CommentItemModel>();
            vm.CanOperate = this.ClientManager.IsAdmin || (this.ClientManager.IsLogin && topic.CreateUserID == this.ClientManager.CurrentUser.ID);

            var commentEntityList = await this.CommentService.QueryByTopic(id);

            foreach (var commentEntity in commentEntityList)
            {
                var model = new CommentItemModel(commentEntity);
                model.CanOperate = this.ClientManager.IsAdmin || (this.ClientManager.IsLogin && commentEntity.CreateUserID == this.ClientManager.CurrentUser.ID);

                vm.CommentList.Add(model);
            }

            return this.View(vm);
        }

        [HttpGet("new")]
        [Filters.RequireLogin]
        public IActionResult New()
        {
            var vm = new PostViewModel();
            vm.IsNew = true;
            vm.CategoryList = new SelectList(this.CategoryService.All(), "Key", "Name");        
            vm.Model = new PostModel();

            return this.View("Post", vm);
        }

        [HttpPost("new")]
        [Filters.RequireLogin]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(PostModel model)
        {
            var vm = new PostViewModel();
            vm.CategoryList = new SelectList(this.CategoryService.All(), "Key", "Name");
            vm.Model = model;

            if (!ModelState.IsValid)
            {
                return this.Notice(Core.Resource.Messages.ModelStateNotValid);
            }
            
            var result = await this.TopicService.Add(model.Category, model.Title, model.Content, this.ClientManager.CurrentUser.ID);

            if (result.Success)
            {
                return this.RedirectToAction("Index", "Topic", new { id = result.Data.Value });
            }
            else
            {
                return this.Notice(result.ErrorMessage);
            }
        }

        [HttpGet("{id:int}/edit")]
        [Filters.RequireLogin]
        public async Task<IActionResult> Edit(int id)
        {
            var topic = await this.TopicService.Get(id);

            if (topic == null || !this.ClientManager.CanOperateTopic(topic))
            {
                return this.Forbid();
            }

            var vm = new PostViewModel();
            vm.CategoryList = new SelectList(this.CategoryService.All(), "Key", "Name", topic.Category);
            vm.Model = new PostModel
            {
                Category = topic.Category,
                Content = topic.Content,
                Title = topic.Title
            };

            return this.View("Post", vm);
        }

        [HttpPost("{id:int}/edit")]
        [Filters.RequireLogin]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PostModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.Notice(Core.Resource.Messages.ModelStateNotValid);
            }

            var result = await this.TopicService.Edit(id, model.Category, model.Title, model.Content);

            if (result.Success)
            {
                return this.RedirectToAction("Index", "Topic", new { id = id });
            }
            else
            {
                return this.Notice(result.ErrorMessage);
            }
        }

        [HttpGet("{id:int}/delete")]
        [Filters.RequireLogin]
        public async Task<IActionResult> Delete(int id)
        {
            var topic = await this.TopicService.Get(id);
            if (topic == null || topic.CreateUserID != this.ClientManager.CurrentUser.ID)
            {
                return this.Forbid();
            }

            await this.TopicService.Delete(id);

            return this.RedirectToAction("Index", "Home");
        }
    }
}
