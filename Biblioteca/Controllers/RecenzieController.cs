using Biblioteca.Models;
using Biblioteca.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Biblioteca.Models.Carte;

namespace Biblioteca.Controllers
{
    public class RecenzieController : Controller
    {
        private readonly IRecenzieService _recenzieService;
        private readonly IImprumutService _imprumutService;
        private readonly UserManager<Utilizator> _userManager;

        public RecenzieController(IRecenzieService recenzieService, IImprumutService imprumutService, UserManager<Utilizator> userManager)
        {
            _recenzieService = recenzieService;
            _imprumutService = imprumutService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var recenzii = await _recenzieService.GetToateRecenziile();
            return View(recenzii);
        }

        [Authorize]
        public async Task<IActionResult> Adauga(int? idCarte)
        {
            var utilizator = await _userManager.GetUserAsync(User);
            if (utilizator == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            await IncarcaCartiViewBag(idCarte);

            return View(new Recenzie
            {
                Id_Utilizator = utilizator.Id,
                Id_Carte = idCarte ?? 0
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Adauga(Recenzie recenzie)
        {
            var utilizator = await _userManager.GetUserAsync(User);
            if (utilizator == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            recenzie.Id_Utilizator = utilizator.Id;
            ModelState.Remove("Utilizator");
            ModelState.Remove("Carte");

            if (!ModelState.IsValid)
            {
                await IncarcaCartiViewBag(recenzie.Id_Carte);
                return View(recenzie);
            }

            var (succes, mesaj) = await _recenzieService.AdaugaRecenzieAsync(recenzie);
            if (!succes)
            {
                ModelState.AddModelError("", mesaj);
                await IncarcaCartiViewBag(recenzie.Id_Carte);
                return View(recenzie);
            }

            TempData["Succes"] = mesaj;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Sterge(int id)
        {
            var utilizator = await _userManager.GetUserAsync(User);
            if (utilizator == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            var (succes, mesaj) = await _recenzieService.StergeRecenzieAsync(id, utilizator.Id);
            TempData[succes ? "Succes" : "Eroare"] = mesaj;
            return RedirectToAction(nameof(Index));
        }

        private async Task IncarcaCartiViewBag(int? idCarteCurenta)
        {
            var carti = await _recenzieService.GetCartileDisponibileAsync();
            ViewBag.Id_Carte = new SelectList(
                carti.Select(c => new { c.Id_Carte, Titlu = $"{c.Titlu} — {c.Autor?.Nume_Autor}" }),
                "Id_Carte", "Titlu", idCarteCurenta
            );
        }
    }
}