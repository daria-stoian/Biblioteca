using Biblioteca.Models;

namespace Biblioteca.Repositories
{
    public interface ICategorieRepository : IRepository<Categorie>
    {
        Task<Categorie?> GetWithCartiAsync(int id);
        Task<bool> AreCartiAsync(int id);
    }
}
