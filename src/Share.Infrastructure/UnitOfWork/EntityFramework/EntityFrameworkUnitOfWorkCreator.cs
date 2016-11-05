using Autofac;
using Autofac.Core;
using Microsoft.EntityFrameworkCore;
using System;

namespace Share.Infrastructure.UnitOfWork.EntityFramework
{
    internal class EntityFrameworkUnitOfWorkCreator<TContext> : IUnitOfWorkCreator
        where TContext : EntityFrameworkContext
    {
        private IComponentContext ComponentContext { get; set; }

        private DbContextOptionsBuilder<TContext> OptionsBuilder { get; set; }

        public EntityFrameworkUnitOfWorkCreator(IComponentContext componentContext, DbContextOptionsBuilder<TContext> optionsBuilder)
        {
            if (componentContext == null)
            {
                throw new ArgumentNullException(nameof(componentContext));
            }
            if (optionsBuilder == null)
            {
                throw new ArgumentNullException(nameof(optionsBuilder));
            }
            
            this.ComponentContext = componentContext;
            this.OptionsBuilder = optionsBuilder;
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            var contextParameter = new TypedParameter(typeof(DbContextOptions), this.OptionsBuilder.Options);
            var context = this.ComponentContext.Resolve<TContext>(contextParameter);
            
            return new EntityFrameworkUnitOfWork(context, this.ComponentContext.Resolve<IComponentContext>());
        }
    }

    public abstract class EntityFrameworkContext : DbContext
    {
        public EntityFrameworkContext(DbContextOptions options)
            : base(options)
        {

        }

        private EntityFrameworkContext()
        {

        }
    }
}
