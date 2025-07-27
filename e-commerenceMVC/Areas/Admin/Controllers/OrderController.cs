using Ecommerence.DataAccess.Repository.IRepository;
using Ecommerence.Models;
using Ecommerence.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace e_commerenceMVC.Areas.Admin.Controllers
{
    [Area("admin")]

    public class OrderController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork; // IUnitOfWork arayüzü üzerinden veritabanı işlemlerini gerçekleştirmek için kullanılır.
        }
        public IActionResult Index()
        {
            return View();
        }







        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            
            List<OrderHeader> OrderHeaderList = _unitOfWork.orderHeader.ButunVerileriGetir(includeProperties: "ApplicationUser").ToList(); // Tüm sipariş başlıklarını alır.
            
            return Json(new { data = OrderHeaderList }); // Ürün listesini JSON formatında döner.
        }





        #endregion

    }
        }

    
    
