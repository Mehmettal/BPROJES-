namespace BPROJESİ.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using BPROJESİ.Models;  // Product modeli burada tanımlı
    using System.Linq;
    using System;
    using BPROJESİ.Data;

    public class AnasayfaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnasayfaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Anasayfa()
        {
            var products = _context.Products.ToList(); // Veritabanından ürünleri al
            return View(products); // View'e gönder
        }
        [HttpGet]
        public IActionResult GetCats()
        {
            var pets = _context.Products.Select(p => new
            {
                id = p.Id,
                name = p.Name,
                price = p.Price,
                imageUrl = p.ImageUrl,
                description = p.Description
            }).ToList();

            return Json(pets);
        }
    }
}