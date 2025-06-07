using System;

namespace MobilnaAplikacija.Models
{
    public class Rezervacija
    {
        public long id { get; set; }
        public DateTime datumRezervacije { get; set; }
        public string clanIme { get; set; }
        public string trenerIme { get; set; }
        public bool IsUserReservation { get; set; }
    }
}