using DotNetClub.Core.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Web.ViewComponents
{
    public class UserInfoPanelViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(User user)
        {
            return this.View(user);
        }
    }
}
