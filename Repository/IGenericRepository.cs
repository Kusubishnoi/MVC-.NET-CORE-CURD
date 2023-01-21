using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace Identity.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(object id);
        void Insert(T obj);
        void Update(T obj);
        void Delete(object id);
        void Save();
        public IQueryable<T> Search(Expression<Func<T, bool>> query);
    }
}
