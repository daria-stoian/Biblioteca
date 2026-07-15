#nullable disable
using System.ComponentModel.DataAnnotations;
using Biblioteca.Models;
using Biblioteca.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Biblioteca.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly SignInManager<Utilizator> _signInManager;

        public RegisterModel(IAccountService accountService, SignInManager<Utilizator> signInManager)
        {
            _accountService = accountService;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Numele complet este obligatoriu.")]
            [StringLength(100)]
            [Display(Name = "Nume complet")]
            public string Nume_Complet { get; set; }

            [Required(ErrorMessage = "Email-ul este obligatoriu.")]
            [EmailAddress(ErrorMessage = "Introdu un email valid.")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Parola este obligatorie.")]
            [StringLength(100, MinimumLength = 6,
                ErrorMessage = "Parola trebuie sa aiba cel putin 6 caractere.")]
            [DataType(DataType.Password)]
            [Display(Name = "Parola")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirma parola")]
            [Compare("Password", ErrorMessage = "Parolele nu coincid.")]
            public string ConfirmPassword { get; set; }

            [Display(Name = "Telefon")]
            public string Telefon { get; set; }

            [Display(Name = "Adresa")]
            public string Adresa { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (!ModelState.IsValid) return Page();

            var (succes, erori) = await _accountService.RegisterAsync(
                Input.Nume_Complet, Input.Email, Input.Password,
                Input.Telefon, Input.Adresa);

            if (succes)
                return LocalRedirect(returnUrl);

            foreach (var eroare in erori)
                ModelState.AddModelError(string.Empty, eroare);

            return Page();
        }
    }
}
