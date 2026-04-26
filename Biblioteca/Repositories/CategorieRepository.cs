using Biblioteca.Models;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repositories
{
    public class CategorieRepository : Repository<Categorie>, ICategorieRepository
    {
        public CategorieRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Categorie?> GetWithCartiAsync(int id)
            => await _context.Categorii
                .Include(c => c.Carti!)
                    .ThenInclude(carte => carte.Autor)
                .FirstOrDefaultAsync(c => c.Id_Categorie == id);

        public async Task<bool> AreCartiAsync(int id)
            => await _context.Carti.AnyAsync(c => c.Id_Categorie == id);
    }
}
