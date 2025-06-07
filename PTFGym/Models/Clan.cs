using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PTFGym.Models
{
    public class Clan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Ime { get; set; }

        public string Email { get; set; }

        public DateTime DatumPocetkaClanstva { get; set; }

        public DateTime DatumKrajaClanstva { get; set; }
        public virtual List<Rezervacija>? Rezervacije { get; set; }
        public virtual List<Napredak>? Napredak { get; set; }
        public virtual List<Clanarina>? Clanarine { get; set; }
        public virtual List<Termin>? Termini { get; set; }

        public Clan()
        {

        }
    }
}
