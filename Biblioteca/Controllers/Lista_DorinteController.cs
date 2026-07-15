using Biblioteca.Models;
using Biblioteca.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers
{
    [Authorize]
    public class Lista_DorinteController : Controller
    {
        private readonly IListaDorinteService _service;
        private readonly UserManager<Utilizator> _userManager;

        public Lista_DorinteController(IListaDorinteService service, UserManager<Utilizator> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var utilizator = await _userManager.GetUserAsync(User);
            if (utilizator == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            var lista = await _service.GetListaUtilizatorAsync(utilizator.Id);
            return View(lista);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Adauga(int idCarte)
        {
            var utilizator = await _userManager.GetUserAsync(User);
            if (utilizator == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            var (succes, mesaj) = await _service.AdaugaInListaAsync(utilizator.Id, idCarte);
            TempData[succes ? "Succes" : "Eroare"] = mesaj;

            string? returnUrl = Request.Headers["Referer"].ToString();
            return !string.IsNullOrEmpty(returnUrl) ? Redirect(returnUrl) : RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Sterge(int idCarte)
        {
            var utilizator = await _userManager.GetUserAsync(User);
            if (utilizator == null)
                return RedirectToPage("/Account/Login", new { area = "Identity" });

            var (succes, mesaj) = await _service.StergedinListaAsync(utilizator.Id, idCarte);
            TempData[succes ? "Succes" : "Eroare"] = mesaj;
            return RedirectToAction(nameof(Index));
        }
    }
}