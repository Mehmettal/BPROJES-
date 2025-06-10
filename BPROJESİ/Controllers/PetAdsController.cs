using BPROJESİ.Data;
using BPROJESİ.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BPROJESİ.Controllers
{
    public class PetAdsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PetAdsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Fotoğrafları ve model verisini alan Create POST action
        [HttpPost]
        public async Task<IActionResult> Create(PetAd model, List<IFormFile> Photos)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            // Fotoğrafları yükle
            if (Photos != null && Photos.Count > 0)
            {
                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadDir))
                    Directory.CreateDirectory(uploadDir);

                var photoPaths = new List<string>();

                for (int i = 0; i < Photos.Count; i++)
                {
                    var photo = Photos[i];
                    var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(photo.FileName)}";
                    var filePath = Path.Combine(uploadDir, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await photo.CopyToAsync(stream);
                    }

                    photoPaths.Add("/uploads/" + uniqueFileName);
                }

                model.PhotoPaths = string.Join(",", photoPaths);
            }

            _context.PetAds.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }


        [HttpGet]
        public async Task<IActionResult> GetPetAds()
        {
            var ads = await _context.PetAds.ToListAsync();
            return Json(ads);
        }

        public async Task<IActionResult> Index()
        {
            var ads = await _context.PetAds.ToListAsync();
            return View(ads);
        }
    }
}
