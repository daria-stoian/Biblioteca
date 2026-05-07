using Biblioteca.Models;
using Biblioteca.Repositories.Interfaces;
using Biblioteca.Services.Interfaces;
using static Biblioteca.Models.Rezervare_Eveniment;

namespace Biblioteca.Services
{
    public class RezervareEvenimentService : IRezervareEvenimentService
    {
        private readonly IRepositoryWrapper _repo;

        public RezervareEvenimentService(IRepositoryWrapper repo) => _repo = repo;

        public async Task<IEnumerable<Rezervare_Eveniment>> GetRezervariUtilizatorAsync(int idUtilizator)
            => await _repo.RezervariEvenimente.GetByUtilizatorAsync(idUtilizator);

        public async Task<IEnumerable<Rezervare_Eveniment>> GetRezervariEvenimentAsync(int idEveniment)
            => await _repo.RezervariEvenimente.GetByEvenimentAsync(idEveniment);

        public async Task<(bool succes, string mesaj)> RezervaloculAsync(int idUtilizator, int idEveniment)
        {
            var eveniment = await _repo.Evenimente.GetByIdAsync(idEveniment);
            if (eveniment == null) return (false, "Evenimentul nu a fost găsit.");

            if (eveniment.Data_Ora <= DateTime.UtcNow)
                return (false, "Nu mai poți rezerva un loc la un eveniment trecut.");

            if (eveniment.EstePlin)
                return (false, "Ne pare rău, nu mai sunt locuri disponibile.");

            if (await _repo.RezervariEvenimente.AreRezervareActivaAsync(idUtilizator, idEveniment))
                return (false, "Ai deja o rezervare activă la acest eveniment.");

            var rezervare = new Rezervare_Eveniment
            {
                Id_Utilizator = idUtilizator,
                Id_Eveniment = idEveniment,
                Data_Rzervare = DateTime.UtcNow,
                Status_Rezervare = StatusRezervare.Activă
            };

            eveniment.Locuri_Rezervate++;
            await _repo.Evenimente.UpdateAsync(eveniment);
            await _repo.RezervariEvenimente.AddAsync(rezervare);

            return (true, $"Loc rezervat la \"{eveniment.Titlu_Eveniment}\"! Te așteptăm pe {eveniment.Data_Ora:dd MMM yyyy, HH:mm}.");
        }

        public async Task<(bool succes, string mesaj)> AnuleazaRezervareAsync(int idRezervare, int idUtilizatorCurent)
        {
            var rezervare = await _repo.RezervariEvenimente.GetWithDetailsAsync(idRezervare);
            if (rezervare == null) return (false, "Rezervarea nu a fost găsită.");

            if (rezervare.Id_Utilizator != idUtilizatorCurent)
                return (false, "Nu poți anula rezervarea altcuiva.");

            if (rezervare.Status_Rezervare == StatusRezervare.Anulată)
                return (false, "Rezervarea a fost deja anulată.");

            var eveniment = await _repo.Evenimente.GetByIdAsync(rezervare.Id_Eveniment);
            if (eveniment != null && eveniment.Locuri_Rezervate > 0)
            {
                eveniment.Locuri_Rezervate--;
                await _repo.Evenimente.UpdateAsync(eveniment);
            }

            rezervare.Status_Rezervare = StatusRezervare.Anulată;
            await _repo.RezervariEvenimente.UpdateAsync(rezervare);

            return (true, "Rezervarea a fost anulată.");
        }
    }
}