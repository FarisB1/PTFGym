using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace PTFGym.Models
{
    public class ApplicationUser : IdentityUser
    {
        [ForeignKey("Clan")]
        public int? ClanId { get; set; }
        public virtual Clan Clan { get; set; }

        [ForeignKey("Trener")]
        public int? TrenerId{ get; set; }
        public virtual Trener Trener { get; set; }

        public ApplicationUser() { }
    }
}
