using Biblioteca.Models;
using Biblioteca.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Biblioteca.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<Utilizator> _userManager;
        private readonly SignInManager<Utilizator> _signInManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public AccountService(
            UserManager<Utilizator> userManager,
            SignInManager<Utilizator> signInManager,
            RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<(bool succes, string mesaj)> LoginAsync(string email, string parola, bool tineMinte)
        {
            var utilizator = await _userManager.FindByEmailAsync(email);
            if (utilizator == null)
                return (false, "Email sau parolă incorectă.");

            var rezultat = await _signInManager.PasswordSignInAsync(
                utilizator, parola, tineMinte, lockoutOnFailure: false);

            if (!rezultat.Succeeded)
                return (false, "Email sau parolă incorectă.");

            return (true, "Autentificare reușită!");
        }

        public async Task<(bool succes, IEnumerable<string> erori)> RegisterAsync(
            string numeComplet, string email, string parola, string? telefon, string? adresa)
        {
            var utilizator = new Utilizator
            {
                Nume_Complet = numeComplet,
                Email = email,
                UserName = email,
                Telefon = telefon,
                Adresa = adresa ?? ""
            };

            var rezultat = await _userManager.CreateAsync(utilizator, parola);

            if (!rezultat.Succeeded)
                return (false, rezultat.Errors.Select(e => e.Description));

            if (!await _roleManager.RoleExistsAsync("Cititor"))
                await _roleManager.CreateAsync(new IdentityRole<int>("Cititor"));

            await _userManager.AddToRoleAsync(utilizator, "Cititor");
            await _signInManager.SignInAsync(utilizator, isPersistent: false);

            return (true, Enumerable.Empty<string>());
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}