using System.Security.Claims;
using Ecommerence.DataAccess.Repository.IRepository;
using Ecommerence.Models;
using Ecommerence.Models.ViewModel;
using Ecommerence.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace e_commerenceMVC.Areas.Customer.Controllers
{
    [Area("Customer")] // Bu controller'ın Customer alanında olduğunu belirtir.
    [Authorize] // Bu controller'a erişim için kullanıcıların oturum açmış olması gerekir.
    public class ShoppingCartController : Controller
    {
        private IUnitOfWork _unitOfWork;
        [BindProperty] // Bu özelliklerin model bağlama işlemi sırasında otomatik olarak doldurulmasını sağlar.    
        public ShoppingCartVM shoppingCartVM { get; set; } // ShoppingCartViewModel, alışveriş sepeti verilerini tutar.
        public ShoppingCartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork; // IUnitOfWork arayüzü üzerinden veritabanı işlemlerini gerçekleştirmek için kullanılır.
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.shoppingCart.ButunVerileriGetir(u => u.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader = new()
            };
            
            foreach (var cart in shoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedQuantity(cart); // Her alışveriş sepeti öğesi için fiyatı belirler.
                shoppingCartVM.OrderHeader.OrderTotal += (cart.Count * cart.Price); // Toplam sipariş tutarını hesaplar.

            }
            return View(shoppingCartVM);
        }
        private double GetPriceBasedQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Count <= 50)
            {
                return shoppingCart.Product.Price;
            }
            else
            {
                if (shoppingCart.Count <= 100)
                {
                    return shoppingCart.Product.Price50;
                }
                else
                {
                    return shoppingCart.Product.Price100;
                }
            }

        }
        public IActionResult Plus(int id)
        {
            var cartFromDb = _unitOfWork.shoppingCart.Get(u => u.Id == id);
            cartFromDb.Count += 1; // Sepetteki ürün sayısını bir artırır.
            _unitOfWork.shoppingCart.Guncelle(cartFromDb); // Güncellenen alışveriş sepeti öğesini veritabanında günceller.
            _unitOfWork.save(); // Değişiklikleri kaydeder.
            return RedirectToAction(nameof(Index)); // Sepet sayfasına yönlendirir.    
        }
        public IActionResult Minus(int id)
        {
            var cartfromDb = _unitOfWork.shoppingCart.Get(u => u.Id == id);
            if(cartfromDb.Count <= 1) // Eğer sepet sayısı 1 veya daha az ise, bu öğeyi sepetten kaldırır.
            {
                _unitOfWork.shoppingCart.Remove(cartfromDb);
                _unitOfWork.save();
                return RedirectToAction(nameof(Index));
            }
            cartfromDb.Count -= 1;
            
            _unitOfWork.shoppingCart.Guncelle(cartfromDb); // Sepetteki ürün sayısını bir azaltır.
            _unitOfWork.save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            var cartFromDb = _unitOfWork.shoppingCart.Get(u => u.Id == id); // Veritabanından sepet öğesini alır.
            if (cartFromDb != null)
            {
                _unitOfWork.shoppingCart.Remove(cartFromDb); // Sepet öğesini siler.
                _unitOfWork.save(); // Değişiklikleri kaydeder.
                return RedirectToAction(nameof(Index)); // Sepet sayfasına yönlendirir.
            }
            return NotFound(); // Eğer sepet öğesi bulunamazsa, 404 Not Found döner.
        }
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.shoppingCart.ButunVerileriGetir(u => u.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader = new()
            };
            shoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.applicationUser.Get(u => u.Id == userId); // Kullanıcı bilgilerini alır.
            shoppingCartVM.OrderHeader.State = shoppingCartVM.OrderHeader.ApplicationUser.State; // Kullanıcının eyalet bilgisini alır.
            shoppingCartVM.OrderHeader.City = shoppingCartVM.OrderHeader.ApplicationUser.City; // Kullanıcının şehir bilgisini alır.
            shoppingCartVM.OrderHeader.StreetAddress = shoppingCartVM.OrderHeader.ApplicationUser.StreetAddress; // Kullanıcının adres bilgisini alır.
            shoppingCartVM.OrderHeader.PostalCode = shoppingCartVM.OrderHeader.ApplicationUser.PostalCode; // Kullanıcının posta kodunu alır.
            shoppingCartVM.OrderHeader.PhoneNumber = shoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber; // Kullanıcının telefon numarasını alır.
            foreach (var cart in shoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedQuantity(cart);
                shoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }
           
            return View(shoppingCartVM); // Alışveriş sepeti ve sipariş başlığı bilgilerini içeren ShoppingCartVM modelini görüntüler.


        }
        [ActionName("Summary")] // Bu metot, "Summary" adında bir HTTP GET isteği ile çağrılır.
        [HttpPost]
        public IActionResult SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCartVM.ShoppingCartList = _unitOfWork.shoppingCart.ButunVerileriGetir(u => u.ApplicationUserId == userId, includeProperties: "Product"); // Kullanıcının alışveriş sepetindeki tüm ürünleri alır.

            shoppingCartVM.OrderHeader.OrderDate = DateTime.Now; // Sipariş tarihini günceller.
            shoppingCartVM.OrderHeader.ApplicationUserId = userId; // Sipariş başlığına kullanıcı ID'sini ekler.
            
            ApplicationUser applicationUser = _unitOfWork.applicationUser.Get(u => u.Id == userId); // Kullanıcı bilgilerini alır.
            shoppingCartVM.OrderHeader.Name = applicationUser.Name;
            
            if (applicationUser.CompanyId.GetValueOrDefault()==0) // Eğer kullanıcı bir şirkete ait değilse, şirket bilgilerini alır.
            {
                shoppingCartVM.OrderHeader.OrderStatus = SD.PaymentStatusPending; // Sipariş durumu "Pending" olarak ayarlanır.
                shoppingCartVM.OrderHeader.PaymentStatus = SD.StatusPending; // Ödeme durumu "Pending" olarak ayarlanır.
            }
            else
            {
                shoppingCartVM.OrderHeader.OrderStatus = SD.PaymentStatusDelayedPayment; // Eğer kullanıcı bir şirkete ait ise, sipariş durumu "Approved" olarak ayarlanır.
                shoppingCartVM.OrderHeader.PaymentStatus = SD.StatusApproved; // Ödeme durumu "Approved" olarak ayarlanır.
            }
            _unitOfWork.orderHeader.Add(shoppingCartVM.OrderHeader); // Sipariş başlığını veritabanına ekler.
            _unitOfWork.save(); // Değişiklikleri kaydeder.
            foreach(var cart in shoppingCartVM.ShoppingCartList)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cart.ProductId, // Sepetteki ürünün ID'sini alır.
                    OrderHeaderId = shoppingCartVM.OrderHeader.Id,
                    Price = cart.Price,
                    Count = cart.Count // Sepetteki ürünün sayısını alır.
                    

                };
                _unitOfWork.orderDetail.Add(orderDetail); // Sipariş detayını veritabanına ekler.
                _unitOfWork.save();
            }

            if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            {
                
            
            }

            foreach (var cart in shoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedQuantity(cart);
                shoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            return RedirectToAction(nameof(OrderConfirmation), new {id=shoppingCartVM.OrderHeader.Id}); // Alışveriş sepeti ve sipariş başlığı bilgilerini içeren ShoppingCartVM modelini görüntüler.


        }
        public IActionResult OrderConfirmation(int id)
        {
            return View(id);
        }











        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<ShoppingCart> ShoppingCartList = _unitOfWork.shoppingCart.ButunVerileriGetir(includeProperties: "Product").ToList(); // Tüm ürünleri alır.
            return Json(new { data = ShoppingCartList }); // Ürün listesini JSON formatında döner.
        }
        
        #endregion

    }
}
