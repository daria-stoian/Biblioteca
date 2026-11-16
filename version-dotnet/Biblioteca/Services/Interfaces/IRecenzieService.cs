using Biblioteca.Models;

namespace Biblioteca.Services.Interfaces
{
    public interface IRecenzieService
    {
        Task<IEnumerable<Recenzie>> GetToateRecenziile();
        Task<IEnumerable<Recenzie>> GetRecenziiCarteAsync(int idCarte);
        Task<IEnumerable<Recenzie>> GetRecenziiUtilizatorAsync(string idUtilizator);
        Task<Recenzie?> GetDetaliiRecenzieAsync(int id);
        Task<double> GetNotaMedieCarteAsync(int idCarte);
        Task<(bool succes, string mesaj)> AdaugaRecenzieAsync(Recenzie recenzie);
        Task<(bool succes, string mesaj)> StergeRecenzieAsync(int idRecenzie, string idUtilizatorCurent);
        Task<IEnumerable<Carte>> GetCartileDisponibileAsync();
    }
}
