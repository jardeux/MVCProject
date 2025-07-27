using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using e_commerenceMVC.DataAccess.Data;
using e_commerenceMVC.Models;
using Ecommerence.DataAccess.Repository.IRepository;
using Ecommerence.Models;
namespace Ecommerence.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private ApplicationDbContext _db;
        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Guncelle(ShoppingCart obj)
        {
            _db.shoppingCarts.Update(obj);


        }
    }
}
