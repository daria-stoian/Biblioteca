using Microsoft.AspNetCore.Identity;

namespace Biblioteca.Models
{
    public class Utilizator : IdentityUser<int>
    {
        public string Nume_Complet { get; set; } = string.Empty;
        public string? Telefon { get; set; }
        public string? Adresa { get; set; }
        public string? Poza_Profil { get; set; }

        public ICollection<Imprumut>? Imprumuturi { get; set; }
        public ICollection<Recenzie>? Recenzii { get; set; }
        public ICollection<Rezervare_Eveniment>? RezervariEvenimente { get; set; }
        public ICollection<Lista_Dorinte>? ListaDorinte { get; set; }
        public ICollection<Contact>? MesajeContact { get; set; }
    }
}