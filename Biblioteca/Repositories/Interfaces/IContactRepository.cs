using Biblioteca.Models;

namespace Biblioteca.Repositories.Interfaces
{
    public interface IContactRepository : IBaseRepository<Contact>
    {
        Task<IEnumerable<Contact>> GetNecititeAsync();
        Task<IEnumerable<Contact>> GetByUtilizatorAsync(int idUtilizator);
    }
}
