using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Models;
using Biblioteca.Services.Interfaces;

namespace Biblioteca.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService _service;

        public ContactController(IContactService service) => _service = service;

        public IActionResult Index() => View(new Contact());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Trimite(Contact contact)
        {
            ModelState.Remove("Utilizator");
            if (!ModelState.IsValid) return View("Index", contact);

            // Daca utilizatorul e autentificat, atasam ID-ul
            var (succes, mesaj) = await _service.TrimiteMessajAsync(contact);
            TempData[succes ? "Succes" : "Eroare"] = mesaj;
            return succes ? RedirectToAction(nameof(Index)) : View("Index", contact);
        }

        // Admin: lista mesaje
        public async Task<IActionResult> Mesaje()
            => View(await _service.GetToateMesajeleAsync());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> MarcheazaCitit(int id)
        {
            await _service.MarheazaCititAsync(id);
            return RedirectToAction(nameof(Mesaje));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Sterge(int id)
        {
            await _service.StergeAsync(id);
            TempData["Succes"] = "Mesajul a fost șters.";
            return RedirectToAction(nameof(Mesaje));
        }
    }
}
