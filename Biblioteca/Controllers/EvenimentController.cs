using Biblioteca.Models;
using Biblioteca.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers
{
    public class EvenimentController : Controller
    {
        private readonly IEvenimentService _service;
        private readonly IRezervareEvenimentService _rezervareService;
        private readonly UserManager<Utilizator> _userManager;

        public EvenimentController(IEvenimentService service, IRezervareEvenimentService rezervareService, UserManager<Utilizator> userManager)
        {
            _service = service;
            _rezervareService = rezervareService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
            => View(await _service.GetEvenimenteViitoareAsync());

        public async Task<IActionResult> Detalii(int id)
        {
            var ev = await _service.GetDetaliiAsync(id);
            if (ev == null) return NotFound();
            return View(ev);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Adauga() => View();

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Adauga(Eveniment eveniment)
        {
            ModelState.Remove("Rezervari");
            if (!ModelState.IsValid) return View(eveniment);
            var (succes, mesaj) = await _service.AdaugaAsync(eveniment);
            if (!succes) { ModelState.AddModelError("", mesaj); return View(eveniment); }
            TempData["Succes"] = mesaj;
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Editeaza(int id)
        {
            var ev = await _service.GetDetaliiAsync(id);
            if (ev == null) return NotFound();
            return View(ev);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Editeaza(int id, Eveniment eveniment)
        {
            if (id != eveniment.Id_Eveniment) return NotFound();
            ModelState.Remove("Rezervari");
            if (!ModelState.IsValid) return View(eveniment);
            var (succes, mesaj) = await _service.ActualizeazaAsync(eveniment);
            if (!succes) { ModelState.AddModelError("", mesaj); return View(eveniment); }
            TempData["Succes"] = mesaj;
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Sterge(int id)
        {
            var ev = await _service.GetDetaliiAsync(id);
            if (ev == null) return NotFound();
            return View(ev);
        }

        [HttpPost, ActionName("Sterge"), ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> StergeConfirmat(int id)
        {
            var (succes, mesaj) = await _service.StergeAsync(id);
            TempData[succes ? "Succes" : "Eroare"] = mesaj;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Rezerva(int idEveniment)
        {
            var utilizator = await _userManager.GetUserAsync(User);
            if (utilizator == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            var (succes, mesaj) = await _rezervareService.RezervaloculAsync(utilizator.Id, idEveniment);
            TempData[succes ? "Succes" : "Eroare"] = mesaj;
            return RedirectToAction(nameof(Index));
        }
    }
}