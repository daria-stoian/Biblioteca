using Biblioteca.Models;
using Biblioteca.Repositories.Interfaces;
using Biblioteca.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Services
{
    public class ContactService : IContactService
    {
        private readonly IRepositoryWrapper _repo;
        private readonly ApplicationDbContext _context;

        public ContactService(IRepositoryWrapper repo, ApplicationDbContext context)
        {
            _repo = repo;
            _context = context;
        }

        // Interogare directa cu Include si ordonare — ocoleste GetAllAsync din BaseRepository
        public async Task<IEnumerable<Contact>> GetToateMesajeleAsync()
            => await _context.Contacte
                .OrderByDescending(c => c.Data_Trimitere)
                .ToListAsync();

        public async Task<Contact?> GetDetaliiAsync(int id)
            => await _context.Contacte
                .Include(c => c.Utilizator)
                .FirstOrDefaultAsync(c => c.Id_Mesaj == id);

        public async Task<(bool succes, string mesaj)> TrimiteMessajAsync(Contact contact)
        {
            if (string.IsNullOrWhiteSpace(contact.Mesaj))
                return (false, "Mesajul nu poate fi gol.");

            contact.Data_Trimitere = DateTime.UtcNow;
            contact.Status_Mesaj = StatusMesaj.Necitit;

            await _context.Contacte.AddAsync(contact);
            await _context.SaveChangesAsync();

            return (true, "Mesajul tău a fost trimis. Te vom contacta în curând!");
        }

        public async Task<(bool succes, string mesaj)> MarheazaCititAsync(int id)
        {
            var mesaj = await _context.Contacte.FindAsync(id);
            if (mesaj == null) return (false, "Mesajul nu a fost găsit.");

            mesaj.Status_Mesaj = StatusMesaj.Citit;
            await _context.SaveChangesAsync();
            return (true, "Mesaj marcat ca citit.");
        }

        public async Task<(bool succes, string mesaj)> StergeAsync(int id)
        {
            var mesaj = await _context.Contacte.FindAsync(id);
            if (mesaj == null) return (false, "Mesajul nu a fost găsit.");

            _context.Contacte.Remove(mesaj);
            await _context.SaveChangesAsync();
            return (true, "Mesajul a fost șters.");
        }
    }
}