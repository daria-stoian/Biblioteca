using Biblioteca.Models;

namespace Biblioteca.Services.Interfaces
{
    public interface IEvenimentService
    {
        Task<IEnumerable<Eveniment>> GetToateEvenimenteleAsync();
        Task<IEnumerable<Eveniment>> GetEvenimenteViitoareAsync();
        Task<Eveniment?> GetDetaliiAsync(int id);
        Task<(bool succes, string mesaj)> AdaugaAsync(Eveniment eveniment);
        Task<(bool succes, string mesaj)> ActualizeazaAsync(Eveniment eveniment);
        Task<(bool succes, string mesaj)> StergeAsync(int id);
    }
}
