using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteca.Models
{
    public class Carte
    {
        public enum StatusDisponibilitate
        {
            Disponibil,
            Indisponibil
        }

        [Key]
        public int Id_Carte { get; set; }

        public int Id_Autor { get; set; }
        [ForeignKey("Id_Autor")]
        public Autor? Autor { get; set; }
        public int Id_Categorie { get; set; }
        [ForeignKey("Id_Categorie")]
        public Categorie? Categorie { get; set; }

        [Required]
        public string Titlu { get; set; }
        public string? ISBN { get; set; }
        public int An_Publicare { get; set; }
        public int Numar_Pagini { get; set; }
        public string? Descriere { get; set; }
        public string? Imagine_Coperta { get; set; }
        public StatusDisponibilitate Status_Disponibilitate { get; set; } = StatusDisponibilitate.Disponibil;

        public ICollection<Imprumut>? Imprumuturi { get; set; }
        public ICollection<Recenzie>? Recenzii { get; set; }
        public ICollection<Lista_Dorinte>? ListaDorinte { get; set; }
    }
}
