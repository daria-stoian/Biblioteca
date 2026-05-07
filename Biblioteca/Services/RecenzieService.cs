using Biblioteca.Models;
using Biblioteca.Repositories.Interfaces;
using Biblioteca.Services.Interfaces;

namespace Biblioteca.Services
{
    public class RecenzieService : IRecenzieService
    {
        private readonly IRepositoryWrapper _repo;

        public RecenzieService(IRepositoryWrapper repo) => _repo = repo;

        public async Task<IEnumerable<Recenzie>> GetToateRecenziile()
            => await _repo.Recenzii.GetAllAsync();

        public async Task<IEnumerable<Recenzie>> GetRecenziiCarteAsync(int idCarte)
            => await _repo.Recenzii.GetByCarteAsync(idCarte);

        public async Task<IEnumerable<Recenzie>> GetRecenziiUtilizatorAsync(int idUtilizator)
            => await _repo.Recenzii.GetByUtilizatorAsync(idUtilizator);

        public async Task<Recenzie?> GetDetaliiRecenzieAsync(int id)
            => await _repo.Recenzii.GetWithDetailsAsync(id);

        public async Task<double> GetNotaMedieCarteAsync(int idCarte)
            => await _repo.Recenzii.GetNotaMedieAsync(idCarte);

        public async Task<IEnumerable<Carte>> GetCartileDisponibileAsync()
            => await _repo.Carti.GetAllAsync();

        public async Task<(bool succes, string mesaj)> AdaugaRecenzieAsync(Recenzie recenzie)
        {
            if (recenzie.Nota < 1 || recenzie.Nota > 5)
                return (false, "Nota trebuie să fie între 1 și 5.");

            bool areDejaRecenzie = await _repo.Recenzii.AreRecenzieAsync(recenzie.Id_Utilizator, recenzie.Id_Carte);
            if (areDejaRecenzie)
                return (false, "Ai publicat deja o recenzie pentru această carte.");

            recenzie.Data_Postare = DateTime.UtcNow;
            await _repo.Recenzii.AddAsync(recenzie);
            return (true, "Recenzia ta a fost publicată. Mulțumim!");
        }

        public async Task<(bool succes, string mesaj)> StergeRecenzieAsync(int idRecenzie, int idUtilizatorCurent)
        {
            var recenzie = await _repo.Recenzii.GetByIdAsync(idRecenzie);
            if (recenzie == null) return (false, "Recenzia nu a fost găsită.");

            if (recenzie.Id_Utilizator != idUtilizatorCurent)
                return (false, "Nu poți șterge recenzia altcuiva.");

            await _repo.Recenzii.DeleteAsync(idRecenzie);
            return (true, "Recenzia a fost ștearsă.");
        }
    }
}