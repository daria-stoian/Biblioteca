using Biblioteca.Models;
using static Biblioteca.Models.Carte;

namespace Biblioteca.Services.Interfaces
{
    public interface ICarteService
    {
        Task<IEnumerable<Carte>> GetToateCartileAsync();
        Task<IEnumerable<Carte>> GetCartileDisponibileAsync();
        Task<IEnumerable<Carte>> GetByCategorieAsync(int idCategorie);
        Task<IEnumerable<Carte>> CautaCartiAsync(string termen);
        Task<Carte?> GetDetaliiCarteAsync(int id);
        Task<(bool succes, string mesaj)> AdaugaCarteAsync(Carte carte, IFormFile? imagine, IFormFile? pdf);
        Task<(bool succes, string mesaj)> ActualizeazaCarteAsync(Carte carte, IFormFile? imagine, IFormFile? pdf);
        Task<(bool succes, string mesaj)> StergeCarteAsync(int id);
        Task<(bool succes, string mesaj)> SetStatusAsync(int id, StatusDisponibilitate status);
        Task<(bool areAcces, string? caleFisier, string? numeFisier)> GetPDFAsync(int idCarte, int idUtilizator);
        Task<IEnumerable<Carte>> GetTopCartiAsync(int numar);
        Task<IEnumerable<Autor>> GetAutoriAsync();
        Task<IEnumerable<Categorie>> GetCategoriiAsync();
    }
}
