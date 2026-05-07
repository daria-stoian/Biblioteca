using Biblioteca.Models;

namespace Biblioteca.Services.Interfaces
{
    public interface IContactService
    {
        Task<IEnumerable<Contact>> GetToateMesajeleAsync();
        Task<Contact?> GetDetaliiAsync(int id);
        Task<(bool succes, string mesaj)> TrimiteMessajAsync(Contact contact);
        Task<(bool succes, string mesaj)> MarheazaCititAsync(int id);
        Task<(bool succes, string mesaj)> StergeAsync(int id);
    }
}
