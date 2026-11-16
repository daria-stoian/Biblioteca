using Biblioteca.Models;

namespace Biblioteca.Repositories.Interfaces
{
    public interface IAutorRepository : IBaseRepository<Autor>
    {
        Task<Autor?> GetWithCartiAsync(int id);
        Task<IEnumerable<Autor>> SearchAsync(string termen);
        Task<bool> AreCartiAsync(int id);
    }
}
