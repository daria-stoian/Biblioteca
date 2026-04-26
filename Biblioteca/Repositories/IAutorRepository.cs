using Biblioteca.Models;

namespace Biblioteca.Repositories
{
    public interface IAutorRepository : IRepository<Autor>
    {
        Task<Autor?> GetWithCartiAsync(int id);
        Task<IEnumerable<Autor>> SearchAsync(string termen);
        Task<bool> AreCartiAsync(int id);
    }
}
