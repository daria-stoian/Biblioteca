using Biblioteca.Models;

namespace Biblioteca.Repositories.Interfaces
{
    public interface IImprumutRepository : IBaseRepository<Imprumut>
    {
        Task<IEnumerable<Imprumut>> GetByUtilizatorAsync(int idUtilizator);
        Task<IEnumerable<Imprumut>> GetActiveAsync();
        Task<IEnumerable<Imprumut>> GetIntarziateAsync();
        Task<Imprumut?> GetWithDetailsAsync(int id);
        Task<bool> AreImprumutActivAsync(int idUtilizator, int idCarte);
        Task ReturnareDirectaAsync(int idImprumut, DateTime dataReturnare, decimal taxaPenalizare);
        Task<bool> UtilizatorExistaAsync(int idUtilizator);
    }
}