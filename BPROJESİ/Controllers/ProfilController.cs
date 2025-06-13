using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BPROJESİ.Data;
using System.Linq;
using System.Threading.Tasks;
using BPROJESİ.Models;

namespace BPROJESİ.Controllers
{
    [Authorize]
    public class ProfilController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfilController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Profile()
        {
            return View();
        }

        // Kullanıcı Bilgileri
        [HttpGet]
        public async Task<IActionResult> GetUserInfo()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            return Json(new
            {
                userName = user.UserName,
                email = user.Email,
                phoneNumber = user.PhoneNumber
            });
        }

        // İlanlarım
        [HttpGet]
        public async Task<IActionResult> GetMyAds()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id;
            if (userId == null)
                return Unauthorized();

            var adsList = _context.PetAds
                .Where(x => x.UserId == userId)
                .ToList();

            var ads = adsList.Select(x => new
            {
                name = x.AdTitle,
                description = x.Description,
                price = x.Price,
                imageUrl = !string.IsNullOrEmpty(x.PhotoPaths) && x.PhotoPaths.Contains(",")
                    ? x.PhotoPaths.Split(',')[0]
                    : x.PhotoPaths
            }).ToList();

            return Json(ads);
        }

        // Sepettekiler (eski sistem, istersen silebilirsin)
        [HttpGet]
        public async Task<IActionResult> GetMyCart()
        {
            var user = await _userManager.GetUserAsync(User);
            var userName = user?.UserName;
            if (userName == null)
                return Unauthorized();

            var cartItems = _context.CartItems
                .Where(x => x.UserName == userName)
                .Select(x => new
                {
                    productName = x.ProductName,
                    productPrice = x.ProductPrice,
                    quantity = x.Quantity,
                    imageUrl = x.ImageUrl
                })
                .ToList();

            return Json(cartItems);
        }

        // 🟢 SİPARİŞLERİM (Sipariş geçmişi)
        [HttpGet]
        public async Task<IActionResult> GetMyOrders()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var orders = _context.Orders
                .Where(o => o.UserId == user.Id)
                .OrderByDescending(o => o.Tarih)
                .Select(o => new
                {
                    tarih = o.Tarih,
                    adSoyad = o.AdSoyad,
                    adres = o.Adres,
                    odemeTuru = o.OdemeTuru,
                    items = o.OrderItems.Select(oi => new {
                        productName = oi.ProductName,
                        quantity = oi.Quantity,
                        productPrice = oi.ProductPrice,
                        imageUrl = oi.ImageUrl
                    }).ToList()
                })
                .ToList();

            return Json(orders);
        }
        [HttpPost]
public async Task<IActionResult> Guncelle([FromBody] ProfilUpdateModel model)
{
    var user = await _userManager.GetUserAsync(User);
    if (user == null) return Unauthorized();

    if (!string.IsNullOrEmpty(model.Email))
        user.Email = model.Email;
    if (!string.IsNullOrEmpty(model.PhoneNumber))
        user.PhoneNumber = model.PhoneNumber;
    // Kullanıcı adını değiştirmek istemiyorsan sadece readonly bırak

    var result = await _userManager.UpdateAsync(user);
    if (result.Succeeded)
        return Json(new { success = true });

    return Json(new { success = false, message = string.Join(",", result.Errors.Select(x => x.Description)) });
}

// Model
public class ProfilUpdateModel
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}

    }
}
