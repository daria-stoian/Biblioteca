using Biblioteca.Models;
using Biblioteca.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Controllers
{
    public class AutorController : Controller
    {
        private readonly IAutorService _service;

        public AutorController(IAutorService service) => _service = service;

        public async Task<IActionResult> Index(string? cautare)
        {
            var autori = await _service.CautaAutoriAsync(cautare ?? "");
            ViewBag.Cautare = cautare;
            return View(autori);
        }

        public async Task<IActionResult> Detalii(int id)
        {
            var autor = await _service.GetDetaliiAsync(id);
            if (autor == null) return NotFound();
            return View(autor);
        }

        public IActionResult Adauga() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Adauga(Autor autor)
        {
            if (!ModelState.IsValid) return View(autor);
            var (succes, mesaj) = await _service.AdaugaAsync(autor);
            if (!succes) { ModelState.AddModelError("", mesaj); return View(autor); }
            TempData["Succes"] = mesaj;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Editeaza(int id)
        {
            var autor = await _service.GetDetaliiAsync(id);
            if (autor == null) return NotFound();
            return View(autor);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Editeaza(int id, Autor autor)
        {
            if (id != autor.Id_Autor) return NotFound();
            ModelState.Remove("Carti");
            if (!ModelState.IsValid) return View(autor);
            var (succes, mesaj) = await _service.ActualizeazaAsync(autor);
            if (!succes) { ModelState.AddModelError("", mesaj); return View(autor); }
            TempData["Succes"] = mesaj;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Sterge(int id)
        {
            var autor = await _service.GetDetaliiAsync(id);
            if (autor == null) return NotFound();
            return View(autor);
        }

        [HttpPost, ActionName("Sterge"), ValidateAntiForgeryToken]
        public async Task<IActionResult> StergeConfirmat(int id)
        {
            var (succes, mesaj) = await _service.StergeAsync(id);
            TempData[succes ? "Succes" : "Eroare"] = mesaj;
            return RedirectToAction(nameof(Index));
        }
    }
}