using Biblioteca.Models;

namespace Biblioteca.Repositories.Interfaces
{
    public interface ICategorieRepository : IBaseRepository<Categorie>
    {
        Task<Categorie?> GetWithCartiAsync(int id);
        Task<bool> AreCartiAsync(int id);
    }
}
