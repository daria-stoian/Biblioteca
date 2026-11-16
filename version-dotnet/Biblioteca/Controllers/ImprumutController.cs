using Biblioteca.Models;
using Biblioteca.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers
{
    [Authorize]
    public class ImprumutController : Controller
    {
        private readonly IImprumutService _imprumutService;
        private readonly UserManager<Utilizator> _userManager;

        public ImprumutController(IImprumutService imprumutService, UserManager<Utilizator> userManager)
        {
            _imprumutService = imprumutService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var utilizator = await _userManager.GetUserAsync(User);
            if (utilizator == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            var imprumuturi = await _imprumutService.GetImprumuturiUtilizatorAsync(utilizator.Id);
            return View(imprumuturi);
        }

        public async Task<IActionResult> Imprumuta(int idCarte)
        {
            var carte = await _imprumutService.GetCarteSimplaAsync(idCarte);
            if (carte == null) return NotFound();
            return View(carte);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImprumutaConfirmat(int idCarte)
        {
            var utilizator = await _userManager.GetUserAsync(User);
            if (utilizator == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            var (succes, mesaj) = await _imprumutService.ImprumutaCarteAsync(utilizator.Id, idCarte, 14);
            TempData[succes ? "Succes" : "Eroare"] = mesaj;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Returneaza(int id)
        {
            var (succes, mesaj) = await _imprumutService.ReturnareCarteAsync(id);
            TempData[succes ? "Succes" : "Eroare"] = mesaj;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Prelungeste(int id)
        {
            var (succes, mesaj) = await _imprumutService.PrelungesteImprumutAsync(id, 14);
            TempData[succes ? "Succes" : "Eroare"] = mesaj;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detalii(int id)
        {
            var imprumut = await _imprumutService.GetDetaliiImprumutAsync(id);
            if (imprumut == null) return NotFound();
            return View(imprumut);
        }
    }
}