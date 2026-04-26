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
    public class Lista_DorinteController : Controller
    {
        private readonly IListaDorinteService _service;

        public Lista_DorinteController(IListaDorinteService service) => _service = service;

        public async Task<IActionResult> Index()
        {
            int idUtilizator = 1; // temporar
            var lista = await _service.GetListaUtilizatorAsync(idUtilizator);
            return View(lista);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Adauga(int idCarte)
        {
            int idUtilizator = 1; // temporar
            var (succes, mesaj) = await _service.AdaugaInListaAsync(idUtilizator, idCarte);
            TempData[succes ? "Succes" : "Eroare"] = mesaj;

            // Redirectioneaza inapoi la pagina anterioara
            string? returnUrl = Request.Headers["Referer"].ToString();
            return !string.IsNullOrEmpty(returnUrl) ? Redirect(returnUrl) : RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Sterge(int idCarte)
        {
            int idUtilizator = 1; // temporar
            var (succes, mesaj) = await _service.StergedinListaAsync(idUtilizator, idCarte);
            TempData[succes ? "Succes" : "Eroare"] = mesaj;
            return RedirectToAction(nameof(Index));
        }
    }
}
