using System.Collections.Generic;
using e_commerenceMVC.DataAccess.Data;
using e_commerenceMVC.Models;
using Ecommerence.DataAccess.Repository;
using Ecommerence.DataAccess.Repository.IRepository;
using Ecommerence.Models;
using Ecommerence.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.category.ButunVerileriGetir().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.CategoryId.ToString()
                }),
                Product = new Product() // Yeni bir Product nesnesi oluşturur.  

            };
            
            if (id == null || id == 0)
            {
                return View (productVM); // Yeni ürün eklemek için Upsert sayfasına yönlendirir.
            }
            else
            {
                productVM.Product = _unitOfWork.product.Get(u => u.ProductId == id); // Güncelleme için mevcut ürünü alır.
                if (productVM.Product == null)
                {
                    return NotFound(); // Ürün bulunamazsa NotFound döner.
                }
                return View(productVM); // Ürünü güncellemek için Upsert sayfasına yönlendirir.
            }
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.product.Add(obj.Product);
                TempData["success"] = "Ürün başarıyla eklendi.";
                _unitOfWork.save();
                return RedirectToAction("Index");
            }
            return View(obj);
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
