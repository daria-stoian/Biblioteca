using Biblioteca.Models;

namespace Biblioteca.Repositories.Interfaces
{
    public interface IRezervareEvenimentRepository : IBaseRepository<Rezervare_Eveniment>
    {
        Task<IEnumerable<Rezervare_Eveniment>> GetByUtilizatorAsync(int idUtilizator);
        Task<IEnumerable<Rezervare_Eveniment>> GetByEvenimentAsync(int idEveniment);
        Task<bool> AreRezervareActivaAsync(int idUtilizator, int idEveniment);
        Task<Rezervare_Eveniment?> GetWithDetailsAsync(int id);
    }
}
