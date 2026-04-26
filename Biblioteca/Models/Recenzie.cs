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
        public int Id_Carte { get; set; }
        [ForeignKey("Id_Carte")]
        public Carte? Carte { get; set; }

        public int Nota { get; set; }
        public string? Comentariu { get; set; }
        public DateTime Data_Postare { get; set; } = DateTime.UtcNow;
    }
}
