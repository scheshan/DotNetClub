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
using DotNetClub.Core.Model.User;

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
            ViewBag.Title = "设置";

            return this.View("Index");
        }

        [HttpPost("editsettings")]
        public async Task<IActionResult> EditSettings(EditUserInfoModel model)
        {
            ViewBag.Title = "设置";

            if (!ModelState.IsValid)
            {
                ViewBag.SettingsResult = OperationResult.Failure(Core.Resource.Messages.ModelStateNotValid);
                return this.View("Index");
            }

            await this.UserService.EditUserInfo(SecurityManager.CurrentUser.ID, model);

            ViewBag.SettingsResult = new OperationResult();

            return this.View("Index");
        }

        [HttpPost("editpassword")]
        public async Task<IActionResult> EditPassword(EditPasswordModel model)
        {
            ViewBag.Title = "设置";

            if (!ModelState.IsValid)
            {
                ViewBag.PasswordResult = OperationResult.Failure(Core.Resource.Messages.ModelStateNotValid);
            }

            var result = await this.UserService.EditPassword(SecurityManager.CurrentUser.ID, model.OldPassword, model.NewPassword);

            ViewBag.PasswordResult = result;

            return this.View("Index");
        }

        [HttpGet("messages")]
        public async Task<IActionResult> Messages(int page)
        {
            ViewBag.Title = "消息";

            var vm = new MessagesViewModel();
            vm.UnreadMessageList = await this.MessageService.QueryUnreadMessageList(this.ClientManager.CurrentUser.ID);
            vm.HistoryMessageList = await this.MessageService.QueryHistoryMessgaeList(this.ClientManager.CurrentUser.ID, 20);

            var unreadMessageIDList = vm.UnreadMessageList.Select(t => t.ID).ToArray();
            await this.MessageService.MarkAsRead(unreadMessageIDList);

            return this.View(vm);
        }
    }
}
