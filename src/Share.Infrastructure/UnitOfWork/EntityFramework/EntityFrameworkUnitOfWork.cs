using Autofac;
using Autofac.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;

namespace Share.Infrastructure.UnitOfWork.EntityFramework
{
    public class EntityFrameworkUnitOfWork : UnitOfWorkBase
    {
        public DbContext Context { get; private set; }

        private IComponentContext ComponentContext { get; set; }

        public EntityFrameworkUnitOfWork(DbContext context, IComponentContext componentContext)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            this.Context = context;
            this.ComponentContext = componentContext;
        }

        public override ITransaction BeginTransaction()
        {
            var transaction = this.Context.Database.BeginTransaction();
            return new EntityFrameworkTransaction(transaction);
        }

        public override void Dispose()
        {
            base.Dispose();

            if (this.Context != null)
            {
                this.Context.Dispose();
            }
        }

        protected override T ResolveRepository<T>()
        {
            var ovList = new Parameter[]
                {
                    new TypedParameter(typeof(DbContext), this.Context)
                };
            var repository = this.ComponentContext.Resolve<T>(ovList);

            return repository;
        }

        protected override IRepository<T> ResolveDefaultRepository<T>()
        {
            var ovList = new Parameter[]
                {
                    new TypedParameter(typeof(DbContext), this.Context)
                };
            var repository = this.ComponentContext.Resolve<RepositoryBase<T>>(ovList);

            return repository;
        }
    }
}
