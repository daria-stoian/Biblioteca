using Biblioteca.Models;

namespace Biblioteca.Repositories
{
    public interface IEvenimentRepository : IRepository<Eveniment>
    {
        Task<IEnumerable<Eveniment>> GetViitoareAsync();
        Task<Eveniment?> GetWithRezervariAsync(int id);
    }
}
