using Biblioteca.Models;
using Biblioteca.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Biblioteca.Models.Carte;

namespace Biblioteca.Controllers
{
    public class RecenzieController : Controller
    {
        private readonly IRecenzieService _recenzieService;
        private readonly IImprumutService _imprumutService;

        public RecenzieController(IRecenzieService recenzieService, IImprumutService imprumutService)
        {
            _recenzieService = recenzieService;
            _imprumutService = imprumutService;
        }

        public async Task<IActionResult> Index()
        {
            var recenzii = await _recenzieService.GetToateRecenziile();
            return View(recenzii);
        }

        public async Task<IActionResult> Adauga(int? idCarte)
        {
            int idUtilizatorCurent = 1; // temporar

            await IncarcaCartiViewBag(idCarte);

            return View(new Recenzie
            {
                Id_Utilizator = idUtilizatorCurent,
                Id_Carte = idCarte ?? 0
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Adauga(Recenzie recenzie)
        {
            ModelState.Remove("Utilizator");
            ModelState.Remove("Carte");

            if (!ModelState.IsValid)
            {
                await IncarcaCartiViewBag(recenzie.Id_Carte);
                return View(recenzie);
            }

            bool utilizatorExista = await _imprumutService.UtilizatorExistaAsync(recenzie.Id_Utilizator);
            if (!utilizatorExista)
            {
                ModelState.AddModelError("", "Utilizatorul nu există în baza de date.");
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sterge(int id)
        {
            int idUtilizatorCurent = 1; // temporar
            var (succes, mesaj) = await _recenzieService.StergeRecenzieAsync(id, idUtilizatorCurent);
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
