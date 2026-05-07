using Biblioteca.Models;

namespace Biblioteca.Repositories.Interfaces
{
    public interface ICarteRepository : IBaseRepository<Carte>
    {
        Task<IEnumerable<Carte>> GetByAutorAsync(int idAutor);
        Task<IEnumerable<Carte>> GetByCategorieAsync(int idCategorie);
        Task<IEnumerable<Carte>> GetDisponibileAsync();
        Task<IEnumerable<Carte>> SearchAsync(string termen);
        Task<Carte?> GetWithDetailsAsync(int id);
        Task<IEnumerable<Carte>> GetTopByRecenziiAsync(int numar);
        Task<IEnumerable<Autor>> GetToiAutoriiAsync();
        Task<IEnumerable<Categorie>> GetToateCategoriiAsync();
        Task<Carte?> GetWithAutorAsync(int id);
    }
}
