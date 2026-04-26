using Biblioteca.Models;

namespace Biblioteca.Repositories
{
    public interface IListaDorinteRepository
    {
        Task<IEnumerable<Lista_Dorinte>> GetByUtilizatorAsync(int idUtilizator);
        Task<bool> ExistaInListaAsync(int idUtilizator, int idCarte);
        Task AdaugaAsync(Lista_Dorinte item);
        Task StergeAsync(int idUtilizator, int idCarte);
    }
}
