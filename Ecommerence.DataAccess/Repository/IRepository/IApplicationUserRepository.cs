using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerence.Models;

namespace Ecommerence.DataAccess.Repository.IRepository
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        public void UserGuncelle(ApplicationUser obj); // ApplicationUser güncelleme işlemi için bir metod tanımlanır.
        public ApplicationUser GetById(string id); // ApplicationUser'ı Id'sine göre getirmek için bir metod tanımlanır.


    }
}
