using Autofac;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Share.Infrastructure.UnitOfWork
{
    public sealed class UnitOfWorkBuilder
    {
        private ContainerBuilder ContainerBuilder { get; set; }

        public UnitOfWorkBuilder(ContainerBuilder containerBuilder)
        {
            if (containerBuilder == null)
            {
                throw new ArgumentNullException(nameof(containerBuilder));
            }

            this.ContainerBuilder = containerBuilder;
        }

        public void AddEntityFramework<TContext>(string name, Action<DbContextOptionsBuilder> optionsAction)
            where TContext : EntityFramework.EntityFrameworkContext
        {
            ContainerBuilder.RegisterType<TContext>();
            var optionsBuilder = new DbContextOptionsBuilder<TContext>();
            optionsAction?.Invoke(optionsBuilder);
            ContainerBuilder.RegisterGeneric(typeof(EntityFramework.RepositoryBase<>));            

            string creatorName = Consts.UNIT_OF_WORK_CREATOR_PREFIX + name;

            this.ContainerBuilder.Register(c=>new EntityFramework.EntityFrameworkUnitOfWorkCreator<TContext>(c.Resolve<IComponentContext>(), optionsBuilder))
                .Named<IUnitOfWorkCreator>(creatorName)
                .SingleInstance();
        }
    }
}
