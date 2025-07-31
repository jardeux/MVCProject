using System.Collections.Generic;
using e_commerenceMVC.DataAccess.Data;
using e_commerenceMVC.Models;
using Ecommerence.DataAccess.Repository;
using Ecommerence.DataAccess.Repository.IRepository;
using Ecommerence.Models;
using Ecommerence.Models.ViewModel;
using Ecommerence.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace e_commerenceMVC.Areas.Admin.Controllers
{
    [Area("Admin")] // Bu controller'ın Admin alanında olduğunu belirtir.
    [Authorize(Roles = SD.Role_User_Admin)] // Bu controller'a erişim için Admin rolüne sahip olma şartı aranır.


    public class CompanyController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CompanyController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }



        public IActionResult Index()
        {
            List<Company> company = _unitOfWork.company.ButunVerileriGetir().ToList();


            return View(company);
        }
        public IActionResult Upsert(int? id)
        {
            if (id == null || id == 0)
            {
                Company company = new Company();
                return View(company); // Yeni ürün eklemek için Upsert sayfasına yönlendirir.
            }
            else
            {
                Company company = _unitOfWork.company.Get(u => u.CompanyId == id); // Güncelleme için mevcut ürünü alır.
                if (company == null)
                {
                    return NotFound(); // Ürün bulunamazsa NotFound döner.
                }
                return View(company); // Ürünü güncellemek için Upsert sayfasına yönlendirir.
            }
        }
        [HttpPost]
        public IActionResult Upsert(Company obj)
        {
            if (ModelState.IsValid)
            {

                if (obj.CompanyId == 0) // Eğer yeni bir ürün ekleniyorsa
                {
                    _unitOfWork.company.Add(obj); // Ürünü veritabanına ekler.
                    _unitOfWork.save();
                    TempData["success"] = "Ürün başarıyla eklendi."; // Başarılı mesajı saklanır.
                    return RedirectToAction("Index");
                }
                else // Eğer mevcut bir ürün güncelleniyorsa
                {
                    _unitOfWork.company.CompanyGuncelle(obj); // Ürünü günceller.
                    _unitOfWork.save();
                    TempData["success"] = "Ürün başarıyla güncellendi."; // Güncelleme başarılı mesajı saklanır.
                    return RedirectToAction("Index");
                }


            }
            return View(obj);
        }
            #region API CALLS
            [HttpGet]
            public IActionResult GetAll()
            {
                List<Company> companyList = _unitOfWork.company.ButunVerileriGetir().ToList(); // Tüm ürünleri alır.
                return Json(new { data = companyList }); // Ürün listesini JSON formatında döner.
            }

            [HttpDelete]
            public IActionResult Delete(int? id)
            {
                var DeletedId = _unitOfWork.company.Get(u => u.CompanyId == id); // Silinecek ürünü veritabanından alır.
                if (DeletedId == null)
                {
                    return Json(new { success = false, message = "Ürün bulunamadı." }); // Ürün bulunamazsa hata mesajı döner.
                }
                _unitOfWork.company.Remove(DeletedId); // Ürünü veritabanından siler.
                _unitOfWork.save(); // Değişiklikler kaydedilir.
                return Json(new { success = true, message = "Ürün başarıyla silindi." }); // Silme başarılı mesajı döner.
            }
            #endregion

        }
    }
