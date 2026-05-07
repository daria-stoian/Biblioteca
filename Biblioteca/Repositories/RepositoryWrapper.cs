using Biblioteca.Models;
using Biblioteca.Repositories.Interfaces;

namespace Biblioteca.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly ApplicationDbContext _context;

        private ICarteRepository? _carti;
        private IAutorRepository? _autori;
        private ICategorieRepository? _categorii;
        private IImprumutRepository? _imprumuturi;
        private IRecenzieRepository? _recenzii;
        private IEvenimentRepository? _evenimente;
        private IRezervareEvenimentRepository? _rezervariEvenimente;
        private IListaDorinteRepository? _listaDorinte;
        private IContactRepository? _contacte;
        private IUtilizatorRepository? _utilizatori;

        public RepositoryWrapper(ApplicationDbContext context)
        {
            _context = context;
        }

        public ICarteRepository Carti
        {
            get { 
                if (_carti == null) 
                    _carti = new CarteRepository(_context); 
                return _carti; 
            }
        }

        public IAutorRepository Autori
        {
            get { 
                if (_autori == null) 
                    _autori = new AutorRepository(_context); 
                return _autori; 
            }
        }

        public ICategorieRepository Categorii
        {
            get { 
                if (_categorii == null) 
                    _categorii = new CategorieRepository(_context); 
                return _categorii; 
            }
        }

        public IImprumutRepository Imprumuturi
        {
            get { 
                if (_imprumuturi == null) 
                    _imprumuturi = new ImprumutRepository(_context); 
                return _imprumuturi; 
            }
        }

        public IRecenzieRepository Recenzii
        {
            get { 
                if (_recenzii == null) 
                    _recenzii = new RecenzieRepository(_context); 
                return _recenzii; 
            }
        }

        public IEvenimentRepository Evenimente
        {
            get { 
                if (_evenimente == null) 
                    _evenimente = new EvenimentRepository(_context); 
                return _evenimente; 
            }
        }

        public IRezervareEvenimentRepository RezervariEvenimente
        {
            get { 
                if (_rezervariEvenimente == null) 
                    _rezervariEvenimente = new RezervareEvenimentRepository(_context); 
                return _rezervariEvenimente; 
            }
        }

        public IListaDorinteRepository ListaDorinte
        {
            get { 
                if (_listaDorinte == null) 
                    _listaDorinte = new ListaDorinteRepository(_context); 
                return _listaDorinte; 
            }
        }

        public IContactRepository Contacte
        {
            get { 
                if (_contacte == null) 
                    _contacte = new ContactRepository(_context); 
                return _contacte; 
            }
        }

        public IUtilizatorRepository Utilizatori
        {
            get { 
                if (_utilizatori == null) 
                    _utilizatori = new UtilizatorRepository(_context); 
                return _utilizatori; 
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}