using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PTFGym.Models
{
    public class Rezervacija
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Clan")]
        public int ClanId { get; set; }

        [ForeignKey("Trener")]
        public int TrenerId { get; set; }

        public DateTime DatumRezervacije { get; set; }

        public virtual Clan? Clan { get; set; }
        public virtual Trener? Trener { get; set; }
        public Rezervacija()
        {

        }
    }
}
