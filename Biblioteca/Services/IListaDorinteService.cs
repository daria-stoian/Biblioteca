using Biblioteca.Models;

namespace Biblioteca.Services
{
    public interface IListaDorinteService
    {
        Task<IEnumerable<Lista_Dorinte>> GetListaUtilizatorAsync(int idUtilizator);
        Task<(bool succes, string mesaj)> AdaugaInListaAsync(int idUtilizator, int idCarte);
        Task<(bool succes, string mesaj)> StergedinListaAsync(int idUtilizator, int idCarte);
    }
}
