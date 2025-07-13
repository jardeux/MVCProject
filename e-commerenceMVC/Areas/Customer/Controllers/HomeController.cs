using System.Diagnostics;
using e_commerenceMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace e_commerenceMVC.Areas.Customer.Controllers
{
    [Area("Customer")] // Bu controller'ýn Customer alanýnda olduðunu belirtir.
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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
