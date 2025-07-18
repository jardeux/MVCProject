using System.Diagnostics;
using e_commerenceMVC.Models;
using Ecommerence.DataAccess.Repository.IRepository;
using Ecommerence.Models;
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

        public IActionResult Details(int id)
        {
            Product product = _unitOfWork.product.Get(i => i.ProductId == id, includeProperties: "Category");
            return View(product);

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
