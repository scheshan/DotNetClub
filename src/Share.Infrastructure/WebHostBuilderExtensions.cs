using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Autofac;

namespace Share.Infrastructure
{
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder UseInfrastructureFactory(this IWebHostBuilder builder)
        {
            builder.ConfigureServices(services => services.AddSingleton<IServiceProviderFactory<ContainerBuilder>, InfrastructureFactory>());

            return builder;
        }
    }
}
