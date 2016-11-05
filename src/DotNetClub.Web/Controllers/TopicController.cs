using DotNetClub.Core;
using DotNetClub.Core.Entity;
using DotNetClub.Core.Model.Topic;
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
    public class TopicController : ControllerBase
    {
        private CategoryService CategoryService { get; set; }

        private TopicService TopicService { get; set; }

        private CommentService CommentService { get; set; }

        private TopicCollectService TopicCollectService { get; set; }

        public TopicController(CategoryService categoryService, TopicService topicService, CommentService commentService, TopicCollectService topicCollectService)
        {
            this.CategoryService = categoryService;
            this.TopicService = topicService;
            this.CommentService = commentService;
            this.TopicCollectService = topicCollectService;
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> Index(long id)
        {
            var topic = await this.TopicService.Get(id);

            if (topic == null)
            {
                return this.NotFound();
            }

            ViewBag.Title = topic.Title;

            this.TopicService.IncreaseVisit(id);

            var vm = new IndexViewModel();
            vm.Topic = topic;
            vm.CommentList = await this.CommentService.QueryByTopic(id);
            vm.CanOperate = this.SecurityManager.CanOperateTopic(topic);

            if (this.SecurityManager.IsLogin)
            {
                vm.IsCollected = await this.TopicCollectService.IsCollected(topic.ID, this.SecurityManager.CurrentUser.ID);
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
            vm.Model = new SaveTopicModel();

            ViewBag.Title = "新主题";

            return this.View("Post", vm);
        }

        [HttpPost("new")]
        [Filters.RequireLogin]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New(SaveTopicModel model)
        {
            var vm = new PostViewModel();
            vm.CategoryList = new SelectList(this.CategoryService.All(), "Key", "Name");
            vm.Model = model;

            if (!ModelState.IsValid)
            {
                return this.Notice(Core.Resource.Messages.ModelStateNotValid);
            }

            var result = await this.TopicService.Add(model);

            if (result.Success)
            {
                return this.RedirectToAction("Index", "Topic", new { id = result.Data });
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

            if (topic == null || !this.SecurityManager.CanOperateTopic(topic))
            {
                return this.Forbid();
            }

            ViewBag.Title = "编辑主题";

            var vm = new PostViewModel();
            vm.CategoryList = new SelectList(this.CategoryService.All(), "Key", "Name", topic.Category);
            vm.Model = new SaveTopicModel
            {
                Category = topic.Category.Key,
                Content = topic.Content,
                Title = topic.Title
            };

            return this.View("Post", vm);
        }

        [HttpPost("{id:int}/edit")]
        [Filters.RequireLogin]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SaveTopicModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.Notice(Core.Resource.Messages.ModelStateNotValid);
            }

            var result = await this.TopicService.Edit(id, model);

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
        public async Task<IActionResult> Delete(long id)
        {
            var result = await this.TopicService.Delete(id);
            if (result.Success)
            {
                return this.RedirectToAction("Index", "Home");
            }
            else
            {
                return this.Forbid();
            }
        }

        //        [HttpGet("{id:int}/recommand")]
        //        [Filters.RequireLogin]
        //        public async Task<IActionResult> Recommand(int id)
        //        {
        //            var result = await this.TopicService.ToggleRecommand(id);

        //            if (result.Success)
        //            {
        //                return this.RedirectToAction("Index", "Topic", new { id = id });
        //            }
        //            else
        //            {
        //                return this.Notice(result.ErrorMessage);
        //            }
        //        }

        //        [HttpGet("{id:int}/top")]
        //        [Filters.RequireLogin]
        //        public async Task<IActionResult> Top(int id)
        //        {
        //            var result = await this.TopicService.ToggleTop(id);

        //            if (result.Success)
        //            {
        //                return this.RedirectToAction("Index", "Topic", new { id = id });
        //            }
        //            else
        //            {
        //                return this.Notice(result.ErrorMessage);
        //            }
        //        }

        //        [HttpGet("{id:int}/lock")]
        //        [Filters.RequireLogin]
        //        public async Task<IActionResult> Lock(int id)
        //        {
        //            var result = await this.TopicService.ToggleLock(id);

        //            if (result.Success)
        //            {
        //                return this.RedirectToAction("Index", "Topic", new { id = id });
        //            }
        //            else
        //            {
        //                return this.Notice(result.ErrorMessage);
        //            }
        //        }

        //        [HttpGet("{id:int}/collect")]
        //        [Filters.RequireLogin]
        //        public async Task<IActionResult> Collect(int id)
        //        {
        //            var result = await this.UserCollectService.Collect(id);

        //            if (result.Success)
        //            {
        //                return this.RedirectToAction("Index", "Topic", new { id = id });
        //            }
        //            else
        //            {
        //                return this.Notice(result.ErrorMessage);
        //            }
        //        }
    }
}
