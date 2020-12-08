using APISample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace APISample.Generics
{
    public interface IGeneric<T> where T : class
    {
        void Delete(T entityToDelete);
        void Delete(object id);
        IQueryable<TResult> Query<TResult>(
            Expression<Func<T, object>> select,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] includes);
        IQueryable<T> Query(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] includes);

        T GetByID(object id);
        IEnumerable<TResult> GetWithRawSql<TResult>(string query,
            params object[] parameters);
        T Insert(T entity);
        T Update(T entityToUpdate);
    }
}
