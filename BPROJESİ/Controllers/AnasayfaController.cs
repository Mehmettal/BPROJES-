using Microsoft.AspNetCore.Mvc;
using BPROJESİ.Models;
using System.Linq;
using BPROJESİ.Data;

namespace BPROJESİ.Controllers
{
    public class AnasayfaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnasayfaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Ana sayfa view
        public IActionResult Anasayfa()
        {
            var petAds = _context.PetAds.ToList();
            return View(petAds);
        }

        // Ana sayfada JS ile çekilen ilanları döner
        [HttpGet]
        public IActionResult GetCats()
        {
            var pets = _context.PetAds
                .ToList()
                .Select(p => new
                {
                    id = p.Id,
                    name = p.AdTitle ?? "",
                    price = p.Price ?? 0,
                    imageUrl = !string.IsNullOrEmpty(p.PhotoPaths)
                        ? p.PhotoPaths.Split(',')[0]
                        : "/images/default.jpg",
                    description = p.Description ?? "",
                    userId = p.UserId ?? ""   // <-- EKLEDİK!
                }).ToList();

            return Json(pets);
        }
    }
}
