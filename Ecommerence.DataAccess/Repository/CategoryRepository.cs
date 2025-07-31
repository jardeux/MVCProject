using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using e_commerenceMVC.DataAccess.Data;
using e_commerenceMVC.Models;
using Ecommerence.DataAccess.Repository.IRepository;
namespace Ecommerence.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Guncelle(Category obj)
        {
            _db.Categories.Update(obj);


        }
    }
}
