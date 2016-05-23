using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetClub.Core
{
    public static class IAppBuilderExtensions
    {
        public static void UseClientManagerInitializer(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<InitClientManagerMiddleware>();
        }

    }
}
