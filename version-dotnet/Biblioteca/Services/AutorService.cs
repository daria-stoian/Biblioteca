using Biblioteca.Models;
using Biblioteca.Repositories.Interfaces;
using Biblioteca.Services.Interfaces;

namespace Biblioteca.Services
{
    public class AutorService : IAutorService
    {
        private readonly IRepositoryWrapper _repo;

        public AutorService(IRepositoryWrapper repo) => _repo = repo;

        public async Task<IEnumerable<Autor>> GetToiAutoriiAsync()
            => await _repo.Autori.GetAllAsync();

        public async Task<IEnumerable<Autor>> CautaAutoriAsync(string termen)
            => string.IsNullOrWhiteSpace(termen)
                ? await _repo.Autori.GetAllAsync()
                : await _repo.Autori.SearchAsync(termen);

        public async Task<Autor?> GetDetaliiAsync(int id)
            => await _repo.Autori.GetWithCartiAsync(id);

        public async Task<(bool succes, string mesaj)> AdaugaAsync(Autor autor)
        {
            var toti = await _repo.Autori.GetAllAsync();
            if (toti.Any(a => a.Nume_Autor.ToLower() == autor.Nume_Autor.ToLower()))
                return (false, "Există deja un autor cu acest nume.");

            await _repo.Autori.AddAsync(autor);
            return (true, "Autorul a fost adăugat.");
        }

        public async Task<(bool succes, string mesaj)> ActualizeazaAsync(Autor autor)
        {
            if (!await _repo.Autori.ExistsAsync(autor.Id_Autor))
                return (false, "Autorul nu a fost găsit.");

            await _repo.Autori.UpdateAsync(autor);
            return (true, "Autorul a fost actualizat.");
        }

        public async Task<(bool succes, string mesaj)> StergeAsync(int id)
        {
            if (await _repo.Autori.AreCartiAsync(id))
                return (false, "Autorul nu poate fi șters — are cărți în catalog.");

            await _repo.Autori.DeleteAsync(id);
            return (true, "Autorul a fost șters.");
        }
    }
}