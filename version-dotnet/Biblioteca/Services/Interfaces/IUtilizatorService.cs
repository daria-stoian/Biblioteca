using Biblioteca.Models;

namespace Biblioteca.Services.Interfaces
{
    public interface IUtilizatorService
    {
        Task<Utilizator?> GetProfilAsync(string idUtilizator);
        Task<(bool succes, string mesaj)> ActualizeazaProfilAsync(
            string idUtilizator, string numeComplet, string? telefon, string? adresa, IFormFile? pozaProfil);
    }
}