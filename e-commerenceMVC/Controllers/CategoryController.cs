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
            List<Models.Category> objCategoryList = _db.Categories.ToList(); // Veritabanındaki tüm kategorileri listelemek için kullanılır.
            return View(objCategoryList); // objCategoryList listesini View'a gönderir.
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj) // Bu metod, Create sayfasında form gönderildiğinde tetiklenir.
        {
            if(obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "İsim ile display order aynı olamaz."); // Eğer Name ve DisplayOrder aynı ise, ModelState'e özel bir hata eklenir.
            }
            
            if(ModelState.IsValid) 
            {
                //ModelState.IsValid kontrolü yapıldıktan sonra veritabanına ekleme işlemi yapılır.
                _db.Categories.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index"); //Eğer veritabanına ekleme işlemi başarılı ise Index sayfasına yönlendirme yapılır.
            }
                return View(); //Eğer ModelState.IsValid kontrolü başarısız ise, tekrar Create sayfasına yönlendirme yapılır.

        }
    }
}
