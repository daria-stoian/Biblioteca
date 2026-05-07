using Biblioteca.Models;
using Biblioteca.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Biblioteca.Models.Rezervare_Eveniment;

namespace Biblioteca.Repositories
{
    public class RezervareEvenimentRepository : BaseRepository<Rezervare_Eveniment>, IRezervareEvenimentRepository
    {
        public RezervareEvenimentRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Rezervare_Eveniment>> GetByUtilizatorAsync(int idUtilizator)
            => await _context.Rezervari_Evenimente
                .Include(r => r.Eveniment)
                .Where(r => r.Id_Utilizator == idUtilizator)
                .OrderByDescending(r => r.Data_Rzervare)
                .ToListAsync();

        public async Task<IEnumerable<Rezervare_Eveniment>> GetByEvenimentAsync(int idEveniment)
            => await _context.Rezervari_Evenimente
                .Include(r => r.Utilizator)
                .Where(r => r.Id_Eveniment == idEveniment)
                .ToListAsync();

        public async Task<bool> AreRezervareActivaAsync(int idUtilizator, int idEveniment)
            => await _context.Rezervari_Evenimente
                .AnyAsync(r => r.Id_Utilizator == idUtilizator
                            && r.Id_Eveniment == idEveniment
                            && r.Status_Rezervare == StatusRezervare.Activă);

        public async Task<Rezervare_Eveniment?> GetWithDetailsAsync(int id)
            => await _context.Rezervari_Evenimente
                .Include(r => r.Utilizator)
                .Include(r => r.Eveniment)
                .FirstOrDefaultAsync(r => r.Id_Rezervare == id);
    }
}
