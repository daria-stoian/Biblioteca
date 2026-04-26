using Biblioteca.Models;

namespace Biblioteca.Services
{
    public interface IRezervareEvenimentService
    {
        Task<IEnumerable<Rezervare_Eveniment>> GetRezervariUtilizatorAsync(int idUtilizator);
        Task<IEnumerable<Rezervare_Eveniment>> GetRezervariEvenimentAsync(int idEveniment);
        Task<(bool succes, string mesaj)> RezervaloculAsync(int idUtilizator, int idEveniment);
        Task<(bool succes, string mesaj)> AnuleazaRezervareAsync(int idRezervare, int idUtilizatorCurent);
    }
}
