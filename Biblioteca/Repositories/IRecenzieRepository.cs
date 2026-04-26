using Biblioteca.Models;

namespace Biblioteca.Repositories
{
    public interface IRecenzieRepository : IRepository<Recenzie>
    {
        Task<IEnumerable<Recenzie>> GetByCarteAsync(int idCarte);
        Task<IEnumerable<Recenzie>> GetByUtilizatorAsync(int idUtilizator);
        Task<bool> AreRecenzieAsync(int idUtilizator, int idCarte);
        Task<double> GetNotaMedieAsync(int idCarte);
        Task<Recenzie?> GetWithDetailsAsync(int id);
    }
}
