using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PTFGym.Models
{
    public class Termin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime DatumVrijeme { get; set; }

        public string VrstaTreninga { get; set; } // Personalni, Grupni, itd.

        public int MaksimalniBrojClanova { get; set; }


        [ForeignKey("Trener")]
        public int? TrenerId { get; set; }
        public virtual Trener? Trener { get; set; }

        // Many-to-Many relationship with Clan
        public virtual List<Clan>? Clanovi { get; set; }

        [NotMapped] // This ensures it doesn't get stored in the database
        public bool IsUserEnrolled { get; set; }
        public Termin()
        {
            Clanovi = new List<Clan>();
        }
    }
}
