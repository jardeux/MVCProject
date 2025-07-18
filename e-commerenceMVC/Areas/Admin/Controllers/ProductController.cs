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
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }



        public IActionResult Index()
        {
            List<Product> products = _unitOfWork.product.ButunVerileriGetir(includeProperties: "Category").ToList();       
            

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
                string wwwRootPath = _webHostEnvironment.WebRootPath; // Web uygulamasının kök dizinini alır.
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName); // Benzersiz bir dosya adı oluşturur.
                    var uploads = Path.Combine(wwwRootPath, @"images\products"); // Ürün resimlerinin yükleneceği klasör.
                    if(!string.IsNullOrEmpty(obj.Product.ImageUrl)) // Eğer mevcut bir resim URL'si varsa, eski resmi siler.
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath); // Eski resmi siler.
                        }
                    }


                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream); // Dosyayı belirtilen konuma kopyalar.
                    }
                obj.Product.ImageUrl = @"\images\products\" + fileName; // Ürün resminin URL'sini ayarlar.
                }
                if(obj.Product.ProductId == 0) // Eğer yeni bir ürün ekleniyorsa
                {
                    _unitOfWork.product.Add(obj.Product); // Ürünü veritabanına ekler.
                    TempData["success"] = "Ürün başarıyla eklendi."; // Başarılı mesajı saklanır.
                }
                else // Eğer mevcut bir ürün güncelleniyorsa
                {
                    _unitOfWork.product.ProductGuncelle(obj.Product); // Ürünü günceller.
                    TempData["success"] = "Ürün başarıyla güncellendi."; // Güncelleme başarılı mesajı saklanır.
                }
            } 
            if (ModelState.IsValid)
            {

                TempData["success"] = "Ürün başarıyla eklendi.";
                _unitOfWork.save();
                return RedirectToAction("Index");
            }
            return View(obj);
        }   
        

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> productList = _unitOfWork.product.ButunVerileriGetir().ToList(); // Tüm ürünleri alır.
            return Json(new { data = productList }); // Ürün listesini JSON formatında döner.
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var DeletedId = _unitOfWork.product.Get(u => u.ProductId == id); // Silinecek ürünü veritabanından alır.
            if (DeletedId == null)
            {
                return Json(new { success = false, message = "Ürün bulunamadı." }); // Ürün bulunamazsa hata mesajı döner.
            }
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, DeletedId.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath); // Eski resmi siler.
            }

            _unitOfWork.product.Remove(DeletedId); // Ürünü veritabanından siler.
            _unitOfWork.save(); // Değişiklikler kaydedilir.
            return Json(new { success = true, message = "Ürün başarıyla silindi." }); // Silme başarılı mesajı döner.
        }



        #endregion

    }
}
