using Microsoft.AspNetCore.Mvc;
using BPROJESİ.Models;
using System.Linq;
using System;
using BPROJESİ.Data;
public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var products = _context.Products.ToList();
        return View(products);
    }
}
