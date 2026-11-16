using Biblioteca.Models;

namespace Biblioteca.Services.Interfaces
{
    public interface IRezervareEvenimentService
    {
        Task<IEnumerable<Rezervare_Eveniment>> GetRezervariUtilizatorAsync(string idUtilizator);
        Task<IEnumerable<Rezervare_Eveniment>> GetRezervariEvenimentAsync(int idEveniment);
        Task<(bool succes, string mesaj)> RezervaloculAsync(string idUtilizator, int idEveniment);
        Task<(bool succes, string mesaj)> AnuleazaRezervareAsync(int idRezervare, string idUtilizatorCurent);
    }
}
