using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerence.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> ButunVerileriGetir();  // verileri getirmek istediğimizde çalışır örneğin T category olursa List<Category> döner 
        T Get(Expression<Func<T, bool>>filter); // LINQ sorgusu
        void Add(T entity);
        void Remove(T entity);  
        void RemoveRange(IEnumerable<T> entity);





    }
}
