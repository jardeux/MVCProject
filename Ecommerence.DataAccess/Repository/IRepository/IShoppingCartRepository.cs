using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using e_commerenceMVC.Models;
using Ecommerence.Models;

namespace Ecommerence.DataAccess.Repository.IRepository
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        public void Guncelle(ShoppingCart obj); // Category güncelleme işlemi için bir metod tanımlanır.   
        
    }
}
