using Biblioteca.Models;
using Biblioteca.Repositories.Interfaces;
using Biblioteca.Services.Interfaces;

namespace Biblioteca.Services
{
    public class CategorieService : ICategorieService
    {
        private readonly IRepositoryWrapper _repo;

        public CategorieService(IRepositoryWrapper repo) => _repo = repo;

        public async Task<IEnumerable<Categorie>> GetToateCategoriile()
            => await _repo.Categorii.GetAllAsync();

        public async Task<Categorie?> GetDetaliiAsync(int id)
            => await _repo.Categorii.GetWithCartiAsync(id);

        public async Task<(bool succes, string mesaj)> AdaugaAsync(Categorie categorie)
        {
            var toate = await _repo.Categorii.GetAllAsync();
            if (toate.Any(c => c.Nume_Categorie.ToLower() == categorie.Nume_Categorie.ToLower()))
                return (false, "Există deja o categorie cu acest nume.");

            await _repo.Categorii.AddAsync(categorie);
            return (true, "Categoria a fost adăugată.");
        }

        public async Task<(bool succes, string mesaj)> ActualizeazaAsync(Categorie categorie)
        {
            if (!await _repo.Categorii.ExistsAsync(categorie.Id_Categorie))
                return (false, "Categoria nu a fost găsită.");

            await _repo.Categorii.UpdateAsync(categorie);
            return (true, "Categoria a fost actualizată.");
        }

        public async Task<(bool succes, string mesaj)> StergeAsync(int id)
        {
            if (await _repo.Categorii.AreCartiAsync(id))
                return (false, "Categoria nu poate fi ștearsă — are cărți asociate. Mută cărțile mai întâi.");

            await _repo.Categorii.DeleteAsync(id);
            return (true, "Categoria a fost ștearsă.");
        }
    }
}