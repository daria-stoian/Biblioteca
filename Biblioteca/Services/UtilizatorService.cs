using Biblioteca.Models;
using Biblioteca.Repositories.Interfaces;
using Biblioteca.Services.Interfaces;

namespace Biblioteca.Services
{
    public class UtilizatorService : IUtilizatorService
    {
        private readonly IRepositoryWrapper _repo;

        public UtilizatorService(IRepositoryWrapper repo) => _repo = repo;

        public async Task<Utilizator?> GetProfilAsync(int idUtilizator)
            => await _repo.Utilizatori.GetWithDetailsAsync(idUtilizator);

        public async Task<(bool succes, string mesaj)> ActualizeazaProfilAsync(
            int idUtilizator, string numeComplet, string? telefon, string? adresa)
        {
            var utilizator = await _repo.Utilizatori.GetByIdAsync(idUtilizator);
            if (utilizator == null) return (false, "Utilizatorul nu a fost găsit.");

            utilizator.Nume_Complet = numeComplet;
            utilizator.Telefon = telefon;
            utilizator.Adresa = adresa;

            await _repo.Utilizatori.UpdateAsync(utilizator);
            return (true, "Profilul a fost actualizat cu succes.");
        }
    }
}