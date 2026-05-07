using Biblioteca.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers
{
    public class UtilizatorController : Controller
    {
        private readonly IUtilizatorService _utilizatorService;

        public UtilizatorController(IUtilizatorService utilizatorService)
        {
            _utilizatorService = utilizatorService;
        }

        public async Task<IActionResult> Profil()
        {
            int idUtilizatorCurent = 1; // temporar — va fi inlocuit cu sesiunea dupa Login

            var utilizator = await _utilizatorService.GetProfilAsync(idUtilizatorCurent);
            if (utilizator == null) return NotFound();

            return View(utilizator);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizeazaProfil(string numeComplet, string? telefon, string? adresa)
        {
            int idUtilizatorCurent = 1; // temporar

            var (succes, mesaj) = await _utilizatorService.ActualizeazaProfilAsync(
                idUtilizatorCurent, numeComplet, telefon, adresa);

            TempData[succes ? "Succes" : "Eroare"] = mesaj;
            return RedirectToAction(nameof(Profil));
        }
    }
}