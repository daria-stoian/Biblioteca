using Biblioteca.Models;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repositories
{
    public class ListaDorinteRepository : IListaDorinteRepository
    {
        private readonly ApplicationDbContext _context;
        public ListaDorinteRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<Lista_Dorinte>> GetByUtilizatorAsync(int idUtilizator)
            => await _context.Lista_Dorinte.Include(ld => ld.Carte!)
                    .ThenInclude(c => c.Autor)
                .Where(ld => ld.Id_Utilizator == idUtilizator)
                .OrderByDescending(ld => ld.Data_Adaugare)
                .ToListAsync();

        public async Task<bool> ExistaInListaAsync(int idUtilizator, int idCarte)
            => await _context.Lista_Dorinte.AnyAsync(ld => ld.Id_Utilizator == idUtilizator && ld.Id_Carte == idCarte);

        public async Task AdaugaAsync(Lista_Dorinte item)
        {
            await _context.Lista_Dorinte.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task StergeAsync(int idUtilizator, int idCarte)
        {
            var item = await _context.Lista_Dorinte.FirstOrDefaultAsync(ld => ld.Id_Utilizator == idUtilizator && ld.Id_Carte == idCarte);
            if (item != null)
            {
                _context.Lista_Dorinte.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
