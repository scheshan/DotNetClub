using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using System.Reflection;

namespace DotNetClub.Data.EntityFramework
{
    public class EntityFrameworkModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var assembly = Assembly.Load(new AssemblyName("DotNetClub.Data.EntityFramework"));
            foreach (var typeInfo in assembly.DefinedTypes)
            {
                if (typeInfo.Name.EndsWith("Repository"))
                {
                    builder.RegisterType(typeInfo.AsType()).AsImplementedInterfaces();
                }
            }
        }
    }
}
