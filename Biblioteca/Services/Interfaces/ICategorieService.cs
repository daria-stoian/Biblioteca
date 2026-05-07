using Biblioteca.Models;

namespace Biblioteca.Services.Interfaces
{
    public interface ICategorieService
    {
        Task<IEnumerable<Categorie>> GetToateCategoriile();
        Task<Categorie?> GetDetaliiAsync(int id);
        Task<(bool succes, string mesaj)> AdaugaAsync(Categorie categorie);
        Task<(bool succes, string mesaj)> ActualizeazaAsync(Categorie categorie);
        Task<(bool succes, string mesaj)> StergeAsync(int id);
    }
}
