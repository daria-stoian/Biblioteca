using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteca.Models
{
    public class Imprumut
    {
        public enum StatusImprumut
        {
            Activ,
            Returnat,
            Intarziat,
            Prelungit
        }

        [Key]
        public int Id_Imprumut { get; set; }
        public int Id_Utilizator { get; set; }
        [ForeignKey("Id_Utilizator")]
        public Utilizator? Utilizator { get; set; }
        public int Id_Carte { get; set; }
        [ForeignKey("Id_Carte")]
        public Carte? Carte { get; set; }

        public DateTime Data_Imprumut { get; set; } = DateTime.UtcNow;
        public DateTime Data_Scadenta { get; set; }
        public DateTime? Data_Returnare { get; set; }
        public decimal Taxa_Penalizare { get; set; } = 0;
        public StatusImprumut Status { get; set; } = StatusImprumut.Activ;

        [NotMapped]
        public bool EsteIntarziat => Data_Returnare == null && DateTime.UtcNow > Data_Scadenta;

        [NotMapped]
        public int ZileIntarziere => EsteIntarziat
            ? (int)(DateTime.UtcNow - Data_Scadenta).TotalDays
            : 0;
    }
}
