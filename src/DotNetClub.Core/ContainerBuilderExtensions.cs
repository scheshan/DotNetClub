using Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DotNetClub.Core
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder AddCoreServices(this ContainerBuilder builder)
        {
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();

            var assembly = Assembly.Load(new AssemblyName("DotNetClub.Core"));
            foreach (var typeInfo in assembly.DefinedTypes)
            {
                if (typeInfo.Name.EndsWith("Service"))
                {
                    builder.RegisterType(typeInfo.AsType());
                }
            }

            AutoMapperConfig.Configure();

            return builder;
        }
    }
}
