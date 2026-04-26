using Biblioteca.Models;

namespace Biblioteca.Repositories
{
    public interface IImprumutRepository : IRepository<Imprumut>
    {
        Task<IEnumerable<Imprumut>> GetByUtilizatorAsync(int idUtilizator);
        Task<IEnumerable<Imprumut>> GetActiveAsync();
        Task<IEnumerable<Imprumut>> GetIntarziateAsync();
        Task<Imprumut?> GetWithDetailsAsync(int id);
        Task<bool> AreImprumutActivAsync(int idUtilizator, int idCarte);
        Task<bool> UtilizatorExistaAsync(int idUtilizator);
    }
}
