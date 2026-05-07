using Biblioteca.Models;

namespace Biblioteca.Repositories.Interfaces
{
    public interface IRecenzieRepository : IBaseRepository<Recenzie>
    {
        Task<IEnumerable<Recenzie>> GetByCarteAsync(int idCarte);
        Task<IEnumerable<Recenzie>> GetByUtilizatorAsync(int idUtilizator);
        Task<bool> AreRecenzieAsync(int idUtilizator, int idCarte);
        Task<double> GetNotaMedieAsync(int idCarte);
        Task<Recenzie?> GetWithDetailsAsync(int id);
    }
}
