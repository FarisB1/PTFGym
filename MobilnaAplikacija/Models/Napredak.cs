using System;

namespace MobilnaAplikacija.Models
{
    public class Napredak
    {
        public long id { get; set; }
        public DateTime datumUnosa { get; set; }
        public double tezina { get; set; }
        public string biljeske { get; set; }
        public string clanIme { get; set; }
        public string trenerIme { get; set; }
        public bool isUserProgress { get; set; }
    }
}