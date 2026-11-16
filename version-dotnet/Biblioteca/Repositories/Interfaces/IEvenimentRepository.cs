using Biblioteca.Models;

namespace Biblioteca.Repositories.Interfaces
{
    public interface IEvenimentRepository : IBaseRepository<Eveniment>
    {
        Task<IEnumerable<Eveniment>> GetViitoareAsync();
        Task<Eveniment?> GetWithRezervariAsync(int id);
    }
}
