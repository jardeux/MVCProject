using System.Diagnostics;
using System.Security.Claims;
using e_commerenceMVC.Models;
using Ecommerence.DataAccess.Repository.IRepository;
using Ecommerence.Models;
using Ecommerence.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace e_commerenceMVC.Areas.Customer.Controllers
{
    [Area("Customer")] // Bu controller'ýn Customer alanýnda olduðunu belirtir.
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.product.ButunVerileriGetir(includeProperties: "Category");
            return View(productList);
        }

        public IActionResult Details(int ProductId)
        {
            ShoppingCart shoppingCart = new ShoppingCart()
            {
                Product = _unitOfWork.product.Get(i => i.ProductId == ProductId, includeProperties: "Category"),
                Count = 1,
                ProductId = ProductId
            };            return View(shoppingCart);

        }
        [HttpPost]
        [Authorize]    
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            // IDENTITY kolonu otomatik artsýn
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;

            ShoppingCart cartFromDb = _unitOfWork.shoppingCart.Get(u => u.ApplicationUserId == userId &&
            u.ProductId == shoppingCart.ProductId);

            if (cartFromDb != null)
            {
                //shopping cart exists
                cartFromDb.Count += shoppingCart.Count;
                _unitOfWork.shoppingCart.Guncelle(cartFromDb);
                _unitOfWork.save();
            }
            else
            {
                //add cart record
                _unitOfWork.shoppingCart.Add(shoppingCart);
                _unitOfWork.save();
                
            }
            TempData["success"] = "Cart updated successfully";




            return RedirectToAction(nameof(Index));
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
