using Biblioteca.Models;

namespace Biblioteca.Services.Interfaces
{
    public interface IImprumutService
    {
        Task<IEnumerable<Imprumut>> GetImprumuturiUtilizatorAsync(string idUtilizator);
        Task<IEnumerable<Imprumut>> GetToateImprumuturileAsync();
        Task<Imprumut?> GetDetaliiImprumutAsync(int id);
        Task<(bool succes, string mesaj)> ImprumutaCarteAsync(string idUtilizator, int idCarte, int zileTermen);
        Task<(bool succes, string mesaj)> ReturnareCarteAsync(int idImprumut);
        Task<(bool succes, string mesaj)> PrelungesteImprumutAsync(int idImprumut, int zileExtra);
        decimal CalculeazaTaxaPenalizareAsync(Imprumut imprumut);
        Task<bool> UtilizatorExistaAsync(string idUtilizator);
        Task<Carte?> GetCarteSimplaAsync(int idCarte);
        Task<bool> AreImprumutActivAsync(string idUtilizator, int idCarte);
    }
}