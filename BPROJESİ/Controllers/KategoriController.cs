using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BPROJESİ.Data;
using BPROJESİ.Models;
using System.Linq;
using System.Threading.Tasks;

namespace BPROJESİ.Controllers
{
    public class KategoriController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KategoriController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Kategoriler()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetFilteredAds(string kategori, string yas, string cinsiyet, string fiyat)
        {
            var petAds = _context.PetAds.AsQueryable();

            if (!string.IsNullOrEmpty(kategori) && kategori != "Tümü")
                petAds = petAds.Where(x => x.PetType == kategori);

            if (!string.IsNullOrEmpty(yas) && yas != "Tümü")
            {
                if (yas == "Yavru") petAds = petAds.Where(x => x.Age < 12);
                if (yas == "Genç") petAds = petAds.Where(x => x.Age >= 12 && x.Age < 36);
                if (yas == "Yetişkin") petAds = petAds.Where(x => x.Age >= 36);
            }

            if (!string.IsNullOrEmpty(cinsiyet) && cinsiyet != "Tümü")
                petAds = petAds.Where(x => x.Gender == cinsiyet);

            if (!string.IsNullOrEmpty(fiyat) && fiyat != "Tümü")
            {
                if (fiyat == "0-500 TL") petAds = petAds.Where(x => x.Price <= 500);
                if (fiyat == "500-2000 TL") petAds = petAds.Where(x => x.Price > 500 && x.Price <= 2000);
                if (fiyat == "2000+ TL") petAds = petAds.Where(x => x.Price > 2000);
            }

            var ads = await petAds
                .OrderByDescending(x => x.Id)
                .Select(ad => new {
                    ad.Id,
                    ad.AdTitle,
                    ad.PetType,
                    ad.Breed,
                    ad.Gender,
                    ad.Age,
                    ad.Price,
                    ad.PhotoPaths,
                    ad.City
                }).ToListAsync();

            return Json(new { success = true, ads });
        }
    }
}
