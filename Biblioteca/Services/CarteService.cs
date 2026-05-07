using Biblioteca.Models;
using Biblioteca.Repositories.Interfaces;
using Biblioteca.Services.Interfaces;
using static Biblioteca.Models.Carte;

namespace Biblioteca.Services
{
    public class CarteService : ICarteService
    {
        private readonly IRepositoryWrapper _repo;
        private readonly IWebHostEnvironment _env;

        public CarteService(IRepositoryWrapper repo, IWebHostEnvironment env)
        {
            _repo = repo;
            _env = env;
        }

        public async Task<IEnumerable<Carte>> GetToateCartileAsync()
            => await _repo.Carti.GetAllAsync();

        public async Task<IEnumerable<Carte>> GetCartileDisponibileAsync()
            => await _repo.Carti.GetDisponibileAsync();

        public async Task<IEnumerable<Carte>> GetByCategorieAsync(int idCategorie)
            => await _repo.Carti.GetByCategorieAsync(idCategorie);

        public async Task<IEnumerable<Carte>> CautaCartiAsync(string termen)
        {
            if (string.IsNullOrWhiteSpace(termen))
                return await _repo.Carti.GetDisponibileAsync();
            return await _repo.Carti.SearchAsync(termen);
        }

        public async Task<Carte?> GetDetaliiCarteAsync(int id)
            => await _repo.Carti.GetWithDetailsAsync(id);

        public async Task<IEnumerable<Carte>> GetTopCartiAsync(int numar)
            => await _repo.Carti.GetTopByRecenziiAsync(numar);

        public async Task<IEnumerable<Autor>> GetAutoriAsync()
            => await _repo.Carti.GetToiAutoriiAsync();

        public async Task<IEnumerable<Categorie>> GetCategoriiAsync()
            => await _repo.Carti.GetToateCategoriiAsync();

        public async Task<(bool succes, string mesaj)> AdaugaCarteAsync(Carte carte, IFormFile? imagine, IFormFile? pdf)
        {
            if (!string.IsNullOrEmpty(carte.ISBN))
            {
                var toateCartile = await _repo.Carti.GetAllAsync();
                if (toateCartile.Any(c => c.ISBN == carte.ISBN))
                    return (false, "Există deja o carte cu acest ISBN în catalog.");
            }

            if (imagine != null && imagine.Length > 0)
            {
                var rezultat = await SalveazaImagineAsync(imagine);
                if (!rezultat.succes) return (false, rezultat.mesaj);
                carte.Imagine_Coperta = rezultat.numeFisier;
            }

            if (pdf != null && pdf.Length > 0)
            {
                var rezultat = await SalveazaPDFAsync(pdf);
                if (!rezultat.succes) return (false, rezultat.mesaj);
                carte.Fisier_PDF = rezultat.numeFisier;
            }

            carte.Status_Disponibilitate = StatusDisponibilitate.Disponibil;
            await _repo.Carti.AddAsync(carte);
            return (true, "Cartea a fost adăugată cu succes.");
        }

        public async Task<(bool succes, string mesaj)> ActualizeazaCarteAsync(Carte carte, IFormFile? imagine, IFormFile? pdf)
        {
            var carteVeche = await _repo.Carti.GetByIdAsync(carte.Id_Carte);
            if (carteVeche == null) return (false, "Cartea nu a fost găsită.");

            if (!string.IsNullOrEmpty(carte.ISBN))
            {
                var toateCartile = await _repo.Carti.GetAllAsync();
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

            if (pdf != null && pdf.Length > 0)
            {
                if (!string.IsNullOrEmpty(carteVeche.Fisier_PDF))
                    StergePDF(carteVeche.Fisier_PDF);

                var rezultat = await SalveazaPDFAsync(pdf);
                if (!rezultat.succes) return (false, rezultat.mesaj);
                carte.Fisier_PDF = rezultat.numeFisier;
            }
            else
            {
                carte.Fisier_PDF = carteVeche.Fisier_PDF;
            }

            await _repo.Carti.UpdateAsync(carte);
            return (true, "Cartea a fost actualizată cu succes.");
        }

        public async Task<(bool succes, string mesaj)> StergeCarteAsync(int id)
        {
            var imprumuturiActive = await _repo.Imprumuturi.GetActiveAsync();
            if (imprumuturiActive.Any(i => i.Id_Carte == id))
                return (false, "Cartea nu poate fi ștearsă — are împrumuturi active. Marchează-o ca Indisponibilă.");

            var carte = await _repo.Carti.GetByIdAsync(id);
            if (carte == null) return (false, "Cartea nu a fost găsită.");

            if (!string.IsNullOrEmpty(carte.Imagine_Coperta))
                StergeImagine(carte.Imagine_Coperta);

            if (!string.IsNullOrEmpty(carte.Fisier_PDF))
                StergePDF(carte.Fisier_PDF);

            await _repo.Carti.DeleteAsync(id);
            return (true, "Cartea a fost ștearsă.");
        }

        public async Task<(bool succes, string mesaj)> SetStatusAsync(int id, StatusDisponibilitate status)
        {
            var carte = await _repo.Carti.GetByIdAsync(id);
            if (carte == null) return (false, "Cartea nu a fost găsită.");

            carte.Status_Disponibilitate = status;
            await _repo.Carti.UpdateAsync(carte);

            string mesaj = status == StatusDisponibilitate.Disponibil
                ? "Cartea este acum vizibilă în catalog."
                : "Cartea a fost scoasă din catalog.";
            return (true, mesaj);
        }

        // Verifica daca utilizatorul are imprumut activ si returneaza calea catre PDF
        public async Task<(bool areAcces, string? caleFisier, string? numeFisier)> GetPDFAsync(int idCarte, int idUtilizator)
        {
            var carte = await _repo.Carti.GetByIdAsync(idCarte);
            if (carte == null || string.IsNullOrEmpty(carte.Fisier_PDF))
                return (false, null, null);

            // Verificam daca utilizatorul are imprumut activ pentru aceasta carte
            bool areImprumutActiv = await _repo.Imprumuturi.AreImprumutActivAsync(idUtilizator, idCarte);
            if (!areImprumutActiv)
                return (false, null, null);

            string caleFisier = Path.Combine(_env.WebRootPath, "pdf", carte.Fisier_PDF);
            if (!File.Exists(caleFisier))
                return (false, null, null);

            return (true, caleFisier, carte.Titlu + ".pdf");
        }

        // --- Metode private helper ---

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

        private async Task<(bool succes, string mesaj, string numeFisier)> SalveazaPDFAsync(IFormFile pdf)
        {
            if (!pdf.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                return (false, "Doar fișiere PDF sunt acceptate.", "");

            if (pdf.Length > 50 * 1024 * 1024)
                return (false, "Fișierul PDF depășește limita de 50MB.", "");

            string folder = Path.Combine(_env.WebRootPath, "pdf");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            string numeFisier = Guid.NewGuid().ToString() + ".pdf";
            string cale = Path.Combine(folder, numeFisier);

            using var stream = new FileStream(cale, FileMode.Create);
            await pdf.CopyToAsync(stream);

            return (true, "", numeFisier);
        }

        private void StergePDF(string numeFisier)
        {
            string cale = Path.Combine(_env.WebRootPath, "pdf", numeFisier);
            if (File.Exists(cale)) File.Delete(cale);
        }
    }
}