using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Share.Infrastructure.UnitOfWork
{
    public interface IRepository
    {

    }

    public interface IRepository<T> : IRepository
        where T : class, IEntity
    {
        #region sync methods

        void Insert(T entity);        

        long InsertAndReturnIdentity(T entity);        

        void InsertAll(IEnumerable<T> entityList);        
        
        List<T> All();        

        List<T> Query(Expression<Func<T, bool>> predicate);        

        int Delete(Expression<Func<T, bool>> predicate);        

        int Delete(params T[] entityList);

        T Get(Expression<Func<T, bool>> predicate);

        int Update(object updateOnly, Expression<Func<T, bool>> predicate);

        int Update(T entity);

        bool Exist(Expression<Func<T, bool>> predicate);

        List<TProperty> Column<TProperty>(Expression<Func<T, bool>> predicate, Expression<Func<T, TProperty>> propertySelector);

        long Count();

        long Count(Expression<Func<T, bool>> predicate);

        #endregion

        #region async methods

        Task InsertAsync(T entity);

        Task<long> InsertAndReturnIdentityAsync(T entity);

        Task InsertAllAsync(IEnumerable<T> entityList);

        Task<List<T>> AllAsync();

        Task<List<T>> QueryAsync(Expression<Func<T, bool>> predicate);

        Task<int> DeleteAsync(Expression<Func<T, bool>> predicate);

        Task<int> DeleteAsync(params T[] entityList);

        Task<T> GetAsync(Expression<Func<T, bool>> predicate);

        Task<int> UpdateAsync(object updateOnly, Expression<Func<T, bool>> predicate);

        Task<int> UpdateAsync(T entity);

        Task<bool> ExistAsync(Expression<Func<T, bool>> predicate);

        Task<List<TProperty>> ColumnAsync<TProperty>(Expression<Func<T, bool>> predicate, Expression<Func<T, TProperty>> propertySelector);

        Task<long> CountAsync();

        Task<long> CountAsync(Expression<Func<T, bool>> predicate);

        #endregion
    }
}
