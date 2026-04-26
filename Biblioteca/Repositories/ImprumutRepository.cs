using Biblioteca.Models;
using Microsoft.EntityFrameworkCore;
using static Biblioteca.Models.Imprumut;

namespace Biblioteca.Repositories
{
    public class ImprumutRepository : Repository<Imprumut>, IImprumutRepository
    {
        public ImprumutRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Imprumut>> GetByUtilizatorAsync(int idUtilizator)
            => await _context.Imprumuturi
                .Include(i => i.Carte)
                    .ThenInclude(c => c!.Autor)
                .Where(i => i.Id_Utilizator == idUtilizator)
                .OrderByDescending(i => i.Data_Imprumut)
                .ToListAsync();

        public async Task<IEnumerable<Imprumut>> GetActiveAsync()
            => await _context.Imprumuturi
                .Include(i => i.Carte)
                .Include(i => i.Utilizator)
                .Where(i => i.Status == StatusImprumut.Activ || i.Status == StatusImprumut.Prelungit)
                .ToListAsync();

        public async Task<IEnumerable<Imprumut>> GetIntarziateAsync()
            => await _context.Imprumuturi
                .Include(i => i.Carte)
                .Include(i => i.Utilizator)
                .Where(i => i.Status == StatusImprumut.Intarziat
                         || (i.Data_Returnare == null && i.Data_Scadenta < DateTime.UtcNow))
                .ToListAsync();

        public async Task<Imprumut?> GetWithDetailsAsync(int id)
            => await _context.Imprumuturi
                .Include(i => i.Carte)
                    .ThenInclude(c => c!.Autor)
                .Include(i => i.Utilizator)
                .FirstOrDefaultAsync(i => i.Id_Imprumut == id);

        public async Task<bool> AreImprumutActivAsync(int idUtilizator, int idCarte)
            => await _context.Imprumuturi
                .AnyAsync(i => i.Id_Utilizator == idUtilizator
                            && i.Id_Carte == idCarte
                            && (i.Status == StatusImprumut.Activ || i.Status == StatusImprumut.Prelungit));

        public async Task<bool> UtilizatorExistaAsync(int idUtilizator)
            => await _context.Utilizatori
                .AnyAsync(u => u.Id_Utilizator == idUtilizator);
    }
}

