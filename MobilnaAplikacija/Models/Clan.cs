using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilnaAplikacija.Models
{
    public class Clan
    {
        public int id { get; set; }

        public string ime { get; set; }

        public string email { get; set; }

        public DateTime datumPocetkaClanstva { get; set; }

        public DateTime datumKrajaClanstva { get; set; }
    }
}
