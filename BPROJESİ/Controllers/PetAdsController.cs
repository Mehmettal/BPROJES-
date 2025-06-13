using BPROJESİ.Data;
using BPROJESİ.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace BPROJESİ.Controllers
{
    [Authorize]
    public class PetAdsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PetAdsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Tüm ilanları JSON olarak döner (AJAX için) -- FOR ile
        [HttpGet]
        public async Task<IActionResult> GetPetAds()
        {
            var petAdsList = await _context.PetAds.ToListAsync();
            var petAds = new List<object>();
            int count = petAdsList.Count;
            for (int i = 0; i < count; i++)
            {
                var p = petAdsList[i];
                petAds.Add(new
                {
                    p.Id,
                    p.AdTitle,
                    p.Description,
                    p.PetType,
                    p.Breed,
                    p.Gender,
                    p.Age,
                    p.Price,
                    p.AdType,
                    p.City,
                    p.District,
                    p.ContactPhone,
                    p.PhotoPaths
                });
            }
            return Json(petAds);
        }

        // İlan Detayı
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAdDetails(int id)
        {
            var ad = await _context.PetAds
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (ad == null)
                return Json(new { success = false, message = "İlan bulunamadı" });

            return Json(new
            {
                success = true,
                data = new
                {
                    ad.Id,
                    ad.AdTitle,
                    ad.PetType,
                    ad.Breed,
                    ad.Gender,
                    ad.Age,
                    ad.Size,
                    ad.Vaccinated,
                    ad.Sterilized,
                    ad.Microchipped,
                    ad.City,
                    ad.District,
                    ad.AdType,
                    ad.Price,
                    ad.Description,
                    ad.ContactPhone,
                    ad.PhotoPaths,
                    ad.UserId
                }
            });
        }

        // Kendi ilanlarım
        [HttpGet]
        public async Task<IActionResult> MyAds()
        {
            var userId = _userManager.GetUserId(User);
            var myAdsList = await _context.PetAds.Where(x => x.UserId == userId).ToListAsync();
            return View(myAdsList);
        }

        // Yeni ilan ekleme (AJAX POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PetAd model, List<IFormFile> Photos)
        {
            ModelState.Remove("UserId");

            if (!ModelState.IsValid)
            {
                var allErrors = new List<string>();
                var modelStates = ModelState.Values.ToList();
                int modelStateCount = modelStates.Count;
                for (int i = 0; i < modelStateCount; i++)
                {
                    var errors = modelStates[i].Errors;
                    int errorCount = errors.Count;
                    for (int j = 0; j < errorCount; j++)
                    {
                        allErrors.Add(errors[j].ErrorMessage);
                    }
                }
                return Json(new { success = false, errors = allErrors });
            }

            var userId = _userManager.GetUserId(User);

            // Fotoğraf yükleme
            var photoPaths = new List<string>();
            if (Photos != null && Photos.Count > 0)
            {
                int photoCount = Photos.Count > 5 ? 5 : Photos.Count;
                for (int i = 0; i < photoCount; i++)
                {
                    var photo = Photos[i];
                    if (photo.Length > 0)
                    {
                        var fileName = System.Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
                        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                        var uploadDir = Path.GetDirectoryName(uploadPath);
                        if (!Directory.Exists(uploadDir))
                            Directory.CreateDirectory(uploadDir);

                        using (var stream = new FileStream(uploadPath, FileMode.Create))
                        {
                            photo.CopyTo(stream);
                        }
                        photoPaths.Add("/uploads/" + fileName);
                    }
                }
            }

            var newAd = new PetAd
            {
                AdTitle = model.AdTitle,
                Description = model.Description,
                Price = model.Price,
                PetType = model.PetType,
                Breed = model.Breed,
                Gender = model.Gender,
                Age = model.Age,
                Size = model.Size,
                Vaccinated = model.Vaccinated,
                Sterilized = model.Sterilized,
                Microchipped = model.Microchipped,
                City = model.City,
                District = model.District,
                AdType = model.AdType,
                ContactPhone = model.ContactPhone,
                UserId = userId,
                PhotoPaths = string.Join(",", photoPaths)
            };

            _context.PetAds.Add(newAd);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }
        [HttpGet]
        public async Task<IActionResult> GetAdEdit(int id)
        {
            var ad = await _context.PetAds.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (ad == null)
                return Json(new { success = false, message = "İlan bulunamadı" });

            return Json(new
            {
                success = true,
                data = new
                {
                    ad.Id,
                    ad.AdTitle,
                    ad.PetType,
                    ad.Breed,
                    ad.Gender,
                    ad.Age,
                    ad.Size,
                    ad.Vaccinated,
                    ad.Sterilized,
                    ad.Microchipped,
                    ad.City,
                    ad.District,
                    ad.AdType,
                    ad.Price,
                    ad.Description,
                    ad.ContactPhone,
                    ad.PhotoPaths,
                    ad.UserId
                }
            });
        }
        [HttpPost]
        public async Task<IActionResult> EditAd([FromBody] PetAd model)
        {
            try
            {
                var ad = await _context.PetAds.FirstOrDefaultAsync(p => p.Id == model.Id);
                if (ad == null)
                    return Json(new { success = false, message = "İlan bulunamadı" });

                ad.AdTitle = model.AdTitle;
                ad.PetType = model.PetType;
                ad.Breed = model.Breed;
                ad.Gender = model.Gender;
                ad.Age = model.Age;
                ad.Size = model.Size;
                ad.Vaccinated = model.Vaccinated;
                ad.Sterilized = model.Sterilized;
                ad.Microchipped = model.Microchipped;
                ad.City = model.City;
                ad.District = model.District;
                ad.AdType = model.AdType;
                ad.Price = model.Price;
                ad.Description = model.Description;
                ad.ContactPhone = model.ContactPhone;

                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Hata mesajını detaylı döndür
                return Json(new { success = false, message = "HATA: " + ex.Message });
            }
        }


        // İlan düzenleme (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var ad = await _context.PetAds.FindAsync(id);
            var userId = _userManager.GetUserId(User);

            if (ad == null || ad.UserId != userId)
                return Unauthorized();

            return View(ad);
        }

        // İlan düzenleme (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PetAd model)
        {
            var userId = _userManager.GetUserId(User);
            var ad = await _context.PetAds.FindAsync(model.Id);

            if (ad == null || ad.UserId != userId)
                return Unauthorized();

            ad.AdTitle = model.AdTitle;
            ad.Description = model.Description;
            ad.Price = model.Price;
            ad.PetType = model.PetType;
            ad.Breed = model.Breed;
            ad.Gender = model.Gender;
            ad.Age = model.Age;
            ad.Size = model.Size;
            ad.Vaccinated = model.Vaccinated;
            ad.Sterilized = model.Sterilized;
            ad.Microchipped = model.Microchipped;
            ad.City = model.City;
            ad.District = model.District;
            ad.AdType = model.AdType;
            ad.ContactPhone = model.ContactPhone;

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = ad.Id });
        }

        // İlan silme
        [HttpPost]
        public async Task<IActionResult> DeleteAd(int id)
        {
            var ad = await _context.PetAds.FindAsync(id);
            if (ad == null)
                return Json(new { success = false, message = "İlan bulunamadı!" });

            _context.PetAds.Remove(ad);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

    }
}
