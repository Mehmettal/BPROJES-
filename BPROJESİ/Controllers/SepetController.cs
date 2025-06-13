using BPROJESİ.Data;
using BPROJESİ.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

public class SepetController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public SepetController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    [HttpPost]
    public async Task<IActionResult> SepeteEkle([FromBody] CartAddRequest model)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        if (model == null || model.ProductId == 0)
            return BadRequest(new { success = false, message = "Geçersiz ürün!" });

        var mevcut = _context.CartItems
            .FirstOrDefault(c => c.UserName == user.UserName && c.ProductId == model.ProductId);

        if (mevcut != null)
        {
            mevcut.Quantity += model.Quantity;
        }
        else
        {
            _context.CartItems.Add(new CartItem
            {
                UserName = user.UserName,
                ProductId = model.ProductId,
                ProductName = model.Name,
                ProductPrice = model.Price,
                Quantity = model.Quantity,
                ImageUrl = model.ImageUrl
            });
        }

        await _context.SaveChangesAsync();
        return Json(new { success = true });
    }



    [HttpGet]
    public IActionResult Sepetim()
    {
        return View();
    }

    // Sepetteki ürünleri JSON döner
    [HttpGet]
    public async Task<IActionResult> GetCartItems()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var sepet = _context.CartItems
            .Where(c => c.UserName == user.UserName)
            .Select(c => new
            {
                id = c.Id,
                productName = c.ProductName,
                quantity = c.Quantity,
                productPrice = c.ProductPrice,
                imageUrl = c.ImageUrl
            })
            .ToList();

        return Json(sepet);
    }

    // Adet güncelle
    [HttpPost]
    public async Task<IActionResult> GuncelleAdet([FromBody] CartQuantityUpdateModel model)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var item = _context.CartItems.FirstOrDefault(c => c.Id == model.Id && c.UserName == user.UserName);
        if (item == null) return NotFound();

        item.Quantity += model.Degisim;
        if (item.Quantity < 1)
        {
            _context.CartItems.Remove(item);
        }
        await _context.SaveChangesAsync();

        return Ok();
    }

    // Ürün sil
    [HttpPost]
    public async Task<IActionResult> Sil([FromBody] CartDeleteModel model)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var item = _context.CartItems.FirstOrDefault(c => c.Id == model.Id && c.UserName == user.UserName);
        if (item == null) return NotFound();

        _context.CartItems.Remove(item);
        await _context.SaveChangesAsync();

        return Ok();
    }

    // (İsteğe bağlı) Sepeti onayla, ödeme ekranına yönlendir
    [HttpPost]
    public IActionResult SepetiOnayla()
    {
        // Burada ödeme ekranına yönlendirebilirsin.
        return Json(new { success = true, redirectUrl = Url.Action("Odeme", "Odeme") });
    }
}

// Modeller
public class CartQuantityUpdateModel
{
    public int Id { get; set; }
    public int Degisim { get; set; }
}
public class CartDeleteModel
{
    public int Id { get; set; }
}
