using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BPROJESİ.Models; // ApplicationUser modelin buradaysa
using BPROJESİ.Data;

[Authorize]
public class UserController : Controller
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetProfileJson()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound();

        return Json(new
        {
            user.UserName,
            user.Email,
            user.PhoneNumber,
          
        });
    }

    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }

    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);
        return View(user);
    }
}
