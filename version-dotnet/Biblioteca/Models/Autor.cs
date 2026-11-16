using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class Autor
    {
        [Key]
        public int Id_Autor { get; set; }

        [Required]
        public string Nume_Autor { get; set; }

        public string? Biografie { get; set; }

        public ICollection<Carte>? Carti { get; set; }
    }
}
