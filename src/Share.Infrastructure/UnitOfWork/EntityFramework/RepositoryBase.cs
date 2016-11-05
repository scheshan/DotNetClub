using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq;

namespace Share.Infrastructure.UnitOfWork.EntityFramework
{
    /// <summary>
    /// 用于EntityFramework的仓储基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RepositoryBase<T> : IRepository<T>
        where T : class, IEntity
    {
        protected DbContext Context { get; private set; }

        protected DbSet<T> Set
        {
            get
            {
                return Context.Set<T>();
            }
        }

        public RepositoryBase(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            this.Context = context;
        }

        #region sync methods

        public virtual void Insert(T entity)
        {
            this.Set.Add(entity);
            this.Context.SaveChanges();
        }

        public virtual long InsertAndReturnIdentity(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual void InsertAll(IEnumerable<T> entityList)
        {
            this.Set.AddRange(entityList);
            this.Context.SaveChanges();
        }

        public virtual List<T> All()
        {
            return this.Context.Set<T>().ToList();
        }

        public virtual List<T> Query(Expression<Func<T, bool>> predicate)
        {
            return this.Context.Set<T>().Where(predicate).ToList();
        }

        public virtual int Delete(Expression<Func<T, bool>> predicate)
        {
            var entityList = this.Set.Where(predicate).ToList();
            this.Set.RemoveRange(entityList);
            return this.Context.SaveChanges();
        }

        public virtual int Delete(params T[] entityList)
        {
            this.Set.RemoveRange(entityList);
            return this.Context.SaveChanges();
        }

        public virtual T Get(Expression<Func<T, bool>> predicate)
        {
            return this.Set.Where(predicate).FirstOrDefault();
        }

        public virtual int Update(object updateOnly, Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public virtual bool Exist(Expression<Func<T, bool>> predicate)
        {
            return this.Set.Any(predicate);
        }

        public virtual int Update(T entity)
        {
            this.Set.Update(entity);
            return this.Context.SaveChanges();
        }

        public virtual List<TProperty> Column<TProperty>(Expression<Func<T, bool>> predicate, Expression<Func<T, TProperty>> propertySelector)
        {
            return this.Set.Where(predicate).Select(propertySelector).ToList();
        }

        public virtual long Count()
        {
            return this.Set.LongCount();
        }

        public virtual long Count(Expression<Func<T, bool>> predicate)
        {
            return this.Set.LongCount(predicate);
        }

        #endregion

        #region async methods

        public virtual async Task InsertAsync(T entity)
        {
            this.Set.Add(entity);

            await this.Context.SaveChangesAsync();
        }

        public virtual async Task<long> InsertAndReturnIdentityAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual async Task InsertAllAsync(IEnumerable<T> entityList)
        {
            this.Set.AddRange(entityList);
            await this.Context.SaveChangesAsync();
        }

        public virtual async Task<List<T>> AllAsync()
        {
            return await this.Set.ToListAsync();
        }

        public virtual async Task<List<T>> QueryAsync(Expression<Func<T, bool>> predicate)
        {
            return await this.Set.Where(predicate).ToListAsync();
        }

        public virtual async Task<int> DeleteAsync(Expression<Func<T, bool>> predicate)
        {
            var entityList = this.Set.Where(predicate).ToList();
            this.Set.RemoveRange(entityList);
            return await this.Context.SaveChangesAsync();
        }

        public virtual async Task<int> DeleteAsync(params T[] entityList)
        {
            this.Set.RemoveRange(entityList);
            return await this.Context.SaveChangesAsync();
        }

        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await this.Set.FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<int> UpdateAsync(object updateOnly, Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<int> UpdateAsync(T entity)
        {
            this.Set.Update(entity);
            return await this.Context.SaveChangesAsync();
        }

        public virtual async Task<bool> ExistAsync(Expression<Func<T, bool>> predicate)
        {
            return await this.Set.AnyAsync(predicate);
        }

        public virtual async Task<List<TProperty>> ColumnAsync<TProperty>(Expression<Func<T, bool>> predicate, Expression<Func<T, TProperty>> propertySelector)
        {
            var query = this.Set.Where(predicate).Select(propertySelector);

            return await query.ToListAsync();
        }

        public virtual async Task<long> CountAsync()
        {
            return await this.Set.LongCountAsync();
        }

        public virtual async Task<long> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await this.Set.LongCountAsync(predicate);
        }

        #endregion
    }
}
