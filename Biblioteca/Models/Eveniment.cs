using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteca.Models
{
    public class Eveniment
    {
        [Key]
        public int Id_Eveniment { get; set; }

        [Required]
        public string Titlu_Eveniment { get; set; }
        public string? Descriere { get; set; }
        public DateTime Data_Ora { get; set; }
        public string? Locatie { get; set;}
        public int Locuri_Disponibile { get; set; }
        [NotMapped]
        public int Locuri_Rezervate { get; set; } = 0;
        
        [NotMapped]
        public int Locuri_Ramase => Locuri_Disponibile - Locuri_Rezervate;

        [NotMapped]
        public bool EstePlin => Locuri_Ramase <= 0;

        public ICollection<Rezervare_Eveniment>? Rezervari { get; set; }
    }
}
