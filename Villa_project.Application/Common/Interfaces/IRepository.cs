using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Villa_project.Application.Common.Interfaces
{
    public interface IRepository < T> where T : class
    {
        void Add(T entity );
      //  void Update(T entity );
        void Remove(T entity );
      //  void Save(T entity);

        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, string includeProperties = null ,bool tracked=false);

        T Get(Expression<Func<T, bool>> filter = null, string includeProperties = null, bool tracked = false);

        bool Any(Expression<Func<T, bool>> filter = null);
    }
}
