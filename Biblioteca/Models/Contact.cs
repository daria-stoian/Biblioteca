using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteca.Models
{
    public enum StatusMesaj
    {
        Necitit,
        Citit,
        Raspuns_Trimis
    }

    public class Contact
    {
        [Key]
        public int Id_Mesaj { get; set; }

        public int? Id_Utilizator { get; set; }
        [ForeignKey("Id_Utilizator")]
        public Utilizator? Utilizator { get; set; }

        public string? Nume_Expeditor { get; set; }
        public string? Email_Expeditor { get; set; }
        public string? Mesaj { get; set; }
        public DateTime Data_Trimitere { get; set; } = DateTime.UtcNow;
        public StatusMesaj Status_Mesaj { get; set; } = StatusMesaj.Necitit;
    }
}
