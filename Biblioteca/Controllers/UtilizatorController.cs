using Biblioteca.Models;
using Biblioteca.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers
{
    [Authorize]
    public class UtilizatorController : Controller
    {
        private readonly IUtilizatorService _utilizatorService;
        private readonly UserManager<Utilizator> _userManager;

        public UtilizatorController(IUtilizatorService utilizatorService, UserManager<Utilizator> userManager)
        {
            _utilizatorService = utilizatorService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Profil()
        {
            var utilizator = await _userManager.GetUserAsync(User);
            if (utilizator == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            var profil = await _utilizatorService.GetProfilAsync(utilizator.Id);
            if (profil == null) return NotFound();

            return View(profil);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizeazaProfil(
            string numeComplet, string? telefon, string? adresa, IFormFile? pozaProfil)
        {
            var utilizator = await _userManager.GetUserAsync(User);
            if (utilizator == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            var (succes, mesaj) = await _utilizatorService.ActualizeazaProfilAsync(
                utilizator.Id, numeComplet, telefon, adresa, pozaProfil);

            TempData[succes ? "Succes" : "Eroare"] = mesaj;
            return RedirectToAction(nameof(Profil));
        }
    }
}