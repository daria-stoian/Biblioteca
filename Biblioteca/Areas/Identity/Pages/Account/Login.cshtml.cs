#nullable disable
using System.ComponentModel.DataAnnotations;
using Biblioteca.Models;
using Biblioteca.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Biblioteca.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly SignInManager<Utilizator> _signInManager;

        public LoginModel(IAccountService accountService, SignInManager<Utilizator> signInManager)
        {
            _accountService = accountService;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Email-ul este obligatoriu.")]
            [EmailAddress(ErrorMessage = "Introdu un email valid.")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Parola este obligatorie.")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Tine-ma minte")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
                ModelState.AddModelError(string.Empty, ErrorMessage);

            returnUrl ??= Url.Content("~/");
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (!ModelState.IsValid) return Page();

            var (succes, mesaj) = await _accountService.LoginAsync(
                Input.Email, Input.Password, Input.RememberMe);

            if (succes)
                return LocalRedirect(returnUrl);

            ModelState.AddModelError(string.Empty, mesaj);
            return Page();
        }
    }
}
