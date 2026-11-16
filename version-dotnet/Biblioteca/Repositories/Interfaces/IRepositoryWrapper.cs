namespace Biblioteca.Repositories.Interfaces
{
    public interface IRepositoryWrapper
    {
        ICarteRepository Carti { get; }
        IAutorRepository Autori { get; }
        ICategorieRepository Categorii { get; }
        IImprumutRepository Imprumuturi { get; }
        IRecenzieRepository Recenzii { get; }
        IEvenimentRepository Evenimente { get; }
        IRezervareEvenimentRepository RezervariEvenimente { get; }
        IListaDorinteRepository ListaDorinte { get; }
        IContactRepository Contacte { get; }
        IUtilizatorRepository Utilizatori { get; }

        void Save();
    }
}