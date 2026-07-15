namespace Biblioteca.Services.Interfaces
{
    public interface IAccountService
    {
        Task<(bool succes, string mesaj)> LoginAsync(string email, string parola, bool tineMinte);
        Task<(bool succes, IEnumerable<string> erori)> RegisterAsync(
            string numeComplet, string email, string parola, string? telefon, string? adresa);
        Task LogoutAsync();
    }
}