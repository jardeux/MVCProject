using e_commerenceMVC.Data;
using e_commerenceMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace e_commerenceMVC.Controllers
{
    public class CategoryController : Controller
    {
        //readonly sadece cotr'da atanmak için kullanılır.

        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Models.Category> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            _db.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
