using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetClub.Web.Filters
{
    public class RequireLoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var clientManager = context.HttpContext.RequestServices.GetService<Core.ClientManager>();

            if (clientManager.IsLogin)
            {
                base.OnActionExecuting(context);
            }
            else
            {
                context.Result = new Microsoft.AspNetCore.Mvc.RedirectToActionResult("Login", "Account", null);
            }
        }
    }
}
