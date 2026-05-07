using Biblioteca.Models;

namespace Biblioteca.Services.Interfaces
{
    public interface IUtilizatorService
    {
        Task<Utilizator?> GetProfilAsync(int idUtilizator);
        Task<(bool succes, string mesaj)> ActualizeazaProfilAsync(int idUtilizator, string numeComplet, string? telefon, string? adresa);
    }
}