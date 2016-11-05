using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DotNetClub.Data.EntityFramework
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder AddEntityFrameworkRepository(this ContainerBuilder builder)
        {
            var assembly = Assembly.Load(new AssemblyName("DotNetClub.Data.EntityFramework"));
            foreach (var typeInfo in assembly.DefinedTypes)
            {
                if (typeInfo.Name.EndsWith("Repository"))
                {
                    builder.RegisterType(typeInfo.AsType()).AsImplementedInterfaces();
                }
            }

            return builder;
        }
    }
}
