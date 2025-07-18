using e_commerenceMVC.DataAccess.Data;
using e_commerenceMVC.Models;
using Ecommerence.DataAccess.Repository;
using Ecommerence.DataAccess.Repository.IRepository;
using Ecommerence.Models;
using Ecommerence.Models.ViewModel;
using Microsoft.AspNetCore.Hosting;
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
        public IActionResult Upsert(int? id)
        {
            
            if (id == null || id == 0)
            {
                Category category = new Category();
                return View(category); // Yeni ürün eklemek için Upsert sayfasına yönlendirir.
            }
            else
            {
                Category product = _unitOfWork.category.Get(u => u.CategoryId == id); // Güncelleme için mevcut ürünü alır.
                if (product == null)
                {
                    return NotFound(); // Ürün bulunamazsa NotFound döner.
                }
                return View(product); // Ürünü güncellemek için Upsert sayfasına yönlendirir.
            }
        }
        [HttpPost]
        public IActionResult Upsert(Category obj)
        {
            if (ModelState.IsValid)
            {
                if(obj.CategoryId==0)
                {
                    _unitOfWork.category.Add(obj);
                    TempData["success"] = "Kategori başarıyla eklendi.";
                    _unitOfWork.save();
                    return RedirectToAction("Index");
                }
                else
                {
                    _unitOfWork.category.Guncelle(obj);
                    TempData["success"] = "Kategori başarıyla güncellendi";
                    _unitOfWork.save();
                    return RedirectToAction("Index");
                }
            }
            return View(obj);
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

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Category> categoryList = _unitOfWork.category.ButunVerileriGetir().ToList(); // Tüm ürünleri alır.
            return Json(new { data = categoryList }); // Ürün listesini JSON formatında döner.
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var DeletedId = _unitOfWork.category.Get(u => u.CategoryId == id); // Silinecek ürünü veritabanından alır.
            if (DeletedId == null)
            {
                return Json(new { success = false, message = "Ürün bulunamadı." }); // Ürün bulunamazsa hata mesajı döner.
            }
            _unitOfWork.category.Remove(DeletedId); // Ürünü veritabanından siler.
            _unitOfWork.save(); // Değişiklikler kaydedilir.
            return Json(new { success = true, message = "Ürün başarıyla silindi." }); // Silme başarılı mesajı döner.
        }
        #endregion

    }
}
