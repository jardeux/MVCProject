using e_commerenceMVC.Data;
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
        public IActionResult buyall()
        {
            return View();
        }
    }
}
