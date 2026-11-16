using Biblioteca.Models;
using Biblioteca.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repositories
{
    public class EvenimentRepository : BaseRepository<Eveniment>, IEvenimentRepository
    {
        public EvenimentRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Eveniment>> GetViitoareAsync()
            => await _context.Evenimente
                .Where(e => e.Data_Ora >= DateTime.UtcNow)
                .OrderBy(e => e.Data_Ora)
                .ToListAsync();

        public async Task<Eveniment?> GetWithRezervariAsync(int id)
            => await _context.Evenimente
                .Include(e => e.Rezervari!)
                    .ThenInclude(r => r.Utilizator)
                .FirstOrDefaultAsync(e => e.Id_Eveniment == id);
    }
}
