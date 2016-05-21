using DotNetClub.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Web.ViewComponents
{
    public class LoginInfoPanelViewComponent : ViewComponent
    {
        private ClientManager ClientManager { get; set; }

        public LoginInfoPanelViewComponent(ClientManager clientManager)
        {
            this.ClientManager = clientManager;
        }

        public IViewComponentResult Invoke()
        {
            return this.View(this.ClientManager.CurrentUser);
        }
    }
}
