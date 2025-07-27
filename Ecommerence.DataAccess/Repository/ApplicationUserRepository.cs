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
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private ApplicationDbContext _db { get; set; }

        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        // Burada ApplicationUser ile ilgili özel metotlar ekleyebilirsiniz.
        // Örneğin, kullanıcı bilgilerini güncelleme, silme vb. işlemler için metotlar ekleyebilirsiniz.
        public void UserGuncelle(ApplicationUser obj)
        {
            _db.applicationUsers.Update(obj);
        }
        
        
    }
    
    
}
