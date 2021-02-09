using DataSql.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;

namespace DataSql.Generic
{
    public interface IGeneric<T> where T : class
    {
        void Delete(T entityToDelete);
        void Delete(object id);
        IQueryable<TResult> Query<TResult>(
            Expression<Func<T, TResult>> select,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] includes);
        IQueryable<T> Query(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] includes);

        T GetByFields(Expression<Func<T, bool>> filter = null);
        T GetByID(object id);
        IEnumerable<TResult> GetWithRawSql<TResult>(string query,
            params object[] parameters);
        T Insert(T entity);
        T Update(T entityToUpdate);
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

        public virtual T GetByID(object id)
        {
            return _dbSet.Find(id);
        }
        public virtual T GetByFields(Expression<Func<T, bool>> filter)
        {
            return _dbSet.SingleOrDefault(filter);
        }

        public virtual T Insert(T entity)
        {
            _dbSet.Add(entity);
            _db.SaveChanges();
            return entity;
        }

        public virtual void Delete(object id)
        {
            T entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
            _db.SaveChanges();
        }

        public virtual void Delete(T entityToDelete)
        {
            if (_db.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
            _db.SaveChanges();
        }

        public virtual T Update(T entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _db.Entry(entityToUpdate).State = EntityState.Modified;
            _db.SaveChanges();
            return entityToUpdate;
        }
    }
}

