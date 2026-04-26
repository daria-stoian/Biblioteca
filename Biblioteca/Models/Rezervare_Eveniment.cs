using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteca.Models
{
    public class Rezervare_Eveniment
    {
        public enum StatusRezervare
        {
            Activă,
            Anulată,
            Finalizată
        }

        [Key]
        public int Id_Rezervare { get; set; }

        public int Id_Utilizator { get; set; }
        [ForeignKey("Id_Utilizator")]
        public Utilizator? Utilizator { get; set; }
        public int Id_Eveniment { get; set; }
        [ForeignKey("Id_Eveniment")]
        public Eveniment? Eveniment { get; set; }
        public DateTime Data_Rzervare { get; set; } = DateTime.UtcNow;
        public StatusRezervare Status_Rezervare { get; set; } = StatusRezervare.Activă;
    }
}
