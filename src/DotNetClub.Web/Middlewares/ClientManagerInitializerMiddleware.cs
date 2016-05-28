using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using DotNetClub.Core;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetClub.Web.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ClientManagerInitializerMiddleware
    {
        private readonly RequestDelegate _next;

        public ClientManagerInitializerMiddleware(RequestDelegate next)
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
