using Biblioteca.Models;
using Biblioteca.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repositories
{
    public class AutorRepository : BaseRepository<Autor>, IAutorRepository
    {
        public AutorRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Autor?> GetWithCartiAsync(int id)
            => await _context.Autori
                .Include(a => a.Carti!)
                    .ThenInclude(c => c.Categorie)
                .FirstOrDefaultAsync(a => a.Id_Autor == id);

        public async Task<IEnumerable<Autor>> SearchAsync(string termen)
        {
            var t = termen.ToLower();
            return await _context.Autori
                .Where(a => a.Nume_Autor.ToLower().Contains(t))
                .ToListAsync();
        }

        public async Task<bool> AreCartiAsync(int id)
            => await _context.Carti.AnyAsync(c => c.Id_Autor == id);
    }
}
