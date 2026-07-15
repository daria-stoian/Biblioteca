using Biblioteca.Models;
using Biblioteca.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Biblioteca.Models.Carte;

namespace Biblioteca.Controllers
{
    public class CarteController : Controller
    {
        private readonly ICarteService _carteService;
        private readonly UserManager<Utilizator> _userManager;
        private readonly IImprumutService _imprumutService;

        public CarteController(ICarteService carteService, UserManager<Utilizator> userManager, IImprumutService imprumutService)
        {
            _carteService = carteService;
            _userManager = userManager;
            _imprumutService = imprumutService;
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

            ViewBag.Categorii = await _carteService.GetCategoriiAsync();
            ViewBag.IdCategorieCurenta = idCategorie;
            ViewBag.Cautare = cautare;
            return View(carti);
        }

        public async Task<IActionResult> Detalii(int id)
        {
            var carte = await _carteService.GetDetaliiCarteAsync(id);
            if (carte == null) return NotFound();

            // Verificam daca utilizatorul curent are imprumut activ pentru aceasta carte
            ViewBag.AreImprumutActiv = false;
            if (User.Identity?.IsAuthenticated == true)
            {
                var utilizator = await _userManager.GetUserAsync(User);
                if (utilizator != null)
                    ViewBag.AreImprumutActiv = await _imprumutService.AreImprumutActivAsync(utilizator.Id, id);
            }

            return View(carte);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Adauga()
        {
            await IncarcaSelectListuri();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Adauga(Carte carte, IFormFile? imagine, IFormFile? pdf)
        {
            ModelState.Remove("Autor");
            ModelState.Remove("Categorie");
            ModelState.Remove("Imagine_Coperta");
            ModelState.Remove("Fisier_PDF");

            if (!ModelState.IsValid)
            {
                await IncarcaSelectListuri(carte.Id_Autor, carte.Id_Categorie);
                return View(carte);
            }

            var (succes, mesaj) = await _carteService.AdaugaCarteAsync(carte, imagine, pdf);
            if (!succes)
            {
                ModelState.AddModelError("", mesaj);
                await IncarcaSelectListuri(carte.Id_Autor, carte.Id_Categorie);
                return View(carte);
            }

            TempData["Succes"] = mesaj;
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Editeaza(int id)
        {
            var carte = await _carteService.GetDetaliiCarteAsync(id);
            if (carte == null) return NotFound();
            await IncarcaSelectListuri(carte.Id_Autor, carte.Id_Categorie);
            return View(carte);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Editeaza(int id, Carte carte, IFormFile? imagine, IFormFile? pdf)
        {
            if (id != carte.Id_Carte) return NotFound();
            ModelState.Remove("Autor");
            ModelState.Remove("Categorie");
            ModelState.Remove("Imagine_Coperta");
            ModelState.Remove("Fisier_PDF");

            if (!ModelState.IsValid)
            {
                await IncarcaSelectListuri(carte.Id_Autor, carte.Id_Categorie);
                return View(carte);
            }

            var (succes, mesaj) = await _carteService.ActualizeazaCarteAsync(carte, imagine, pdf);
            if (!succes)
            {
                ModelState.AddModelError("", mesaj);
                await IncarcaSelectListuri(carte.Id_Autor, carte.Id_Categorie);
                return View(carte);
            }

            TempData["Succes"] = mesaj;
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Sterge(int id)
        {
            var carte = await _carteService.GetDetaliiCarteAsync(id);
            if (carte == null) return NotFound();
            return View(carte);
        }

        [HttpPost, ActionName("Sterge"), ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> StergeConfirmat(int id)
        {
            var (succes, mesaj) = await _carteService.StergeCarteAsync(id);
            TempData[succes ? "Succes" : "Eroare"] = mesaj;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> SetIndisponibila(int id)
        {
            var (succes, mesaj) = await _carteService.SetStatusAsync(id, StatusDisponibilitate.Indisponibil);
            TempData[succes ? "Succes" : "Eroare"] = mesaj;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> SetDisponibila(int id)
        {
            var (succes, mesaj) = await _carteService.SetStatusAsync(id, StatusDisponibilitate.Disponibil);
            TempData[succes ? "Succes" : "Eroare"] = mesaj;
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> DescarcaPDF(int idCarte)
        {
            var utilizator = await _userManager.GetUserAsync(User);
            if (utilizator == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            var (areAcces, caleFisier, numeFisier) = await _carteService.GetPDFAsync(idCarte, utilizator.Id);

            if (!areAcces || caleFisier == null)
            {
                TempData["Eroare"] = "Nu ai acces la acest PDF. Împrumută cartea mai întâi.";
                return RedirectToAction(nameof(Detalii), new { id = idCarte });
            }

            return PhysicalFile(caleFisier, "application/pdf", numeFisier);
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