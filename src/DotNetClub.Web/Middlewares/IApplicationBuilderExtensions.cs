using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Web.Middlewares
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseClientManagerInitializer(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ClientManagerInitializerMiddleware>();
        }

        public static IApplicationBuilder UseExecuteTime(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExecuteTimeMiddleware>();
        }
    }
}
