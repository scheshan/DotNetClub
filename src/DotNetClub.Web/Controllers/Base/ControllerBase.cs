using DotNetClub.Web.ViewModels.Notice;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Web.Controllers.Base
{
    public class ControllerBase : Controller
    {
        protected virtual IActionResult Notice(NoticeViewModel vm)
        {
            return this.View("_Notice", vm);
        }

        protected virtual IActionResult Notice(string message)
        {
            var vm = new NoticeViewModel(message);
            return this.Notice(vm);
        }

        protected new IActionResult Forbid()
        {
            return this.Notice("无权操作");
        }
    }
}
