using PTFGym.DTO;
using PTFGym.Models;

namespace PTFGym.Extensions
{
    public static class ClanarinaExtensions
    {
        public static bool CanRenew(this Clanarina clanarina)
        {
            return DateTime.Now.AddDays(7) >= clanarina.DatumZavrsetka;
        }

        public static ClanarinaDto ToDto(this Clanarina clanarina)
        {
            return new ClanarinaDto
            {
                Id = clanarina.Id,
                ClanId = clanarina.ClanId,
                DatumPocetka = clanarina.DatumPocetka,
                DatumZavrsetka = clanarina.DatumZavrsetka,
                Iznos = clanarina.Iznos,
                CanRenew = clanarina.CanRenew()
            };
        }
    }
}
