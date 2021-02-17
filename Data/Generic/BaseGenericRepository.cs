using DataSql.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataSql.Generic
{
    public interface IGeneric<T> where T : class
    {
        Task DeleteAsync(T entityToDelete);
        Task DeleteAsync(object id);
        IQueryable<TResult> Query<TResult>(
            Expression<Func<T, TResult>> select,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] includes);
        IQueryable<T> Query(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] includes);

        Task<T> GetByFieldsAsync(Expression<Func<T, bool>> filter = null);
        Task<T> GetByIdAsync(object id);
        IEnumerable<TResult> GetWithRawSql<TResult>(string query,
            params object[] parameters);
        Task<T> InsertAsync(T entity);
        Task<T> UpdateAsync(int id, T entityToUpdate);
        Task<T> UpdateAsync(T entityToUpdate);
    }
    public class BaseGeneric<T> : IGeneric<T> where T : class
    {
        private DbContext _db;

        private DbSet<T> _dbSet;

        public BaseGeneric(DataContext db)
        {
            _db = db;
            _dbSet = db.Set<T>();
        }

        public virtual IQueryable<TResult> Query<TResult>(
            Expression<Func<T, TResult>> select,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = Query(filter: filter, orderBy: orderBy, includes: includes);
            return (IQueryable<TResult>)query.Select(select);
        }

        public virtual IQueryable<T> Query(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (Expression<Func<T, object>> include in includes)
                query = query.Include(include);

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return query;
        }

        public virtual IEnumerable<TResult> GetWithRawSql<TResult>(
            string query,
            params object[] parameters)
        {
            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    cmd.CommandText = query;
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }

                    if (parameters.Count() > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    using (var dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            var schemaTable = dataReader.GetSchemaTable();
                            IEnumerable<string> columns = schemaTable == null
                                ? Enumerable.Empty<string>()
                                : schemaTable.Rows.OfType<DataRow>().Select(row => row["ColumnName"].ToString());

                            var row = new ExpandoObject() as IDictionary<string, object>;
                            foreach (var column in columns)
                            {
                                row.Add(column, dataReader[column]);
                            }
                            yield return (TResult)row;
                        }
                    }
                }
                finally
                {
                    if (cmd.Connection.State == ConnectionState.Open)
                    {
                        cmd.Connection.Close();
                    }
                }
            }
        }

        public virtual async Task<T> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }
        public virtual async Task<T> GetByFieldsAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.SingleOrDefaultAsync(filter);
        }

        public virtual async Task<T> InsertAsync(T entity)
        {
            _dbSet.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public virtual async Task DeleteAsync(object id)
        {
            T entityToDelete = await _dbSet.FindAsync(id);
            await DeleteAsync(entityToDelete);
            await _db.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(T entityToDelete)
        {
            if (_db.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
            await _db.SaveChangesAsync();
        }

        public virtual async Task<T> UpdateAsync(int id, T entityToUpdate)
        {
            T entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Attach(entityToUpdate);
                _db.Entry(entityToUpdate).State = EntityState.Modified;
                await _db.SaveChangesAsync();
            }
            return entityToUpdate;
        }

        public virtual async Task<T> UpdateAsync(T entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _db.Entry(entityToUpdate).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return entityToUpdate;
        }
    }
}

