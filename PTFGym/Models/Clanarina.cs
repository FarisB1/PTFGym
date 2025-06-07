using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PTFGym.Models
{
    public class Clanarina
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Clan")]
        public int ClanId { get; set; }

        public DateTime DatumPocetka { get; set; }

        public DateTime DatumZavrsetka { get; set; }

        public float Iznos { get; set; }

        public virtual Clan? Clan { get; set; }

        public Clanarina()
        {

        }
    }
}
