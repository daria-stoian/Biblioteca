using Biblioteca.Models;
using Biblioteca.Repositories;

namespace Biblioteca.Services
{
    public class CategorieService : ICategorieService
    {
        private readonly ICategorieRepository _repo;
        public CategorieService(ICategorieRepository repo) => _repo = repo;

        public async Task<IEnumerable<Categorie>> GetToateCategoriile()
            => await _repo.GetAllAsync();

        public async Task<Categorie?> GetDetaliiAsync(int id)
            => await _repo.GetWithCartiAsync(id);

        public async Task<(bool succes, string mesaj)> AdaugaAsync(Categorie categorie)
        {
            var toate = await _repo.GetAllAsync();
            if (toate.Any(c => c.Nume_Categorie.ToLower() == categorie.Nume_Categorie.ToLower()))
                return (false, "Există deja o categorie cu acest nume.");

            await _repo.AddAsync(categorie);
            return (true, "Categoria a fost adăugată.");
        }

        public async Task<(bool succes, string mesaj)> ActualizeazaAsync(Categorie categorie)
        {
            if (!await _repo.ExistsAsync(categorie.Id_Categorie))
                return (false, "Categoria nu a fost găsită.");

            await _repo.UpdateAsync(categorie);
            return (true, "Categoria a fost actualizată.");
        }

        public async Task<(bool succes, string mesaj)> StergeAsync(int id)
        {
            // Nu stergem o categorie daca are carti asociate
            if (await _repo.AreCartiAsync(id))
                return (false, "Categoria nu poate fi ștearsă — are cărți asociate. Mută cărțile mai întâi.");

            await _repo.DeleteAsync(id);
            return (true, "Categoria a fost ștearsă.");
        }
    }
}
