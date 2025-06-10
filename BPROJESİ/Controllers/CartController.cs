using BPROJESİ.Data;
using BPROJESİ.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

public class CartController : Controller
{
    private readonly ApplicationDbContext _context;

    public CartController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult AddToCart(int id)
    {
        var product = _context.Products.Find(id);
        if (product == null)
        {
            return Json(new { success = false, message = "Ürün bulunamadı." });
        }

        // Sepet kaleminde ürünü ara
        var cartItem = _context.CartItems.FirstOrDefault(x => x.ProductId == id);
        if (cartItem != null)
        {
            cartItem.Quantity++;
            _context.CartItems.Update(cartItem);
        }
        else
        {
            cartItem = new CartItem
            {
                ProductId = id,
                Quantity = 1
            };
            _context.CartItems.Add(cartItem);
        }

        _context.SaveChanges();
        return Json(new { success = true, message = "Sepete eklendi." });
    }

    public IActionResult Index()
    {
        // Sepet kalemlerini ürünleriyle birlikte çekiyoruz
        var cartItems = _context.CartItems
                                .Include(c => c.Product)
                                .ToList();

        // ViewModel oluştur
        var model = new CartViewModel
        {
            Items = cartItems.Select(ci => new CartItem
            {
                ProductId = ci.ProductId,
                ProductName = ci.Product.Name,
                ProductPrice = ci.Product.Price,
                Quantity = ci.Quantity,
                ImageUrl = ci.Product.ImageUrl
            }).ToList()
        };

        return View(model);
    }
}
