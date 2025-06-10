using Microsoft.AspNetCore.Mvc;
using BPROJESİ.Models;
using System.Diagnostics;

namespace BPROJESİ.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }



    }
}