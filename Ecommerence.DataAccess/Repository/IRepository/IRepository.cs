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
        IEnumerable<T> ButunVerileriGetir(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);  // verileri getirmek istediğimizde çalışır örneğin T category olursa List<Category> döner 
        // Expression<Func<T, bool>>? filter = null, bu kod ile filtreleme yapabiliriz, örneğin CategoryId = 1 gibi bir filtreleme yapabiliriz.
        // soru işareti koyarak null olabileceğini belirtiyoruz, yani filtre vermek zorunda değiliz. 
        // filter = null diyerek ise filtre vermediğimizde de tüm verileri getireceğini belirtiyoruz.
        T Get(Expression<Func<T, bool>>filter, string? includeProperties = null, bool tracked = false); // LINQ sorgusu
        void Add(T entity);
        void Remove(T entity);  
        void RemoveRange(IEnumerable<T> entity);
        void Update(T entity); // Güncelleme işlemi için bir metod tanımlanır.





    }
}
