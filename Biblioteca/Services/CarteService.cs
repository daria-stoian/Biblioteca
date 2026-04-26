using Biblioteca.Models;
using Biblioteca.Repositories;
using static Biblioteca.Models.Carte;

namespace Biblioteca.Services
{
    public class CarteService : ICarteService
    {
        private readonly ICarteRepository _carteRepo;
        private readonly IImprumutRepository _imprumutRepo;
        private readonly IWebHostEnvironment _env;

        public CarteService(ICarteRepository carteRepo, IImprumutRepository imprumutRepo, IWebHostEnvironment env)
        {
            _carteRepo = carteRepo;
            _imprumutRepo = imprumutRepo;
            _env = env;
        }

        public async Task<IEnumerable<Carte>> GetToateCartileAsync()
            => await _carteRepo.GetAllAsync();

        public async Task<IEnumerable<Carte>> GetCartileDisponibileAsync()
            => await _carteRepo.GetDisponibileAsync();

        public async Task<IEnumerable<Carte>> GetByCategorieAsync(int idCategorie)
            => await _carteRepo.GetByCategorieAsync(idCategorie);

        public async Task<IEnumerable<Carte>> CautaCartiAsync(string termen)
        {
            if (string.IsNullOrWhiteSpace(termen))
                return await _carteRepo.GetDisponibileAsync();
            return await _carteRepo.SearchAsync(termen);
        }

        public async Task<Carte?> GetDetaliiCarteAsync(int id)
            => await _carteRepo.GetWithDetailsAsync(id);

        // Top carti
        public async Task<IEnumerable<Carte>> GetTopCartiAsync(int numar)
            => await _carteRepo.GetTopByRecenziiAsync(numar);

        // Lista autori pentru SelectList
        public async Task<IEnumerable<Autor>> GetAutoriAsync()
            => await _carteRepo.GetToiAutoriiAsync();

        // Lista categorii pentru SelectList
        public async Task<IEnumerable<Categorie>> GetCategoriiAsync()
            => await _carteRepo.GetToateCategoriiAsync();

        public async Task<(bool succes, string mesaj)> AdaugaCarteAsync(Carte carte, IFormFile? imagine)
        {
            if (!string.IsNullOrEmpty(carte.ISBN))
            {
                var toateCartile = await _carteRepo.GetAllAsync();
                if (toateCartile.Any(c => c.ISBN == carte.ISBN))
                    return (false, "Există deja o carte cu acest ISBN în catalog.");
            }

            if (imagine != null && imagine.Length > 0)
            {
                var rezultat = await SalveazaImagineAsync(imagine);
                if (!rezultat.succes) return (false, rezultat.mesaj);
                carte.Imagine_Coperta = rezultat.numeFisier;
            }

            carte.Status_Disponibilitate = StatusDisponibilitate.Disponibil;
            await _carteRepo.AddAsync(carte);
            return (true, "Cartea a fost adăugată cu succes.");
        }

        public async Task<(bool succes, string mesaj)> ActualizeazaCarteAsync(Carte carte, IFormFile? imagine)
        {
            var carteVeche = await _carteRepo.GetByIdAsync(carte.Id_Carte);
            if (carteVeche == null) return (false, "Cartea nu a fost găsită.");

            if (!string.IsNullOrEmpty(carte.ISBN))
            {
                var toateCartile = await _carteRepo.GetAllAsync();
                if (toateCartile.Any(c => c.ISBN == carte.ISBN && c.Id_Carte != carte.Id_Carte))
                    return (false, "Există deja o altă carte cu acest ISBN.");
            }

            if (imagine != null && imagine.Length > 0)
            {
                if (!string.IsNullOrEmpty(carteVeche.Imagine_Coperta))
                    StergeImagine(carteVeche.Imagine_Coperta);

                var rezultat = await SalveazaImagineAsync(imagine);
                if (!rezultat.succes) return (false, rezultat.mesaj);
                carte.Imagine_Coperta = rezultat.numeFisier;
            }
            else
            {
                carte.Imagine_Coperta = carteVeche.Imagine_Coperta;
            }

            await _carteRepo.UpdateAsync(carte);
            return (true, "Cartea a fost actualizată cu succes.");
        }

        public async Task<(bool succes, string mesaj)> StergeCarteAsync(int id)
        {
            bool areImprumuturiActive = await _imprumutRepo.GetActiveAsync()
                .ContinueWith(t => t.Result.Any(i => i.Id_Carte == id));

            if (areImprumuturiActive)
                return (false, "Cartea nu poate fi ștearsă — are împrumuturi active. Marchează-o ca Indisponibilă.");

            var carte = await _carteRepo.GetByIdAsync(id);
            if (carte == null) return (false, "Cartea nu a fost găsită.");

            if (!string.IsNullOrEmpty(carte.Imagine_Coperta))
                StergeImagine(carte.Imagine_Coperta);

            await _carteRepo.DeleteAsync(id);
            return (true, "Cartea a fost ștearsă.");
        }

        public async Task<(bool succes, string mesaj)> SetStatusAsync(int id, StatusDisponibilitate status)
        {
            var carte = await _carteRepo.GetByIdAsync(id);
            if (carte == null) return (false, "Cartea nu a fost găsită.");

            carte.Status_Disponibilitate = status;
            await _carteRepo.UpdateAsync(carte);

            string mesaj = status == StatusDisponibilitate.Disponibil
                ? "Cartea este acum vizibilă în catalog."
                : "Cartea a fost scoasă din catalog.";
            return (true, mesaj);
        }

        private async Task<(bool succes, string mesaj, string numeFisier)> SalveazaImagineAsync(IFormFile imagine)
        {
            var extensiiPermise = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extensie = Path.GetExtension(imagine.FileName).ToLower();

            if (!extensiiPermise.Contains(extensie))
                return (false, "Formatul imaginii nu este acceptat. Folosește JPG, PNG sau WEBP.", "");

            if (imagine.Length > 5 * 1024 * 1024)
                return (false, "Imaginea depășește limita de 5MB.", "");

            string folder = Path.Combine(_env.WebRootPath, "imagini");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            string numeFisier = Guid.NewGuid().ToString() + extensie;
            string cale = Path.Combine(folder, numeFisier);

            using var stream = new FileStream(cale, FileMode.Create);
            await imagine.CopyToAsync(stream);

            return (true, "", numeFisier);
        }

        private void StergeImagine(string numeFisier)
        {
            string cale = Path.Combine(_env.WebRootPath, "imagini", numeFisier);
            if (File.Exists(cale)) File.Delete(cale);
        }
    }
}
