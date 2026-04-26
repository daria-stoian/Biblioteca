using Biblioteca.Models;

namespace Biblioteca.Services
{
    public interface IImprumutService
    {
        Task<IEnumerable<Imprumut>> GetImprumuturiUtilizatorAsync(int idUtilizator);
        Task<IEnumerable<Imprumut>> GetToateImprumuturileAsync();
        Task<Imprumut?> GetDetaliiImprumutAsync(int id);
        Task<(bool succes, string mesaj)> ImprumutaCarteAsync(int idUtilizator, int idCarte, int zileTermen);
        Task<(bool succes, string mesaj)> ReturnareCarteAsync(int idImprumut);
        Task<(bool succes, string mesaj)> PrelungesteImprumutAsync(int idImprumut, int zileExtra);
        decimal CalculeazaTaxaPenalizareAsync(Imprumut imprumut);
        Task<bool> UtilizatorExistaAsync(int idUtilizator);
        Task<Carte?> GetCarteSimplaAsync(int idCarte); // pentru pagina confirmare imprumut
    }
}
