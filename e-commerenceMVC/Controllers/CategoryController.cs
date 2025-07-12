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
            //3 adet validation var bunlardan 1i client side javascript ile eklediğimiz sayfa yenilenmeden doğrulama yapar
            //diğeri asp validation bu controllerdan model error ekleriz viewda <div asp-validation - summary = "All" ></ div > ekleriz
            //custom validation modelden ekleriz 

            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "İsim ile display order aynı olamaz."); // Eğer Name ve DisplayOrder aynı ise, ModelState'e özel bir hata eklenir.
            }
            if (obj.Name.ToLower() == "test")
            {
                ModelState.AddModelError("name", "Test ismi kullanılamaz."); // Eğer Name "test" ise, ModelState'e özel bir hata eklenir.
            }
            if (ModelState.IsValid)
            {
                //ModelState.IsValid kontrolü yapıldıktan sonra veritabanına ekleme işlemi yapılır.
                _db.Categories.Add(obj);
                TempData["success"] = "Kategori başarıyla eklendi."; // TempData kullanılarak başarılı ekleme mesajı saklanır.
                _db.SaveChanges();
                return RedirectToAction("Index"); //Eğer veritabanına ekleme işlemi başarılı ise Index sayfasına yönlendirme yapılır.
            }
            return View(); //Eğer ModelState.IsValid kontrolü başarısız ise, tekrar Create sayfasına yönlendirme yapılır.

        }
        public IActionResult Edit(int? id) // Bu metod, Edit sayfasına yönlendirme yapmak için kullanılır. id parametresi nullable int olarak tanımlanmıştır.
        {
            if (id == null || id == 0)
            {
                return NotFound(); // Eğer id null veya 0 ise, NotFound döndürülür.
            }
            Category categoryFromDb = _db.Categories.Find(id); // Veritabanından id'ye göre kategori bulunur.
            if (categoryFromDb == null)
            {
                return NotFound(); // Eğer kategori bulunamazsa, NotFound döndürülür.
            }
            return View(categoryFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj) // Bu metod, Create sayfasında form gönderildiğinde tetiklenir.
        {
            //3 adet validation var bunlardan 1i client side javascript ile eklediğimiz sayfa yenilenmeden doğrulama yapar
            //diğeri asp validation bu controllerdan model error ekleriz viewda <div asp-validation - summary = "All" ></ div > ekleriz
            //custom validation modelden ekleriz 

            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "İsim ile display order aynı olamaz."); // Eğer Name ve DisplayOrder aynı ise, ModelState'e özel bir hata eklenir.
            }
            if (obj.Name.ToLower() == "test")
            {
                ModelState.AddModelError("name", "Test ismi kullanılamaz."); // Eğer Name "test" ise, ModelState'e özel bir hata eklenir.
            }
            if (ModelState.IsValid)
            {
                //ModelState.IsValid kontrolü yapıldıktan sonra veritabanına ekleme işlemi yapılır.
                _db.Categories.Update(obj);
                TempData["success"] = "Kategori başarıyla güncellendi."; // TempData kullanılarak başarılı güncelleme mesajı saklanır.
                _db.SaveChanges();
                return RedirectToAction("Index"); //Eğer veritabanına ekleme işlemi başarılı ise Index sayfasına yönlendirme yapılır.
            }
            return View(); //Eğer ModelState.IsValid kontrolü başarısız ise, tekrar Create sayfasına yönlendirme yapılır.

        }
        public IActionResult Delete(int? id) // Bu metod, Edit sayfasına yönlendirme yapmak için kullanılır. id parametresi nullable int olarak tanımlanmıştır.
        {
            if (id == null || id == 0)
            {
                return NotFound(); // Eğer id null veya 0 ise, NotFound döndürülür.
            }
            Category categoryFromDb = _db.Categories.Find(id); // Veritabanından id'ye göre kategori bulunur.
            if (categoryFromDb == null)
            {
                return NotFound(); // Eğer kategori bulunamazsa, NotFound döndürülür.
            }
            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id) // Bu metod, Create sayfasında form gönderildiğinde tetiklenir.
        {
            Category obj = _db.Categories.Find(id); // Veritabanından id'ye göre kategori bulunur.
            if(obj==null) // Eğer kategori bulunamazsa, NotFound döndürülür.
            {
                return NotFound();
            }
            _db.Categories.Remove(obj); // Kategori silinir.
            TempData["success"] = "Kategori başarıyla silindi."; // TempData kullanılarak başarılı silme mesajı saklanır.
            _db.SaveChanges(); // Veritabanına kaydedilir.
            return RedirectToAction("Index"); // Silme işlemi başarılı ise Index sayfasına yönlendirme yapılır
        }

    }
}
