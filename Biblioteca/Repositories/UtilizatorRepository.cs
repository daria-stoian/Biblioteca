using Biblioteca.Models;
using Biblioteca.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repositories
{
    public class UtilizatorRepository : BaseRepository<Utilizator>, IUtilizatorRepository
    {
        public UtilizatorRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Utilizator?> GetWithDetailsAsync(int id)
            => await _context.Users
                .Include(u => u.Imprumuturi!)
                    .ThenInclude(i => i.Carte)
                .Include(u => u.Recenzii!)
                    .ThenInclude(r => r.Carte)
                .Include(u => u.RezervariEvenimente!)
                    .ThenInclude(re => re.Eveniment)
                .FirstOrDefaultAsync(u => u.Id == id);
    }
}