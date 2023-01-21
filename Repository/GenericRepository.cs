using Identity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Identity.Repository
{
   
        public class GenericRepository<T> : IGenericRepository<T> where T : class
        {
            private AppIdentityDbContext _context = null;
            private DbSet<T> table = null;

            //public GenericRepository()
            //{
            //    this._context = new AppIdentityDbContext();
            //    table = _context.Set<T>();
            //}

            public GenericRepository(AppIdentityDbContext _context)
            {
                this._context = _context;
                table = _context.Set<T>();
            }

            public IEnumerable<T> GetAll()
            {
                return _context.Set<T>().ToList();
            }

            public T GetById(object id)
            {
                var temp=table.Find(id);

            return temp;
            }

            public void Insert(T obj)
            {
                table.Add(obj);
            }

            public void Update(T obj)
            {
                table.Attach(obj);
                _context.Entry(obj).State = EntityState.Modified;
            }

            public void Delete(object id)
            {
                //T existing = table.Find(id);
                table.Remove((T)id);
            }

            public void Save()
            {
                _context.SaveChanges();
            }
        public IQueryable<T> Search(Expression<Func<T, bool>> query)
        {
            return table.Where(query);
        }
    }
    }

