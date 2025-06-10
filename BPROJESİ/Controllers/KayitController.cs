using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BPROJESİ.Models;  // ApplicationUser modeli buradaysa
using BPROJESİ.Models; // RegisterViewModel varsa buradaysa


public class KayitController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public KayitController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult kayit()
    {
        return View();
    }




    [HttpPost]
    public async Task<IActionResult> kayit(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = new ApplicationUser
        {
            UserName = model.Username,
            Email = model.Email,
            BirthDate = model.BirthDate
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            ViewBag.Success = true; // Bu sayede modal tetiklenecek
            return View(); // Aynı sayfaya dönüyoruz
        }

        return View(model);
    }



}
