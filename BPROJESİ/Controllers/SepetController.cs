using Microsoft.AspNetCore.Mvc;
using BPROJESİ.Models;
using System.Linq;
using System;
using BPROJESİ.Data;

public class SepetController : Controller
{
    private readonly ApplicationDbContext _context;

    public SepetController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult Ekle(int productId)
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == productId);
        if (product == null)
        {
            return NotFound();
        }

        var existingItem = _context.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

        if (existingItem != null)
        {
            existingItem.Quantity++;
        }
        else
        {
            var newItem = new CartItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ProductPrice = product.Price,
                Quantity = 1,
                ImageUrl = product.ImageUrl
            };
            _context.CartItems.Add(newItem);
        }

        _context.SaveChanges();
        return RedirectToAction("Sepetim");
    }

    public IActionResult Sepetim()
    {
        var cartItems = _context.CartItems.ToList();
        return View(cartItems);
    }
}
