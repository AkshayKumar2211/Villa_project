using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Villa_project.Application.Common.Interfaces;
using Villa_project.Domain.Entities;
using Villa_project.Infrastructure.Data;

namespace Villa_project.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> _dbset;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            _dbset= db.Set<T>();
        }
        public void Add(T entity)
        {
            _dbset.Add(entity);
        }

        public bool Any(Expression<Func<T, bool>> filter = null)
        {
           return _dbset.Any(filter);
        }

        public T Get(Expression<Func<T, bool>> filter , string includeProperties=null ,bool tracked=false)
        {
            IQueryable<T> query;
            if(tracked)
            {
                query=_dbset;
            }
            else
            {
                query= _dbset.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query=query.Include(includeProp);
                }
            }

            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, string includeProperties = null , bool tracked = false)
        {
            IQueryable<T> query;
            if (tracked)
            {
                query=_dbset;
            }
            else
            {
                query= _dbset.AsNoTracking();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query=query.Include(includeProp);
                }
            }

            return query.ToList();
        }

        public void Remove(T entity)
        {
            _dbset.Remove(entity);
        }

        
    }
}
