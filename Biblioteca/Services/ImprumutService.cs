using Biblioteca.Models;
using Biblioteca.Repositories.Interfaces;
using Biblioteca.Services.Interfaces;
using static Biblioteca.Models.Carte;
using static Biblioteca.Models.Imprumut;

namespace Biblioteca.Services
{
    public class ImprumutService : IImprumutService
    {
        private readonly IRepositoryWrapper _repo;

        private const decimal TAXA_PE_ZI = 1.00m;
        private const int ZILE_TERMEN_DEFAULT = 14;
        private const int ZILE_PRELUNGIRE = 14;
        private const int MAX_IMPRUMUTURI_ACTIVE = 5;

        public ImprumutService(IRepositoryWrapper repo) => _repo = repo;

        public async Task<IEnumerable<Imprumut>> GetImprumuturiUtilizatorAsync(int idUtilizator)
            => await _repo.Imprumuturi.GetByUtilizatorAsync(idUtilizator);

        public async Task<IEnumerable<Imprumut>> GetToateImprumuturileAsync()
            => await _repo.Imprumuturi.GetActiveAsync();

        public async Task<Imprumut?> GetDetaliiImprumutAsync(int id)
            => await _repo.Imprumuturi.GetWithDetailsAsync(id);

        public async Task<bool> UtilizatorExistaAsync(int idUtilizator)
            => await _repo.Imprumuturi.UtilizatorExistaAsync(idUtilizator);

        public async Task<Carte?> GetCarteSimplaAsync(int idCarte)
            => await _repo.Carti.GetWithAutorAsync(idCarte);

        public async Task<(bool succes, string mesaj)> ImprumutaCarteAsync(int idUtilizator, int idCarte, int zileTermen = ZILE_TERMEN_DEFAULT)
        {
            var carte = await _repo.Carti.GetByIdAsync(idCarte);
            if (carte == null)
                return (false, "Cartea nu a fost găsită.");
            if (carte.Status_Disponibilitate == StatusDisponibilitate.Indisponibil)
                return (false, "Această carte nu este disponibilă momentan.");

            bool areDejaActiv = await _repo.Imprumuturi.AreImprumutActivAsync(idUtilizator, idCarte);
            if (areDejaActiv)
                return (false, "Ai deja această carte împrumutată.");

            var imprumuturiActive = await _repo.Imprumuturi.GetByUtilizatorAsync(idUtilizator);
            int nrActive = imprumuturiActive.Count(i => i.Status == StatusImprumut.Activ || i.Status == StatusImprumut.Prelungit);
            if (nrActive >= MAX_IMPRUMUTURI_ACTIVE)
                return (false, $"Ai atins limita de {MAX_IMPRUMUTURI_ACTIVE} cărți împrumutate simultan. Returnează o carte înainte.");

            var imprumut = new Imprumut
            {
                Id_Utilizator = idUtilizator,
                Id_Carte = idCarte,
                Data_Imprumut = DateTime.UtcNow,
                Data_Scadenta = DateTime.UtcNow.AddDays(zileTermen),
                Status = StatusImprumut.Activ,
                Taxa_Penalizare = 0
            };

            await _repo.Imprumuturi.AddAsync(imprumut);
            return (true, $"Carte împrumutată cu succes! Termen de citire: {imprumut.Data_Scadenta:dd MMM yyyy}.");
        }

        public async Task<(bool succes, string mesaj)> ReturnareCarteAsync(int idImprumut)
        {
            // Citim fara tracking pentru a calcula taxa
            var imprumut = await _repo.Imprumuturi.GetByIdAsync(idImprumut);
            if (imprumut == null) return (false, "Împrumutul nu a fost găsit.");

            if (imprumut.Status == StatusImprumut.Returnat)
                return (false, "Această carte a fost deja returnată.");

            var dataReturnare = DateTime.UtcNow;
            imprumut.Data_Returnare = dataReturnare;
            var taxaPenalizare = CalculeazaTaxaPenalizareAsync(imprumut);

            // Folosim ExecuteUpdateAsync - update direct SQL, fara niciun risc de tracking EF Core
            await _repo.Imprumuturi.ReturnareDirectaAsync(idImprumut, dataReturnare, taxaPenalizare);

            string mesajTaxa = taxaPenalizare > 0
                ? $" Taxă de penalizare: {taxaPenalizare:F2} RON."
                : "";
            return (true, $"Cartea a fost returnată.{mesajTaxa}");
        }

        public async Task<(bool succes, string mesaj)> PrelungesteImprumutAsync(int idImprumut, int zileExtra = ZILE_PRELUNGIRE)
        {
            var imprumut = await _repo.Imprumuturi.GetByIdAsync(idImprumut);
            if (imprumut == null) return (false, "Împrumutul nu a fost găsit.");

            if (imprumut.Status == StatusImprumut.Returnat)
                return (false, "Nu poți prelungi un împrumut deja returnat.");

            if (DateTime.UtcNow > imprumut.Data_Scadenta)
                return (false, "Nu poți prelungi un împrumut cu termenul depășit. Contactează biblioteca.");

            if (imprumut.Status == StatusImprumut.Prelungit)
                return (false, "Împrumutul a fost deja prelungit. Nu mai sunt permise prelungiri suplimentare.");

            imprumut.Data_Scadenta = imprumut.Data_Scadenta.AddDays(zileExtra);
            imprumut.Status = StatusImprumut.Prelungit;

            await _repo.Imprumuturi.UpdateAsync(imprumut);
            return (true, $"Termen prelungit cu {zileExtra} zile. Noul termen: {imprumut.Data_Scadenta:dd MMM yyyy}.");
        }

        public async Task<bool> AreImprumutActivAsync(int idUtilizator, int idCarte)
            => await _repo.Imprumuturi.AreImprumutActivAsync(idUtilizator, idCarte);

        public decimal CalculeazaTaxaPenalizareAsync(Imprumut imprumut)
        {
            if (imprumut.Data_Returnare == null || imprumut.Data_Returnare <= imprumut.Data_Scadenta)
                return 0;

            int zileIntarziere = (int)(imprumut.Data_Returnare.Value - imprumut.Data_Scadenta).TotalDays;
            return zileIntarziere * TAXA_PE_ZI;
        }
    }
}