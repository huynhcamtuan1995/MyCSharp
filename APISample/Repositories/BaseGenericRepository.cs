using APISample.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace APISample.Repositories
{
    public class BaseGenericRepository<T> : IGenericRepository<T> where T : class
    {
        private DbContext _db;

        private DbSet<T> _dbSet;

        public BaseGenericRepository(DataContext db)
        {
            _db = db;
            _dbSet = db.Set<T>();
        }

        //public IQueryable<TResult> WithInfo<T, TProperty, TResult>(this IQueryable<T> q, Expression<Func<T, TProperty>> propertySelector, Expression<Func<T, TProperty, TResult>> resultSelector)
        //{
        //    ParameterExpression param = Expression.Parameter(typeof(T));
        //    InvocationExpression prop = Expression.Invoke(propertySelector, param);

        //    var lambda = Expression.Lambda<Func<T, TResult>>(Expression.Invoke(resultSelector, param, prop), param);
        //    return q.Select(lambda);
        //}
        public virtual IQueryable<T> Query(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (Expression<Func<T, object>> include in includes)
                query = query.Include(include);

            //if (select != null)
            //    query = (IQueryable<T>)query.Select(select);

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return query;
        }

        public virtual IEnumerable<T> GetWithRawSql(
            string query,
            params object[] parameters)
        {
            return _dbSet.FromSqlRaw(query, parameters).ToList();
        }

        public virtual T GetByID(object id)
        {
            return _dbSet.Find(id);
        }

        public virtual void Insert(T entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            T entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(T entityToDelete)
        {
            if (_db.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public virtual void Update(T entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _db.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}

