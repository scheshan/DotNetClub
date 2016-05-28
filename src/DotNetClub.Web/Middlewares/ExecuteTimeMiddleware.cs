using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Diagnostics;

namespace DotNetClub.Web.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExecuteTimeMiddleware
    {
        private readonly RequestDelegate _next;

        public ExecuteTimeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var body = httpContext.Response.Body;

            var ms = new MemoryStream();
            httpContext.Response.Body = ms;

            Stopwatch sw = new Stopwatch();

            try
            {
                sw.Start();
                await _next.Invoke(httpContext);
                sw.Stop();
                httpContext.Response.Headers["ExecuteTime"] = sw.ElapsedMilliseconds.ToString();
                Console.WriteLine($"RequestUrl:{httpContext.Request.Path}, ExecuteTime:{sw.ElapsedMilliseconds}");
                ms.Position = 0;
                await ms.CopyToAsync(body);
            }
            finally
            {
                httpContext.Response.Body = body;
            }
        }
    }
}
