using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Share.Infrastructure.UnitOfWork
{
    public static class UnitOfWorkExtensions
    {
        #region sync methods

        public static void Insert<T>(this IUnitOfWork uw, T entity)
            where T : class, IEntity
        {
            uw.CreateDefaultRepository<T>().Insert(entity);
        }

        public static long InsertAndReturnIdentity<T>(this IUnitOfWork uw, T entity)
            where T : class, IEntity
        {
            return uw.CreateDefaultRepository<T>().InsertAndReturnIdentity(entity);
        }

        public static void InsertAll<T>(this IUnitOfWork uw, IEnumerable<T> entityList)
            where T : class, IEntity
        {
            uw.CreateDefaultRepository<T>().InsertAll(entityList);
        }

        public static List<T> All<T>(this IUnitOfWork uw)
            where T : class, IEntity
        {
            return uw.CreateDefaultRepository<T>().All();
        }

        public static List<T> Query<T>(this IUnitOfWork uw, Expression<Func<T, bool>> predicate)
            where T : class, IEntity
        {
            return uw.CreateDefaultRepository<T>().Query(predicate);
        }

        public static int Delete<T>(this IUnitOfWork uw, Expression<Func<T, bool>> predicate)
            where T : class, IEntity
        {
            return uw.CreateDefaultRepository<T>().Delete(predicate);
        }

        public static int Delete<T>(this IUnitOfWork uw, params T[] entityList)
            where T : class, IEntity
        {
            return uw.CreateDefaultRepository<T>().Delete(entityList);
        }

        public static T Get<T>(this IUnitOfWork uw, Expression<Func<T, bool>> predicate)
            where T : class, IEntity
        {
            return uw.CreateDefaultRepository<T>().Get(predicate);
        }

        public static int Update<T>(this IUnitOfWork uw, object updateOnly, Expression<Func<T, bool>> predicate)
            where T : class, IEntity
        {
            return uw.CreateDefaultRepository<T>().Update(updateOnly, predicate);
        }

        public static int Update<T>(this IUnitOfWork uw, T entity)
            where T : class, IEntity
        {
            return uw.CreateDefaultRepository<T>().Update(entity);
        }

        public static bool Exist<T>(this IUnitOfWork uw, Expression<Func<T, bool>> predicate)
            where T : class, IEntity
        {
            return uw.CreateDefaultRepository<T>().Exist(predicate);
        }

        public static List<TProperty> Column<T, TProperty>(this IUnitOfWork uw, Expression<Func<T, bool>> predicate, Expression<Func<T, TProperty>> propertySelector)
            where T : class, IEntity
        {
            return uw.CreateDefaultRepository<T>().Column(predicate, propertySelector);
        }

        public static long Count<T>(this IUnitOfWork uw)
            where T : class, IEntity
        {
            return uw.CreateDefaultRepository<T>().Count();
        }

        public static long Count<T>(this IUnitOfWork uw, Expression<Func<T, bool>> predicate)
            where T : class, IEntity
        {
            return uw.CreateDefaultRepository<T>().Count(predicate);
        }

        #endregion

        #region async methods

        public static async Task InsertAsync<T>(this IUnitOfWork uw, T entity)
            where T : class, IEntity
        {
            await uw.CreateDefaultRepository<T>().InsertAsync(entity);
        }

        public static async Task<long> InsertAndReturnIdentityAsync<T>(this IUnitOfWork uw, T entity)
            where T : class, IEntity
        {
            return await uw.CreateDefaultRepository<T>().InsertAndReturnIdentityAsync(entity);
        }

        public static async Task InsertAllAsync<T>(this IUnitOfWork uw, IEnumerable<T> entityList)
            where T : class, IEntity
        {
            await uw.CreateDefaultRepository<T>().InsertAllAsync(entityList);
        }

        public static async Task<List<T>> AllAsync<T>(this IUnitOfWork uw)
            where T : class, IEntity
        {
            return await uw.CreateDefaultRepository<T>().AllAsync();
        }

        public static async Task<List<T>> QueryAsync<T>(this IUnitOfWork uw, Expression<Func<T, bool>> predicate)
            where T : class, IEntity
        {
            return await uw.CreateDefaultRepository<T>().QueryAsync(predicate);
        }

        public static async Task<int> DeleteAsync<T>(this IUnitOfWork uw, Expression<Func<T, bool>> predicate)
            where T : class, IEntity
        {
            return await uw.CreateDefaultRepository<T>().DeleteAsync(predicate);
        }

        public static async Task<int> DeleteAsync<T>(this IUnitOfWork uw, params T[] entityList)
            where T : class, IEntity
        {
            return await uw.CreateDefaultRepository<T>().DeleteAsync(entityList);
        }

        public static async Task<T> GetAsync<T>(this IUnitOfWork uw, Expression<Func<T, bool>> predicate)
            where T : class, IEntity
        {
            return await uw.CreateDefaultRepository<T>().GetAsync(predicate);
        }

        public static async Task<int> UpdateAsync<T>(this IUnitOfWork uw, object updateOnly, Expression<Func<T, bool>> predicate)
            where T : class, IEntity
        {
            return await uw.CreateDefaultRepository<T>().UpdateAsync(updateOnly, predicate);
        }

        public static async Task<int> UpdateAsync<T>(this IUnitOfWork uw, T entity)
            where T : class, IEntity
        {
            return await uw.CreateDefaultRepository<T>().UpdateAsync(entity);
        }

        public static async Task<bool> ExistAsync<T>(this IUnitOfWork uw, Expression<Func<T, bool>> predicate)
            where T : class, IEntity
        {
            return await uw.CreateDefaultRepository<T>().ExistAsync(predicate);
        }

        public static async Task<List<TProperty>> ColumnAsync<T, TProperty>(this IUnitOfWork uw, Expression<Func<T, bool>> predicate, Expression<Func<T, TProperty>> propertySelector)
            where T : class, IEntity
        {
            return await uw.CreateDefaultRepository<T>().ColumnAsync(predicate, propertySelector);
        }

        public static async Task<long> CountAsync<T>(this IUnitOfWork uw)
            where T : class, IEntity
        {
            return await uw.CreateDefaultRepository<T>().CountAsync();
        }

        public static async Task<long> CountAsync<T>(this IUnitOfWork uw, Expression<Func<T, bool>> predicate)
            where T : class, IEntity
        {
            return await uw.CreateDefaultRepository<T>().CountAsync(predicate);
        }

        #endregion
    }
}
