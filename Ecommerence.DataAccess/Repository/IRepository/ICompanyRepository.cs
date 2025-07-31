using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerence.Models;

namespace Ecommerence.DataAccess.Repository.IRepository
{
    public interface ICompanyRepository : IRepository<Company>
    {
    public void CompanyGuncelle(Company obj);
    }
}
