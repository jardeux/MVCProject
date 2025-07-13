using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerence.Models;

namespace Ecommerence.DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
       void ProductGuncelle(Product obj); // Product güncelleme işlemi için bir metod tanımlanır.



    }
}
