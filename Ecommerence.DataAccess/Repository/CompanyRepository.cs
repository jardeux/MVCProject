using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using e_commerenceMVC.DataAccess.Data;
using Ecommerence.DataAccess.Repository.IRepository;
using Ecommerence.Models;

namespace Ecommerence.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private ApplicationDbContext _db;
        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
        _db = db;
        }

        public void CompanyGuncelle(Company obj)
        {
            _db.Update(obj);    
        }
    }
}
