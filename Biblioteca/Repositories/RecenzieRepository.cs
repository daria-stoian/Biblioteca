using Biblioteca.Models;
using Biblioteca.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repositories
{
    public class RecenzieRepository : BaseRepository<Recenzie>, IRecenzieRepository
    {
        public RecenzieRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Recenzie>> GetByCarteAsync(int idCarte)
            => await _context.Recenzii
                .AsNoTracking()
                .Include(r => r.Utilizator)
                .Include(r => r.Carte)
                .Where(r => r.Id_Carte == idCarte)
                .OrderByDescending(r => r.Data_Postare)
                .ToListAsync();

        public async Task<IEnumerable<Recenzie>> GetByUtilizatorAsync(int idUtilizator)
            => await _context.Recenzii
                .AsNoTracking()
                .Include(r => r.Carte)
                .Where(r => r.Id_Utilizator == idUtilizator)
                .OrderByDescending(r => r.Data_Postare)
                .ToListAsync();

        public async Task<bool> AreRecenzieAsync(int idUtilizator, int idCarte)
            => await _context.Recenzii
                .AnyAsync(r => r.Id_Utilizator == idUtilizator && r.Id_Carte == idCarte);

        public async Task<double> GetNotaMedieAsync(int idCarte)
        {
            var note = await _context.Recenzii
                .Where(r => r.Id_Carte == idCarte)
                .Select(r => r.Nota)
                .ToListAsync();

            return note.Any() ? note.Average() : 0;
        }

        public async Task<Recenzie?> GetWithDetailsAsync(int id)
            => await _context.Recenzii
                .AsNoTracking()
                .Include(r => r.Utilizator)
                .Include(r => r.Carte)
                .FirstOrDefaultAsync(r => r.Id_Recenzie == id);

        public new async Task<IEnumerable<Recenzie>> GetAllAsync()
            => await _context.Recenzii
                .AsNoTracking()
                .Include(r => r.Utilizator)
                .Include(r => r.Carte)
                .OrderByDescending(r => r.Data_Postare)
                .ToListAsync();
    }
}