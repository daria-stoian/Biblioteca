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

        [Required(ErrorMessage = "Selectează un autor.")]
        public int Id_Autor { get; set; }
        [ForeignKey("Id_Autor")]
        public Autor? Autor { get; set; }

        [Required(ErrorMessage = "Selectează o categorie.")]
        public int Id_Categorie { get; set; }
        [ForeignKey("Id_Categorie")]
        public Categorie? Categorie { get; set; }

        [Required(ErrorMessage = "Titlul este obligatoriu.")]
        [StringLength(200, ErrorMessage = "Titlul nu poate depăși 200 de caractere.")]
        public string Titlu { get; set; }

        [StringLength(20, ErrorMessage = "ISBN-ul nu poate depăși 20 de caractere.")]
        public string? ISBN { get; set; }

        [Range(1000, 2100, ErrorMessage = "Anul publicării trebuie să fie între 1000 și 2100.")]
        public int An_Publicare { get; set; }

        [Range(1, 10000, ErrorMessage = "Numărul de pagini trebuie să fie între 1 și 10000.")]
        public int Numar_Pagini { get; set; }

        [StringLength(2000, ErrorMessage = "Descrierea nu poate depăși 2000 de caractere.")]
        public string? Descriere { get; set; }

        public string? Imagine_Coperta { get; set; }

        public string? Fisier_PDF { get; set; }

        public StatusDisponibilitate Status_Disponibilitate { get; set; } = StatusDisponibilitate.Disponibil;

        public ICollection<Imprumut>? Imprumuturi { get; set; }
        public ICollection<Recenzie>? Recenzii { get; set; }
        public ICollection<Lista_Dorinte>? ListaDorinte { get; set; }
    }
}
