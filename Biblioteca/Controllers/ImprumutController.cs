using Biblioteca.Models;
using Biblioteca.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Controllers
{
    public class ImprumutController : Controller
    {
        private readonly IImprumutService _imprumutService;

        public ImprumutController(IImprumutService imprumutService)
        {
            _imprumutService = imprumutService;
        }

        public async Task<IActionResult> Index()
        {
            bool utilizatorExista = await _imprumutService.UtilizatorExistaAsync(1);
            if (!utilizatorExista)
            {
                TempData["Eroare"] = "Nu există niciun utilizator în baza de date. Adaugă un utilizator mai întâi.";
                return View(new List<Imprumut>());
            }

            int idUtilizatorCurent = 1; // temporar — va fi inlocuit cu sesiunea dupa Login
            var imprumuturi = await _imprumutService.GetImprumuturiUtilizatorAsync(idUtilizatorCurent);
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
            bool utilizatorExista = await _imprumutService.UtilizatorExistaAsync(1);
            if (!utilizatorExista)
            {
                TempData["Eroare"] = "Nu există niciun utilizator înregistrat. Nu se poate face împrumutul.";
                return RedirectToAction("Index", "Carte");
            }

            int idUtilizatorCurent = 1; // temporar
            var (succes, mesaj) = await _imprumutService.ImprumutaCarteAsync(idUtilizatorCurent, idCarte, 14);

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
