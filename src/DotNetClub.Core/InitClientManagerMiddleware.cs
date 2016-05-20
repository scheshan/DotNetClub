using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetClub.Core
{
    public class InitClientManagerMiddleware
    {
        private readonly RequestDelegate _next;

        public InitClientManagerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            var clientManager = httpContext.RequestServices.GetService<ClientManager>();
            clientManager.Init(httpContext);

            return _next(httpContext);
        }
    }
}
