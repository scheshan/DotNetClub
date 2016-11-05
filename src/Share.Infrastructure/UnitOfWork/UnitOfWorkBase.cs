using System;
using System.Collections.Generic;

namespace Share.Infrastructure.UnitOfWork
{
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        private Dictionary<Type, object> Repositories { get; set; }

        public UnitOfWorkBase()
        {
            this.Repositories = new Dictionary<Type, object>();
        }

        public T CreateRepository<T>()
            where T : IRepository
        {
            Type repositoryType = typeof(T);

            if (this.Repositories.ContainsKey(repositoryType))
            {
                return (T)this.Repositories[repositoryType];
            }
            else
            {
                var repository = this.ResolveRepository<T>();

                this.Repositories.Add(repositoryType, repository);

                return repository;
            }
        }

        public virtual void Dispose()
        {
            if (this.Repositories != null)
            {
                this.Repositories.Clear();
            }
        }

        public IRepository<T> CreateDefaultRepository<T>() where T : class, IEntity
        {
            Type repositoryType = typeof(IRepository<T>);

            if (this.Repositories.ContainsKey(repositoryType))
            {
                return (IRepository<T>)this.Repositories[repositoryType];
            }
            else
            {
                var repository = this.ResolveDefaultRepository<T>();
                this.Repositories.Add(repositoryType, repository);

                return repository;
            }
        }

        public abstract ITransaction BeginTransaction();

        protected abstract T ResolveRepository<T>() where T : IRepository;

        protected abstract IRepository<T> ResolveDefaultRepository<T>() where T : class, IEntity;
    }
}
