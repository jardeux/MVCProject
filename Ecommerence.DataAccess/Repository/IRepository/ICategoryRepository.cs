using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using e_commerenceMVC.Models;

namespace Ecommerence.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        public void Guncelle(Category obj); // Category güncelleme işlemi için bir metod tanımlanır.   
        
        


    }
}
