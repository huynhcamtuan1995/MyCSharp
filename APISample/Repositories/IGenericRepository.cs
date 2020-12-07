using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace APISample.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        void Delete(T entityToDelete);
        void Delete(object id);
        IQueryable<T> Query(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] includes);
        T GetByID(object id);
        IEnumerable<T> GetWithRawSql(string query,
            params object[] parameters);
        void Insert(T entity);
        void Update(T entityToUpdate);
    }
}
