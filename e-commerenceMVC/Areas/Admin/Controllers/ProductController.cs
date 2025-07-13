using e_commerenceMVC.DataAccess.Data;
using Ecommerence.DataAccess.Repository;
using Ecommerence.DataAccess.Repository.IRepository;
using Ecommerence.Models;
using Microsoft.AspNetCore.Mvc;

namespace e_commerenceMVC.Areas.Admin.Controllers
{
    [Area("Admin")] // Bu controller'ın Admin alanında olduğunu belirtir.

    public class ProductController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        public IActionResult Index()
        {
            List<Product> products = _unitOfWork.product.ButunVerileriGetir().ToList();
            return View(products);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.product.Add(obj);
                TempData["success"] = "Ürün başarıyla eklendi.";
                _unitOfWork.save();
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        public IActionResult Edit(int? id)
        {
            var ProductId = _unitOfWork.product.Get(u => u.ProductId == id);


            return View(ProductId);
        }
        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            _unitOfWork.product.ProductGuncelle(obj); // Ürünü güncellemek için ProductGuncelle metodunu çağırır.
            TempData["success"] = "Ürün başarıyla güncellendi."; // Güncelleme başarılı mesajı saklanır.
            _unitOfWork.save(); // Değişiklikler kaydedilir.
            return RedirectToAction("Index"); // Güncelleme sonrası Index sayfasına yönlendirilir.
        }
        public IActionResult Delete(int id)
        {
            var DeletedId = _unitOfWork.product.Get(u => u.ProductId == id); // Silinecek ürünü veritabanından alır.
            return View(DeletedId);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int id)
        {
            var DeletedId = _unitOfWork.product.Get(u => u.ProductId == id); // Silinecek ürünü veritabanından alır.
            _unitOfWork.product.Remove(DeletedId); // Ürünü veritabanından siler.
            TempData["success"] = "Ürün başarıyla silindi."; // Silme başarılı mesajı saklanır.
            _unitOfWork.save(); // Değişiklikler kaydedilir.
            return RedirectToAction("Index"); // Silme sonrası Index sayfasına yönlendirilir.
        }
    
    
    
    }
}
