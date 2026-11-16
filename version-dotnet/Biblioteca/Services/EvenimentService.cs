using Biblioteca.Models;
using Biblioteca.Repositories.Interfaces;
using Biblioteca.Services.Interfaces;
using static Biblioteca.Models.Rezervare_Eveniment;

namespace Biblioteca.Services
{
    public class EvenimentService : IEvenimentService
    {
        private readonly IRepositoryWrapper _repo;

        public EvenimentService(IRepositoryWrapper repo) => _repo = repo;

        public async Task<IEnumerable<Eveniment>> GetToateEvenimenteleAsync()
            => await _repo.Evenimente.GetAllAsync();

        public async Task<IEnumerable<Eveniment>> GetEvenimenteViitoareAsync()
            => await _repo.Evenimente.GetViitoareAsync();

        public async Task<Eveniment?> GetDetaliiAsync(int id)
            => await _repo.Evenimente.GetWithRezervariAsync(id);

        public async Task<(bool succes, string mesaj)> AdaugaAsync(Eveniment eveniment)
        {
            if (eveniment.Data_Ora <= DateTime.UtcNow)
                return (false, "Data evenimentului trebuie să fie în viitor.");
            if (eveniment.Locuri_Disponibile < 1)
                return (false, "Evenimentul trebuie să aibă cel puțin un loc disponibil.");

            eveniment.Locuri_Rezervate = 0;
            await _repo.Evenimente.AddAsync(eveniment);
            return (true, "Evenimentul a fost adăugat.");
        }

        public async Task<(bool succes, string mesaj)> ActualizeazaAsync(Eveniment eveniment)
        {
            var vechi = await _repo.Evenimente.GetByIdAsync(eveniment.Id_Eveniment);
            if (vechi == null) return (false, "Evenimentul nu a fost găsit.");

            if (eveniment.Locuri_Disponibile < vechi.Locuri_Rezervate)
                return (false, $"Nu poți reduce locurile sub {vechi.Locuri_Rezervate} (numărul rezervărilor active).");

            await _repo.Evenimente.UpdateAsync(eveniment);
            return (true, "Evenimentul a fost actualizat.");
        }

        public async Task<(bool succes, string mesaj)> StergeAsync(int id)
        {
            var ev = await _repo.Evenimente.GetWithRezervariAsync(id);
            if (ev == null) return (false, "Evenimentul nu a fost găsit.");

            if (ev.Rezervari != null && ev.Rezervari.Any(r => r.Status_Rezervare == StatusRezervare.Activă))
                return (false, "Evenimentul are rezervări active — nu poate fi șters. Anulează rezervările mai întâi.");

            await _repo.Evenimente.DeleteAsync(id);
            return (true, "Evenimentul a fost șters.");
        }
    }
}