using Biblioteca.Models;

namespace Biblioteca.Repositories.Interfaces
{
    public interface IListaDorinteRepository
    {
        Task<IEnumerable<Lista_Dorinte>> GetByUtilizatorAsync(string idUtilizator);
        Task<bool> ExistaInListaAsync(string idUtilizator, int idCarte);
        Task AdaugaAsync(Lista_Dorinte item);
        Task StergeAsync(string idUtilizator, int idCarte);
    }
}
