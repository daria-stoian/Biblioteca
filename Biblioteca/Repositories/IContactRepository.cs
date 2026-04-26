using Biblioteca.Models;

namespace Biblioteca.Repositories
{
    public interface IContactRepository : IRepository<Contact>
    {
        Task<IEnumerable<Contact>> GetNecititeAsync();
        Task<IEnumerable<Contact>> GetByUtilizatorAsync(int idUtilizator);
    }
}
