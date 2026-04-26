using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Models;
using Biblioteca.Services;

namespace Biblioteca.Controllers
{
    public class CategorieController : Controller
    {
        private readonly ICategorieService _service;

        public CategorieController(ICategorieService service) => _service = service;

        public async Task<IActionResult> Index()
            => View(await _service.GetToateCategoriile());

        public async Task<IActionResult> Detalii(int id)
        {
            var categorie = await _service.GetDetaliiAsync(id);
            if (categorie == null) return NotFound();
            return View(categorie);
        }

        public IActionResult Adauga() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Adauga(Categorie categorie)
        {
            if (!ModelState.IsValid) return View(categorie);
            var (succes, mesaj) = await _service.AdaugaAsync(categorie);
            if (!succes) { ModelState.AddModelError("", mesaj); return View(categorie); }
            TempData["Succes"] = mesaj;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Editeaza(int id)
        {
            var categorie = await _service.GetDetaliiAsync(id);
            if (categorie == null) return NotFound();
            return View(categorie);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Editeaza(int id, Categorie categorie)
        {
            if (id != categorie.Id_Categorie) return NotFound();
            if (!ModelState.IsValid) return View(categorie);
            var (succes, mesaj) = await _service.ActualizeazaAsync(categorie);
            if (!succes) { ModelState.AddModelError("", mesaj); return View(categorie); }
            TempData["Succes"] = mesaj;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Sterge(int id)
        {
            var categorie = await _service.GetDetaliiAsync(id);
            if (categorie == null) return NotFound();
            return View(categorie);
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