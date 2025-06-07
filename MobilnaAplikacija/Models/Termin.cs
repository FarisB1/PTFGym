using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilnaAplikacija.Models
{
    public class Termin
    {
        public long id { get; set; }
        public DateTime datumVrijeme { get; set; }
        public string vrstaTreninga { get; set; }
        public long maksimalniBrojClanova { get; set; }
        public long trenerId { get; set; }
        public int trenutniBrojClanova { get; set; }  // New property
        public int slobodnaMjesta { get; set; }       // New property
        public string trenerIme { get; set; }
        public bool IsUserEnrolled { get; set; }
        public List<Clan> Clanovi { get; set; } = new List<Clan>();

    }

}
