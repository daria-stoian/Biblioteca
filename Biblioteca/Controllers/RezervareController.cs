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
    public class RezervareController : Controller
    {
        private readonly IRezervareEvenimentService _service;

        public RezervareController(IRezervareEvenimentService service) => _service = service;

        // Rezervaile mele
        public async Task<IActionResult> Index()
        {
            int idUtilizator = 1; // temporar
            var rezervari = await _service.GetRezervariUtilizatorAsync(idUtilizator);
            return View(rezervari);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Anuleaza(int id)
        {
            int idUtilizator = 1; // temporar
            var (succes, mesaj) = await _service.AnuleazaRezervareAsync(id, idUtilizator);
            TempData[succes ? "Succes" : "Eroare"] = mesaj;
            return RedirectToAction(nameof(Index));
        }
    }
}
