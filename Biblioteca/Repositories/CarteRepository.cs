using Biblioteca.Models;
using Microsoft.EntityFrameworkCore;
using static Biblioteca.Models.Carte;

namespace Biblioteca.Repositories
{
    public class CarteRepository : Repository<Carte>, ICarteRepository
    {
        public CarteRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Carte>> GetByAutorAsync(int idAutor)
            => await _context.Carti
                .Include(c => c.Autor)
                .Include(c => c.Categorie)
                .Where(c => c.Id_Autor == idAutor)
                .ToListAsync();

        public async Task<IEnumerable<Carte>> GetByCategorieAsync(int idCategorie)
            => await _context.Carti
                .Include(c => c.Autor)
                .Include(c => c.Categorie)
                .Where(c => c.Id_Categorie == idCategorie)
                .ToListAsync();

        public async Task<IEnumerable<Carte>> GetDisponibileAsync()
            => await _context.Carti
                .Include(c => c.Autor)
                .Include(c => c.Categorie)
                .Where(c => c.Status_Disponibilitate == StatusDisponibilitate.Disponibil)
                .ToListAsync();

        public async Task<IEnumerable<Carte>> SearchAsync(string termen)
        {
            var t = termen.ToLower();
            return await _context.Carti
                .Include(c => c.Autor)
                .Include(c => c.Categorie)
                .Where(c => c.Titlu.ToLower().Contains(t)
                         || (c.Autor != null && c.Autor.Nume_Autor.ToLower().Contains(t))
                         || (c.ISBN != null && c.ISBN.Contains(t)))
                .ToListAsync();
        }

        public async Task<Carte?> GetWithDetailsAsync(int id)
            => await _context.Carti
                .Include(c => c.Autor)
                .Include(c => c.Categorie)
                .Include(c => c.Recenzii!)
                    .ThenInclude(r => r.Utilizator)
                .FirstOrDefaultAsync(c => c.Id_Carte == id);

        // Top carti dupa numar de recenzii
        public async Task<IEnumerable<Carte>> GetTopByRecenziiAsync(int numar)
            => await _context.Carti
                .Include(c => c.Autor)
                .Include(c => c.Recenzii)
                .Where(c => c.Status_Disponibilitate == StatusDisponibilitate.Disponibil)
                .OrderByDescending(c => c.Recenzii!.Count)
                .Take(numar)
                .ToListAsync();

        // Toti autorii pentru SelectList
        public async Task<IEnumerable<Autor>> GetToiAutoriiAsync()
            => await _context.Autori
                .OrderBy(a => a.Nume_Autor)
                .ToListAsync();

        // Toate categoriile pentru SelectList
        public async Task<IEnumerable<Categorie>> GetToateCategoriiAsync()
            => await _context.Categorii
                .OrderBy(c => c.Nume_Categorie)
                .ToListAsync();

        // Carte cu autor inclus pentru pagina confirmare imprumut
        public async Task<Carte?> GetWithAutorAsync(int id)
            => await _context.Carti
                .Include(c => c.Autor)
                .FirstOrDefaultAsync(c => c.Id_Carte == id);
    }
}
