using DotNetClub.Core;
using DotNetClub.Core.Service;
using DotNetClub.Web.ViewModels.Comment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Web.Controllers
{
    [Route("comment")]
    public class CommentController : Base.ControllerBase
    {
        private CommentService CommentService { get; set; }

        private ClientManager ClientManager { get; set; }

        public CommentController(CommentService commentService, ClientManager clientManager)
        {
            this.CommentService = commentService;
            this.ClientManager = clientManager;
        }

        [HttpPost("add")]
        [Filters.RequireLogin]
        public async Task<IActionResult> Add(AddCommentModel model)
        {
            if (!ModelState.IsValid)
            {
                string message = this.ModelState.Values.Where(t => t.ValidationState == ModelValidationState.Invalid)
                    .Select(t => t.Errors.FirstOrDefault())
                    .FirstOrDefault()
                    .ErrorMessage;

                return this.Notice(message);
            }

            var result = await this.CommentService.Add(model.TopicID, model.Content, model.ReplyTo, this.ClientManager.CurrentUser.ID);

            if (result.Success)
            {
                string url = this.Url.Action("Index", "Topic", new { id = model.TopicID });
                return this.Redirect($"{url}#comment{result.Data.Value}");
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
            var comment = await this.CommentService.Get(id);
            if (comment == null || comment.CreateUserID != ClientManager.CurrentUser.ID)
            {
                return this.Forbid();
            }

            await this.CommentService.Delete(id);

            return this.RedirectToAction("Index", "Topic", new { id = comment.TopicID });
        }

        [HttpGet("{id:int}/edit")]
        [Filters.RequireLogin]
        public async Task<IActionResult> Edit(int id)
        {
            var comment = await this.CommentService.Get(id);
            if (comment == null || comment.CreateUserID != ClientManager.CurrentUser.ID)
            {
                return this.Forbid();
            }

            return this.View("Edit", comment);
        }
    }
}
