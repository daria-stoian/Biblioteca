using Biblioteca.Models;
using Biblioteca.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using static Biblioteca.Models.Carte;

namespace Biblioteca.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICarteService _carteService;

        public HomeController(ICarteService carteService)
        {
            _carteService = carteService;
        }

        public async Task<IActionResult> Index()
        {
            var topCarti = await _carteService.GetTopCartiAsync(4);
            return View(topCarti);
        }

        public async Task<IActionResult> DespreNoi()
        {
            var toateCartile = await _carteService.GetToateCartileAsync();
            ViewBag.NrCarti = toateCartile.Count();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
