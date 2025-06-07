using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PTFGym.Models
{
    public class Napredak
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Clan")]
        public int ClanId { get; set; }

        [ForeignKey("Trener")]
        public int TrenerId { get; set; }

        public DateTime DatumUnosa { get; set; }

        public float Tezina { get; set; }

        public string Biljeske { get; set; }

        public virtual Clan? Clan { get; set; }
        public virtual Trener? Trener { get; set; }

        public Napredak()
        {
        }
    }
}
