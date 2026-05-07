using Biblioteca.Models;

namespace Biblioteca.Repositories.Interfaces
{
    public interface IUtilizatorRepository : IBaseRepository<Utilizator>
    {
        Task<Utilizator?> GetWithDetailsAsync(int id);
    }
}