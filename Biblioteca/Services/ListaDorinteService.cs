using Biblioteca.Models;
using Biblioteca.Repositories.Interfaces;
using Biblioteca.Services.Interfaces;
using System.Collections;

namespace Biblioteca.Services
{
    public class ListaDorinteService : IListaDorinteService
    {
        private readonly IRepositoryWrapper _repo;

        public ListaDorinteService(IRepositoryWrapper repo) => _repo = repo;

        public async Task<IEnumerable<Lista_Dorinte>> GetListaUtilizatorAsync(int idUtilizator)
            => await _repo.ListaDorinte.GetByUtilizatorAsync(idUtilizator);

        public async Task<(bool succes, string mesaj)> AdaugaInListaAsync(int idUtilizator, int idCarte)
        {
            var carte = await _repo.Carti.GetByIdAsync(idCarte);
            if (carte == null) return (false, "Cartea nu a fost găsită.");

            if (await _repo.ListaDorinte.ExistaInListaAsync(idUtilizator, idCarte))
                return (false, "Cartea este deja în lista ta de dorințe.");

            await _repo.ListaDorinte.AdaugaAsync(new Lista_Dorinte
            {
                Id_Utilizator = idUtilizator,
                Id_Carte = idCarte,
                Data_Adaugare = DateTime.UtcNow
            });
            return (true, $"„{carte.Titlu}” a fost adăugată în lista de dorințe.");
        }

        public async Task<(bool succes, string mesaj)> StergedinListaAsync(int idUtilizator, int idCarte)
        {
            if (!await _repo.ListaDorinte.ExistaInListaAsync(idUtilizator, idCarte))
                return (false, "Cartea nu se află în lista ta.");

            await _repo.ListaDorinte.StergeAsync(idUtilizator, idCarte);
            return (true, "Cartea a fost scoasă din lista de dorințe.");
        }
    }
}