﻿using APISample.Models;
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

        public virtual IQueryable<TResult> Query<TResult>(
            Expression<Func<T, object>> select,
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
            return (IEnumerable<TResult>)_dbSet.FromSqlRaw(query, parameters).ToList();
        }

        public virtual T GetByID(object id)
        {
            return _dbSet.Find(id);
        }

        public virtual T Insert(T entity)
        {
            _dbSet.Add(entity);
            return entity;
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

        public virtual T Update(T entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _db.Entry(entityToUpdate).State = EntityState.Modified;
            return entityToUpdate;
        }

    }
}

