using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Models;
using Biblioteca.Services;

namespace Biblioteca.Controllers
{
    public class EvenimentController : Controller
    {
        private readonly IEvenimentService _service;
        private readonly IRezervareEvenimentService _rezervareService;

        public EvenimentController(IEvenimentService service, IRezervareEvenimentService rezervareService)
        {
            _service = service;
            _rezervareService = rezervareService;
        }

        public async Task<IActionResult> Index()
            => View(await _service.GetEvenimenteViitoareAsync());

        public async Task<IActionResult> Detalii(int id)
        {
            var ev = await _service.GetDetaliiAsync(id);
            if (ev == null) return NotFound();
            return View(ev);
        }

        public IActionResult Adauga() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Adauga(Eveniment eveniment)
        {
            ModelState.Remove("Rezervari");
            if (!ModelState.IsValid) return View(eveniment);
            var (succes, mesaj) = await _service.AdaugaAsync(eveniment);
            if (!succes) { ModelState.AddModelError("", mesaj); return View(eveniment); }
            TempData["Succes"] = mesaj;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Editeaza(int id)
        {
            var ev = await _service.GetDetaliiAsync(id);
            if (ev == null) return NotFound();
            return View(ev);
        }

        [HttpPost, ValidateAntiForgeryToken]
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

        public async Task<IActionResult> Sterge(int id)
        {
            var ev = await _service.GetDetaliiAsync(id);
            if (ev == null) return NotFound();
            return View(ev);
        }

        [HttpPost, ActionName("Sterge"), ValidateAntiForgeryToken]
        public async Task<IActionResult> StergeConfirmat(int id)
        {
            var (succes, mesaj) = await _service.StergeAsync(id);
            TempData[succes ? "Succes" : "Eroare"] = mesaj;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Rezerva(int idEveniment)
        {
            int idUtilizator = 1; // temporar
            var (succes, mesaj) = await _rezervareService.RezervaloculAsync(idUtilizator, idEveniment);
            TempData[succes ? "Succes" : "Eroare"] = mesaj;
            return RedirectToAction(nameof(Index));
        }
    }
}
