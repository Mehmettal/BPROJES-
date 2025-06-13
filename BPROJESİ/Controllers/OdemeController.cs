using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using BPROJESİ.Data;
using BPROJESİ.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace BPROJESİ.Controllers
{
    public class OdemeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OdemeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Odeme()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Onayla()
        {
            var adSoyad = Request.Form["adSoyad"].ToString();
            var adres = Request.Form["adres"].ToString();
            var odemeTuru = Request.Form["odemeTuru"].ToString();

            if (string.IsNullOrWhiteSpace(adSoyad) || string.IsNullOrWhiteSpace(adres))
            {
                return Json(new { success = false, message = "Ad-soyad ve adres alanları zorunludur!" });
            }

            // Kredi Kartı kontrolü
            if (odemeTuru == "Kredi Kartı")
            {
                var kartNo = Request.Form["kartNo"].ToString();
                var sonKullanma = Request.Form["sonKullanma"].ToString();
                var cvv = Request.Form["cvv"].ToString();

                if (string.IsNullOrWhiteSpace(kartNo) || kartNo.Length < 16)
                    return Json(new { success = false, message = "Kart numarası geçersiz!" });

                if (string.IsNullOrWhiteSpace(sonKullanma))
                    return Json(new { success = false, message = "Son kullanma tarihi giriniz!" });

                if (string.IsNullOrWhiteSpace(cvv) || cvv.Length < 3)
                    return Json(new { success = false, message = "CVV kodu hatalı!" });
            }

            // 🟢 SİPARİŞ KAYDI VE SEPETİ BOŞALT!
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Json(new { success = false, message = "Kullanıcı oturumu geçersiz!" });

            // Sepetteki ürünleri al
            var cartItems = _context.CartItems.Where(c => c.UserName == user.UserName).ToList();
            if (cartItems.Count == 0)
                return Json(new { success = false, message = "Sepetiniz boş!" });

            // Sipariş ve ürünlerini hazırla
            var order = new Order
            {
                UserId = user.Id,
                AdSoyad = adSoyad,
                Adres = adres,
                OdemeTuru = odemeTuru,
                Tarih = DateTime.Now,
                OrderItems = new List<OrderItem>()
            };

            foreach (var item in cartItems)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductName = item.ProductName,
                    ProductPrice = item.ProductPrice,
                    Quantity = item.Quantity,
                    ImageUrl = item.ImageUrl
                });
            }

            // Siparişi veritabanına kaydet
            _context.Orders.Add(order);

            // Sepeti boşalt
            _context.CartItems.RemoveRange(cartItems);

            await _context.SaveChangesAsync();

            // Başarılı dönüş
            return Json(new { success = true, message = "Siparişiniz başarıyla alındı." });
        }
    }
}
