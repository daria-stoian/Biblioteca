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

        [Required(ErrorMessage = "Numele este obligatoriu.")]
        [StringLength(100, ErrorMessage = "Numele nu poate depăși 100 de caractere.")]
        public string? Nume_Expeditor { get; set; }

        [Required(ErrorMessage = "Email-ul este obligatoriu.")]
        [EmailAddress(ErrorMessage = "Adresa de email nu este validă.")]
        [StringLength(150)]
        public string? Email_Expeditor { get; set; }

        [Required(ErrorMessage = "Mesajul nu poate fi gol.")]
        [StringLength(2000, ErrorMessage = "Mesajul nu poate depăși 2000 de caractere.")]
        public string? Mesaj { get; set; }

        public DateTime Data_Trimitere { get; set; } = DateTime.UtcNow;
        public StatusMesaj Status_Mesaj { get; set; } = StatusMesaj.Necitit;
    }
}
