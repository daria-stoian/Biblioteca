using Microsoft.AspNetCore.Mvc;
using Biblioteca.Models;
using Biblioteca.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Biblioteca.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService _service;
        private readonly UserManager<Utilizator> _userManager;

        public ContactController(IContactService service, UserManager<Utilizator> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        public IActionResult Index() => View(new Contact());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Trimite(Contact contact)
        {
            ModelState.Remove("Utilizator");
            if (!ModelState.IsValid) return View("Index", contact);

            // Daca utilizatorul e autentificat, atasam ID-ul sau la mesaj
            if (User.Identity?.IsAuthenticated == true)
            {
                var utilizator = await _userManager.GetUserAsync(User);
                if (utilizator != null)
                {
                    contact.Id_Utilizator = utilizator.Id;

                    // Prefill cu datele contului daca nu le-a completat manual
                    if (string.IsNullOrWhiteSpace(contact.Nume_Expeditor))
                        contact.Nume_Expeditor = utilizator.Nume_Complet;
                    if (string.IsNullOrWhiteSpace(contact.Email_Expeditor))
                        contact.Email_Expeditor = utilizator.Email;
                }
            }

            var (succes, mesaj) = await _service.TrimiteMessajAsync(contact);
            TempData[succes ? "Succes" : "Eroare"] = mesaj;
            return succes ? RedirectToAction(nameof(Index)) : View("Index", contact);
        }

        // Pagina de mesaje — doar Administrator
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Mesaje()
            => View(await _service.GetToateMesajeleAsync());

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> MarcheazaCitit(int id)
        {
            await _service.MarheazaCititAsync(id);
            return RedirectToAction(nameof(Mesaje));
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Sterge(int id)
        {
            await _service.StergeAsync(id);
            TempData["Succes"] = "Mesajul a fost șters.";
            return RedirectToAction(nameof(Mesaje));
        }
    }
}