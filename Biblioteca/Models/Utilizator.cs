using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteca.Models
{
    public enum RolUtilizator
    {
        Cititor,
        Bibliotecar,
        Administrator
    }
    public class Utilizator
    {
        [Key]
        public int Id_Utilizator { get; set; }

        [Required]
        public string Nume_Complet { get; set; }
        public RolUtilizator Nume_Rol { get; set; } = RolUtilizator.Cititor;

        [Required]
        public string Email { get; set; }
        public string Parola { get; set; }
        public string? Telefon { get; set; }
        public string? Adresa { get; set; }
        [NotMapped]
        public DateTime Created_At { get; set; } = DateTime.UtcNow;
        
        public ICollection<Imprumut>? Imprumuturi { get; set; }
        public ICollection<Recenzie>? Recenzii { get; set; }
        public ICollection<Rezervare_Eveniment>? RezervariEvenimente { get; set; }
        public ICollection<Lista_Dorinte>? ListaDorinte { get; set; }
        public ICollection<Contact>? MesajeContact { get; set; }
    }
}
