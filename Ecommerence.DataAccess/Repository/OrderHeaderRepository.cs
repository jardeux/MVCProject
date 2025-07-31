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
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private ApplicationDbContext _db { get; set; }
        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
