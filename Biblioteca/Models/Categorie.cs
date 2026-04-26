using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class Categorie
    {
        [Key]
        public int Id_Categorie { get; set; }

        [Required(ErrorMessage = "Numele categoriei este obligatoriu.")]
        [Display(Name = "Nume Categorie")]
        public string Nume_Categorie { get; set; }

        public ICollection<Carte>? Carti { get; set; }
    }
}