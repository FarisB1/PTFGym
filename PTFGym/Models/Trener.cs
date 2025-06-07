using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PTFGym.Models
{
    public class Trener
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Ime { get; set; }

        public string? Email { get; set; }

        [Required]
        public string Specijalnost { get; set; }


        public string UserId { get; set; }
        public virtual List<ApplicationUser>? Korisnici { get; set; }
        
        public virtual List<Termin>? Termini { get; set; }

        public virtual List<Napredak>? Napredaks { get; set; }
        public Trener()
        {

        }
    }
}
