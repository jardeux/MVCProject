using e_commerenceMVC.DataAccess.Data;
using e_commerenceMVC.Models;
using Ecommerence.DataAccess.Repository;
using Ecommerence.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace e_commerenceMVC.Areas.Admin.Controllers
{
    [Area("Admin")] // Bu controller'ın Admin alanında olduğunu belirtir.
    public class CategoryController : Controller
    {
        //readonly sadece cotr'da atanmak için kullanılır.

        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork; // IUnitOfWork arayüzü üzerinden veritabanı işlemlerini gerçekleştirmek için kullanılır.
        }

        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.category.ButunVerileriGetir().ToList(); // Veritabanındaki tüm kategorileri listelemek için kullanılır.
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
                _unitOfWork.category.Add(obj); // IUnitOfWork arayüzü üzerinden kategori ekleme işlemi yapılır.
                TempData["success"] = "Kategori başarıyla eklendi."; // TempData kullanılarak başarılı ekleme mesajı saklanır.
                _unitOfWork.save(); // Veritabanına kaydedilir.
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
            Category categoryFromDb = _unitOfWork.category.Get(u => u.CategoryId == id); // Veritabanından id'ye göre kategori bulunur.
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
                _unitOfWork.category.Guncelle(obj);
                TempData["success"] = "Kategori başarıyla güncellendi."; // TempData kullanılarak başarılı güncelleme mesajı saklanır.
                _unitOfWork.save();
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
            Category categoryFromDb = _unitOfWork.category.Get(u => u.CategoryId == id); // Veritabanından id'ye göre kategori bulunur.
            if (categoryFromDb == null)
            {
                return NotFound(); // Eğer kategori bulunamazsa, NotFound döndürülür.
            }
            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id) // Bu metod, Create sayfasında form gönderildiğinde tetiklenir.
        {
            Category obj = _unitOfWork.category.Get(u => u.CategoryId == id); // Veritabanından id'ye göre kategori bulunur.
            if(obj==null) // Eğer kategori bulunamazsa, NotFound döndürülür.
            {
                return NotFound();
            }
            _unitOfWork.category.Remove(obj); // Kategori silinir.
            TempData["success"] = "Kategori başarıyla silindi."; // TempData kullanılarak başarılı silme mesajı saklanır.
            _unitOfWork.save(); // Veritabanına kaydedilir.
            return RedirectToAction("Index"); // Silme işlemi başarılı ise Index sayfasına yönlendirme yapılır
        }

    }
}
