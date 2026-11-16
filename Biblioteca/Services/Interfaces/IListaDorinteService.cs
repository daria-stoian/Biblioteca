using Biblioteca.Models;

namespace Biblioteca.Services.Interfaces
{
    public interface IListaDorinteService
    {
        Task<IEnumerable<Lista_Dorinte>> GetListaUtilizatorAsync(string idUtilizator);
        Task<(bool succes, string mesaj)> AdaugaInListaAsync(string idUtilizator, int idCarte);
        Task<(bool succes, string mesaj)> StergedinListaAsync(string idUtilizator, int idCarte);
    }
}
