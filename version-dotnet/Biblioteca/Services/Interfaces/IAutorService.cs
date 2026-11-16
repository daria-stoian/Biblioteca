using Biblioteca.Models;

namespace Biblioteca.Services.Interfaces
{
    public interface IAutorService
    {
        Task<IEnumerable<Autor>> GetToiAutoriiAsync();
        Task<IEnumerable<Autor>> CautaAutoriAsync(string termen);
        Task<Autor?> GetDetaliiAsync(int id);
        Task<(bool succes, string mesaj)> AdaugaAsync(Autor autor);
        Task<(bool succes, string mesaj)> ActualizeazaAsync(Autor autor);
        Task<(bool succes, string mesaj)> StergeAsync(int id);
    }
}
