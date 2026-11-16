using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteca.Models
{
    public class Eveniment
    {
        [Key]
        public int Id_Eveniment { get; set; }

        [Required(ErrorMessage = "Titlul evenimentului este obligatoriu.")]
        [StringLength(200, ErrorMessage = "Titlul nu poate depăși 200 de caractere.")]
        public string Titlu_Eveniment { get; set; }

        [StringLength(2000)]
        public string? Descriere { get; set; }

        [Required(ErrorMessage = "Data și ora sunt obligatorii.")]
        public DateTime Data_Ora { get; set; }

        [StringLength(200)]
        public string? Locatie { get; set; }

        [Required(ErrorMessage = "Numărul de locuri este obligatoriu.")]
        [Range(1, 9999, ErrorMessage = "Trebuie să existe cel puțin un loc disponibil.")]
        public int Locuri_Disponibile { get; set; }

        public int Locuri_Rezervate { get; set; } = 0;

        [NotMapped]
        public int Locuri_Ramase => Locuri_Disponibile - Locuri_Rezervate;

        [NotMapped]
        public bool EstePlin => Locuri_Ramase <= 0;

        public ICollection<Rezervare_Eveniment>? Rezervari { get; set; }
    }
}
