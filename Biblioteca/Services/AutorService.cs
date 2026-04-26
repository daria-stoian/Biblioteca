using Biblioteca.Models;
using Biblioteca.Repositories;

namespace Biblioteca.Services
{
    public class AutorService : IAutorService
    {
        private readonly IAutorRepository _repo;
        public AutorService(IAutorRepository repo) => _repo = repo;

        public async Task<IEnumerable<Autor>> GetToiAutoriiAsync()
            => await _repo.GetAllAsync();

        public async Task<IEnumerable<Autor>> CautaAutoriAsync(string termen)
            => string.IsNullOrWhiteSpace(termen)
                ? await _repo.GetAllAsync()
                : await _repo.SearchAsync(termen);

        public async Task<Autor?> GetDetaliiAsync(int id)
            => await _repo.GetWithCartiAsync(id);

        public async Task<(bool succes, string mesaj)> AdaugaAsync(Autor autor)
        {
            var toti = await _repo.GetAllAsync();
            if (toti.Any(a => a.Nume_Autor.ToLower() == autor.Nume_Autor.ToLower()))
                return (false, "Există deja un autor cu acest nume.");

            await _repo.AddAsync(autor);
            return (true, "Autorul a fost adăugat.");
        }

        public async Task<(bool succes, string mesaj)> ActualizeazaAsync(Autor autor)
        {
            if (!await _repo.ExistsAsync(autor.Id_Autor))
                return (false, "Autorul nu a fost găsit.");

            await _repo.UpdateAsync(autor);
            return (true, "Autorul a fost actualizat.");
        }

        public async Task<(bool succes, string mesaj)> StergeAsync(int id)
        {
            // Nu stergem un autor daca are carti in catalog
            if (await _repo.AreCartiAsync(id))
                return (false, "Autorul nu poate fi șters — are cărți în catalog.");

            await _repo.DeleteAsync(id);
            return (true, "Autorul a fost șters.");
        }
    }
}
