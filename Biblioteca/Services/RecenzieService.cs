using Biblioteca.Models;
using Biblioteca.Repositories;

namespace Biblioteca.Services
{
    public class RecenzieService : IRecenzieService
    {
        private readonly IRecenzieRepository _recenzieRepo;
        private readonly IImprumutRepository _imprumutRepo;
        private readonly ICarteRepository _carteRepo;

        public RecenzieService(IRecenzieRepository recenzieRepo, IImprumutRepository imprumutRepo, ICarteRepository carteRepo)
        {
            _recenzieRepo = recenzieRepo;
            _imprumutRepo = imprumutRepo;
            _carteRepo = carteRepo;
        }

        public async Task<IEnumerable<Recenzie>> GetToateRecenziile()
            => await _recenzieRepo.GetAllAsync();

        public async Task<IEnumerable<Recenzie>> GetRecenziiCarteAsync(int idCarte)
            => await _recenzieRepo.GetByCarteAsync(idCarte);

        public async Task<IEnumerable<Recenzie>> GetRecenziiUtilizatorAsync(int idUtilizator)
            => await _recenzieRepo.GetByUtilizatorAsync(idUtilizator);

        public async Task<Recenzie?> GetDetaliiRecenzieAsync(int id)
            => await _recenzieRepo.GetWithDetailsAsync(id);

        public async Task<double> GetNotaMedieCarteAsync(int idCarte)
            => await _recenzieRepo.GetNotaMedieAsync(idCarte);

        public async Task<IEnumerable<Carte>> GetCartileDisponibileAsync()
            => await _carteRepo.GetDisponibileAsync();

        public async Task<(bool succes, string mesaj)> AdaugaRecenzieAsync(Recenzie recenzie)
        {
            if (recenzie.Nota < 1 || recenzie.Nota > 5)
                return (false, "Nota trebuie să fie între 1 și 5.");

            bool areDejaRecenzie = await _recenzieRepo.AreRecenzieAsync(recenzie.Id_Utilizator, recenzie.Id_Carte);
            if (areDejaRecenzie)
                return (false, "Ai publicat deja o recenzie pentru această carte.");

            recenzie.Data_Postare = DateTime.UtcNow;
            await _recenzieRepo.AddAsync(recenzie);
            return (true, "Recenzia ta a fost publicată. Mulțumim!");
        }

        public async Task<(bool succes, string mesaj)> StergeRecenzieAsync(int idRecenzie, int idUtilizatorCurent)
        {
            var recenzie = await _recenzieRepo.GetByIdAsync(idRecenzie);
            if (recenzie == null) return (false, "Recenzia nu a fost găsită.");

            if (recenzie.Id_Utilizator != idUtilizatorCurent)
                return (false, "Nu poți șterge recenzia altcuiva.");

            await _recenzieRepo.DeleteAsync(idRecenzie);
            return (true, "Recenzia a fost ștearsă.");
        }
    }
}
