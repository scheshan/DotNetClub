using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DotNetClub.Data.EntityFramework
{
    public class ClubUnitOfWorkRegisteration : Shared.Infrastructure.UnitOfWork.EntityFramework.UnitOfWorkRegisteration<Context.ClubContext>
    {
        public override string Name => Domain.Consts.UnitOfWorkNames.EntityFramework;

        public override Assembly[] EntityAssemblies => new Assembly[] { Assembly.Load(new AssemblyName("DotNetClub.Domain")) };

        public override Assembly[] RepositoryAssemblies  => new Assembly[] { Assembly.Load(new AssemblyName("DotNetClub.Data.EntityFramework")) };
    }
}
