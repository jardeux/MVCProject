using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using e_commerenceMVC.DataAccess.Data;
using e_commerenceMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerence.DataAccess.Repository
{
    public class Repository<T> : IRepository.IRepository<T> where T : class
    {
        
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public IEnumerable<T> ButunVerileriGetir()
        {
            IQueryable<T> query = dbSet;
            return query.ToList(); // IQueryable<T> döndürür, bu yüzden ToList() ile listeye çeviriyoruz.
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            return query.FirstOrDefault();

        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}
