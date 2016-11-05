using System;

namespace Share.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork: IDisposable
    {
        /// <summary>
        /// 传入仓储接口类型，获得该类型的仓储接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T CreateRepository<T>() where T : IRepository;

        /// <summary>
        /// 传入实体类型，获得提供了基础CRUD方法的默认仓储接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IRepository<T> CreateDefaultRepository<T>() where T : class, IEntity;

        ITransaction BeginTransaction();
    }
}
