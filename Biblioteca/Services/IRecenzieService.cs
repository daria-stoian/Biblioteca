using Biblioteca.Models;

namespace Biblioteca.Services
{
    public interface IRecenzieService
    {
        Task<IEnumerable<Recenzie>> GetToateRecenziile();
        Task<IEnumerable<Recenzie>> GetRecenziiCarteAsync(int idCarte);
        Task<IEnumerable<Recenzie>> GetRecenziiUtilizatorAsync(int idUtilizator);
        Task<Recenzie?> GetDetaliiRecenzieAsync(int id);
        Task<double> GetNotaMedieCarteAsync(int idCarte);
        Task<(bool succes, string mesaj)> AdaugaRecenzieAsync(Recenzie recenzie);
        Task<(bool succes, string mesaj)> StergeRecenzieAsync(int idRecenzie, int idUtilizatorCurent);
        Task<IEnumerable<Carte>> GetCartileDisponibileAsync();
    }
}
