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

        private UserVoteService UserVoteService { get; set; }

        public CommentController(CommentService commentService, ClientManager clientManager, UserVoteService userVoteService)
        {
            this.CommentService = commentService;
            this.ClientManager = clientManager;
            this.UserVoteService = userVoteService;
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

            var result = await this.CommentService.Add(model.TopicID, model.Content, model.ReplyTo);

            if (result.Success)
            {
                return this.Redirect(this.GenerateCommentLink(model.TopicID, result.Data.ID));
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
            var result = await this.CommentService.Delete(id);
            if (result.Success)
            {
                return this.RedirectToAction("Index", "Topic", new { id = result.Data.TopicID });
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
            var comment = await this.CommentService.Get(id);
            if (comment == null || !this.ClientManager.CanOperateComment(comment))
            {
                return this.Forbid();
            }

            return this.View("Edit", comment);
        }

        [HttpPost("{id:int}/edit")]
        [Filters.RequireLogin]
        public async Task<IActionResult> Edit(int id, EditCommentModel model)
        {
            var result = await this.CommentService.Edit(id, model.Content);

            if (result.Success)
            {
                return this.Redirect(this.GenerateCommentLink(result.Data.TopicID, result.Data.ID));
            }
            else
            {
                return this.Notice(result.ErrorMessage);
            }
        }

        private string GenerateCommentLink(int topicID, int commentID)
        {
            string url = this.Url.Action("Index", "Topic", new { id = topicID });
            return $"{url}#{commentID}";
        }

        [HttpPost("{id:int}/vote")]
        public async Task<IActionResult> Vote(int id)
        {
            if (!this.ClientManager.IsLogin)
            {
                return this.Content("login");
            }

            var result = await this.UserVoteService.Vote(id);

            if (result.Success)
            {
                return this.Content(result.Data.Value.ToString().ToLower());
            }
            else
            {
                return this.Content(result.ErrorMessage);
            }
        }
    }
}
