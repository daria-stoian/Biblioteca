using Biblioteca.Models;
using Biblioteca.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repositories
{
    public class ContactRepository : BaseRepository<Contact>, IContactRepository
    {
        public ContactRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Contact>> GetNecititeAsync()
            => await _context.Contacte
                .Where(c => c.Status_Mesaj == StatusMesaj.Necitit)
                .OrderByDescending(c => c.Data_Trimitere)
                .ToListAsync();

        public async Task<IEnumerable<Contact>> GetByUtilizatorAsync(int idUtilizator)
            => await _context.Contacte
                .Where(c => c.Id_Utilizator == idUtilizator)
                .OrderByDescending(c => c.Data_Trimitere)
                .ToListAsync();
    }
}
