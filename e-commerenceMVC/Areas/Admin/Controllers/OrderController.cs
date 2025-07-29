using System.Security.Claims;
using Ecommerence.DataAccess.Repository.IRepository;
using Ecommerence.Models;
using Ecommerence.Models.ViewModel;
using Ecommerence.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace e_commerenceMVC.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]

    public class OrderController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public OrderVM OrderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork; // IUnitOfWork arayüzü üzerinden veritabanı işlemlerini gerçekleştirmek için kullanılır.
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details(int OrderId)
        {
            OrderVM orderVM = new()
            {
                OrderHeader = _unitOfWork.orderHeader.Get(u=>u.Id == OrderId, includeProperties: "ApplicationUser"),
                OrderDetail = _unitOfWork.orderDetail.ButunVerileriGetir(u=> u.OrderHeaderId == OrderId, includeProperties: "Product")
            };
            return View(orderVM);
        }
        [HttpPost]
        public IActionResult UpdateOrderDetail()
        {
            var orderHeaderFromDb = _unitOfWork.orderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id);
            orderHeaderFromDb.Name = OrderVM.OrderHeader.Name;
            orderHeaderFromDb.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
            orderHeaderFromDb.StreetAddress = OrderVM.OrderHeader.StreetAddress;
            orderHeaderFromDb.City = OrderVM.OrderHeader.City;
            orderHeaderFromDb.State = OrderVM.OrderHeader.State;
            orderHeaderFromDb.PostalCode = OrderVM.OrderHeader.PostalCode;
            _unitOfWork.orderHeader.Update(orderHeaderFromDb);
            _unitOfWork.save();
            TempData["success"] = "Sipariş güncellendi";
            return RedirectToAction(nameof(Details), new { orderId = orderHeaderFromDb.Id });

        }






        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<OrderHeader> orderHeaders = _unitOfWork.orderHeader
                .ButunVerileriGetir(includeProperties: "ApplicationUser")
                .ToList();

            return Json(new { data = orderHeaders });
        }





        #endregion

    }
        }

    
    
