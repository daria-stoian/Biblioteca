using Biblioteca.Models;
using Biblioteca.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers
{
    [Authorize]
    public class RezervareController : Controller
    {
        private readonly IRezervareEvenimentService _service;
        private readonly UserManager<Utilizator> _userManager;

        public RezervareController(IRezervareEvenimentService service, UserManager<Utilizator> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var utilizator = await _userManager.GetUserAsync(User);
            if (utilizator == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            var rezervari = await _service.GetRezervariUtilizatorAsync(utilizator.Id);
            return View(rezervari);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Anuleaza(int id)
        {
            var utilizator = await _userManager.GetUserAsync(User);
            if (utilizator == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            var (succes, mesaj) = await _service.AnuleazaRezervareAsync(id, utilizator.Id);
            TempData[succes ? "Succes" : "Eroare"] = mesaj;
            return RedirectToAction(nameof(Index));
        }
    }
}   