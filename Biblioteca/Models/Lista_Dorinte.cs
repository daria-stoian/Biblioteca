using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Biblioteca.Models
{
    public class Lista_Dorinte
    {
        public string Id_Utilizator { get; set; } = string.Empty;
        [ForeignKey("Id_Utilizator")]
        public Utilizator Utilizator { get; set; }
        public int Id_Carte { get; set; }
        [ForeignKey("Id_Carte")]
        public Carte Carte { get; set; }

        public DateTime Data_Adaugare { get; set; }
    }
}
