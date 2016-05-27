using DotNetClub.Core;
using DotNetClub.Core.Model;
using DotNetClub.Core.Resource;
using DotNetClub.Core.Service;
using DotNetClub.Web.ViewModels.My;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotNetClub.Web.Controllers
{
    [Route("my")]
    [Filters.RequireLogin]
    public class MyController : Base.ControllerBase
    {
        private UserService UserService { get; set; }

        private ClientManager ClientManager { get; set; }

        private MessageService MessageService { get; set; }

        public MyController(UserService userService, ClientManager clientManager, MessageService messageService)
        {
            this.UserService = userService;
            this.ClientManager = clientManager;
            this.MessageService = messageService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewBag.User = this.ClientManager.CurrentUser;
            base.OnActionExecuting(context);
        }

        [HttpGet("settings")]
        public IActionResult Settings()
        {
            return this.View("Index");
        }

        [HttpPost("editsettings")]
        public async Task<IActionResult> EditSettings(EditSettingsModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.SettingsResult = OperationResult.Failure(Core.Resource.Messages.ModelStateNotValid);
                return this.View("Index");
            }

            await this.UserService.EditUserInfo(ClientManager.CurrentUser.ID, model.WebSite, model.Location, model.Signature);

            ViewBag.SettingsResult = new OperationResult();

            return this.View("Index");
        }

        [HttpPost("editpassword")]
        public async Task<IActionResult> EditPassword(EditPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PasswordResult = OperationResult.Failure(Core.Resource.Messages.ModelStateNotValid);
            }

            bool success = await this.UserService.EditPassword(ClientManager.CurrentUser.ID, model.OldPassword, model.NewPassword);

            if (success)
            {
                ViewBag.PasswordResult = new OperationResult();
            }
            else
            {
                ViewBag.PasswordResult = OperationResult.Failure("密码错误,请确认后再试");
            }

            return this.View("Index");
        }

        [HttpGet("messages")]
        public async Task<IActionResult> Messages(int page)
        {
            var vm = new MessagesViewModel();
            vm.UnreadMessageList = await this.MessageService.QueryUnreadMessageList(this.ClientManager.CurrentUser.ID);
            vm.HistoryMessageList = await this.MessageService.QueryHistoryMessgaeList(this.ClientManager.CurrentUser.ID, 20);

            return this.View(vm);
        }
    }
}
