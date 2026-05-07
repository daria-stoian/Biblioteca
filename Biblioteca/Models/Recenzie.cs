using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteca.Models
{
    public class Recenzie
    {
        [Key]
        public int Id_Recenzie { get; set; }

        public int Id_Utilizator { get; set; }
        [ForeignKey("Id_Utilizator")]
        public Utilizator? Utilizator { get; set; }

        [Required(ErrorMessage = "Selectează o carte.")]
        public int Id_Carte { get; set; }
        [ForeignKey("Id_Carte")]
        public Carte? Carte { get; set; }

        [Required(ErrorMessage = "Nota este obligatorie.")]
        [Range(1, 5, ErrorMessage = "Nota trebuie să fie între 1 și 5.")]
        public int Nota { get; set; }

        [StringLength(2000, ErrorMessage = "Comentariul nu poate depăși 2000 de caractere.")]
        public string? Comentariu { get; set; }

        public DateTime Data_Postare { get; set; } = DateTime.UtcNow;
    }
}
