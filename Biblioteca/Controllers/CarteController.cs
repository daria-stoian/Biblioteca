using Biblioteca.Models;
using Biblioteca.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static Biblioteca.Models.Carte;

namespace Biblioteca.Controllers
{
    public class CarteController : Controller
    {
        private readonly ICarteService _carteService;

        public CarteController(ICarteService carteService)
        {
            _carteService = carteService;
        }

        public async Task<IActionResult> Index(int? idCategorie, string? cautare)
        {
            IEnumerable<Carte> carti;

            if (!string.IsNullOrEmpty(cautare))
                carti = await _carteService.CautaCartiAsync(cautare);
            else if (idCategorie.HasValue)
                carti = await _carteService.GetByCategorieAsync(idCategorie.Value);
            else
                carti = await _carteService.GetCartileDisponibileAsync();

            // Categoriile vin din serviciu, nu din context
            ViewBag.Categorii = await _carteService.GetCategoriiAsync();
            ViewBag.IdCategorieCurenta = idCategorie;
            ViewBag.Cautare = cautare;
            return View(carti);
        }

        public async Task<IActionResult> Detalii(int id)
        {
            var carte = await _carteService.GetDetaliiCarteAsync(id);
            if (carte == null) return NotFound();
            return View(carte);
        }

        public async Task<IActionResult> Adauga()
        {
            await IncarcaSelectListuri();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Adauga(Carte carte, IFormFile? imagine)
        {
            ModelState.Remove("Autor");
            ModelState.Remove("Categorie");
            ModelState.Remove("Imagine_Coperta");

            if (!ModelState.IsValid)
            {
                await IncarcaSelectListuri(carte.Id_Autor, carte.Id_Categorie);
                return View(carte);
            }

            var (succes, mesaj) = await _carteService.AdaugaCarteAsync(carte, imagine);
            if (!succes)
            {
                ModelState.AddModelError("", mesaj);
                await IncarcaSelectListuri(carte.Id_Autor, carte.Id_Categorie);
                return View(carte);
            }

            TempData["Succes"] = mesaj;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Editeaza(int id)
        {
            var carte = await _carteService.GetDetaliiCarteAsync(id);
            if (carte == null) return NotFound();

            await IncarcaSelectListuri(carte.Id_Autor, carte.Id_Categorie);
            return View(carte);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editeaza(int id, Carte carte, IFormFile? imagine)
        {
            if (id != carte.Id_Carte) return NotFound();

            ModelState.Remove("Autor");
            ModelState.Remove("Categorie");
            ModelState.Remove("Imagine_Coperta");

            if (!ModelState.IsValid)
            {
                await IncarcaSelectListuri(carte.Id_Autor, carte.Id_Categorie);
                return View(carte);
            }

            var (succes, mesaj) = await _carteService.ActualizeazaCarteAsync(carte, imagine);
            if (!succes)
            {
                ModelState.AddModelError("", mesaj);
                await IncarcaSelectListuri(carte.Id_Autor, carte.Id_Categorie);
                return View(carte);
            }

            TempData["Succes"] = mesaj;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Sterge(int id)
        {
            var carte = await _carteService.GetDetaliiCarteAsync(id);
            if (carte == null) return NotFound();
            return View(carte);
        }

        [HttpPost, ActionName("Sterge")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StergeConfirmat(int id)
        {
            var (succes, mesaj) = await _carteService.StergeCarteAsync(id);
            TempData[succes ? "Succes" : "Eroare"] = mesaj;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetIndisponibila(int id)
        {
            var (succes, mesaj) = await _carteService.SetStatusAsync(id, StatusDisponibilitate.Indisponibil);
            TempData[succes ? "Succes" : "Eroare"] = mesaj;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetDisponibila(int id)
        {
            var (succes, mesaj) = await _carteService.SetStatusAsync(id, StatusDisponibilitate.Disponibil);
            TempData[succes ? "Succes" : "Eroare"] = mesaj;
            return RedirectToAction(nameof(Index));
        }

        private async Task IncarcaSelectListuri(int idAutor = 0, int idCategorie = 0)
        {
            var autori = await _carteService.GetAutoriAsync();
            var categorii = await _carteService.GetCategoriiAsync();

            ViewBag.Id_Autor = new SelectList(autori, "Id_Autor", "Nume_Autor", idAutor);
            ViewBag.Id_Categorie = new SelectList(categorii, "Id_Categorie", "Nume_Categorie", idCategorie);
        }
    }
}
