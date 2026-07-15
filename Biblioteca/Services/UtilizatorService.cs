using Biblioteca.Models;
using Biblioteca.Repositories.Interfaces;
using Biblioteca.Services.Interfaces;

namespace Biblioteca.Services
{
    public class UtilizatorService : IUtilizatorService
    {
        private readonly IRepositoryWrapper _repo;
        private readonly IWebHostEnvironment _env;

        public UtilizatorService(IRepositoryWrapper repo, IWebHostEnvironment env)
        {
            _repo = repo;
            _env = env;
        }

        public async Task<Utilizator?> GetProfilAsync(int idUtilizator)
            => await _repo.Utilizatori.GetWithDetailsAsync(idUtilizator);

        public async Task<(bool succes, string mesaj)> ActualizeazaProfilAsync(
            int idUtilizator, string numeComplet, string? telefon, string? adresa, IFormFile? pozaProfil)
        {
            var utilizator = await _repo.Utilizatori.GetByIdAsync(idUtilizator);
            if (utilizator == null) return (false, "Utilizatorul nu a fost găsit.");

            utilizator.Nume_Complet = numeComplet;
            utilizator.Telefon = telefon;
            utilizator.Adresa = adresa;

            if (pozaProfil != null && pozaProfil.Length > 0)
            {
                var rezultat = await SalveazaPozaProfilAsync(pozaProfil, utilizator.Poza_Profil);
                if (!rezultat.succes) return (false, rezultat.mesaj);
                utilizator.Poza_Profil = rezultat.numeFisier;
            }

            await _repo.Utilizatori.UpdateAsync(utilizator);
            return (true, "Profilul a fost actualizat cu succes.");
        }

        private async Task<(bool succes, string mesaj, string numeFisier)> SalveazaPozaProfilAsync(
            IFormFile poza, string? pozaVeche)
        {
            var extensiiPermise = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extensie = Path.GetExtension(poza.FileName).ToLower();

            if (!extensiiPermise.Contains(extensie))
                return (false, "Formatul nu este acceptat. Folosește JPG, PNG sau WEBP.", "");

            if (poza.Length > 3 * 1024 * 1024)
                return (false, "Imaginea depășește limita de 3MB.", "");

            string folder = Path.Combine(_env.WebRootPath, "poze-profil");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            if (!string.IsNullOrEmpty(pozaVeche))
            {
                string caleVeche = Path.Combine(folder, pozaVeche);
                if (File.Exists(caleVeche)) File.Delete(caleVeche);
            }

            string numeFisier = Guid.NewGuid().ToString() + extensie;
            string cale = Path.Combine(folder, numeFisier);

            using var stream = new FileStream(cale, FileMode.Create);
            await poza.CopyToAsync(stream);

            return (true, "", numeFisier);
        }
    }
}