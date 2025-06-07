namespace MobilnaAplikacija.Dto
{
    public class ClanarinaDto
    {
        public int Id { get; set; }
        public DateTime DatumPocetka { get; set; }
        public DateTime DatumZavrsetka { get; set; }
        public float Iznos { get; set; }
        public int ClanId { get; set; }
        public string ClanIme { get; set; }
        public bool CanRenew { get; set; }
    }
}