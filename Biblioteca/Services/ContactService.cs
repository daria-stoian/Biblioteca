using Biblioteca.Models;
using Biblioteca.Repositories;

namespace Biblioteca.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _repo;
        public ContactService(IContactRepository repo) => _repo = repo;

        public async Task<IEnumerable<Contact>> GetToateMesajeleAsync()
            => await _repo.GetAllAsync();

        public async Task<Contact?> GetDetaliiAsync(int id)
            => await _repo.GetByIdAsync(id);

        public async Task<(bool succes, string mesaj)> TrimiteMessajAsync(Contact contact)
        {
            if (string.IsNullOrWhiteSpace(contact.Mesaj))
                return (false, "Mesajul nu poate fi gol.");

            contact.Data_Trimitere = DateTime.UtcNow;
            contact.Status_Mesaj = StatusMesaj.Necitit;
            await _repo.AddAsync(contact);
            return (true, "Mesajul tău a fost trimis. Te vom contacta în curând!");
        }

        public async Task<(bool succes, string mesaj)> MarheazaCititAsync(int id)
        {
            var mesaj = await _repo.GetByIdAsync(id);
            if (mesaj == null) return (false, "Mesajul nu a fost găsit.");

            mesaj.Status_Mesaj = StatusMesaj.Citit;
            await _repo.UpdateAsync(mesaj);
            return (true, "Mesaj marcat ca citit.");
        }

        public async Task<(bool succes, string mesaj)> StergeAsync(int id)
        {
            await _repo.DeleteAsync(id);
            return (true, "Mesajul a fost șters.");
        }
    }
}
