using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilnaAplikacija.Models
{
    public class LoginResponse
    {
        public string userId { get; set; }
        public int clanId { get; set; }
        public string email { get; set; }
        public string ime { get; set; }
        public string role { get; set; }
    }
}
