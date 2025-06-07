using PTFGym.Models;

public class ClanarinaViewModel
{
    public Clanarina? ActiveMembership { get; set; }
    public List<Clanarina> MembershipHistory { get; set; } = new List<Clanarina>();
    public bool CanRenew { get; set; }

}