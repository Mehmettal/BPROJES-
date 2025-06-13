using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BPROJESİ.Models;

namespace BPROJESİ.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: /Home/Index  (Giriş ve Kayıt sayfası)
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Home/Register  (Ajax ile kayıt)
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { success = false, errors });
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                BirthDate = model.BirthDate
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                // Kayıt başarılı, profile sayfasına yönlendir
                return Ok(new { success = true, redirectUrl = Url.Action("Profile", "Profil") });
            }

            return BadRequest(new
            {
                success = false,
                errors = result.Errors.Select(e => e.Description)
            });
        }

        // POST: /Home/Login  (Ajax ile giriş)
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.UserName) || string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest(new { success = false, errors = new[] { "Kullanıcı adı ve şifre boş olamaz." } });
            }

            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                // Giriş başarılı, profil sayfasına yönlendir
                return Ok(new { success = true, redirectUrl = Url.Action("Profile", "Profil") });
            }

            return BadRequest(new { success = false, errors = new[] { "Geçersiz kullanıcı adı veya şifre." } });
        }

        // Örnek anasayfa için basit action (auth gerekirse)
        [HttpGet]
        public IActionResult Anasayfa()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index"); // Giriş yapmamışsa giriş sayfasına gönder
            }
            return View();
        }
    }
}
