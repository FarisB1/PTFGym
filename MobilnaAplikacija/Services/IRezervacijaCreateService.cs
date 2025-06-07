using MobilnaAplikacija.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilnaAplikacija.Services
{
    public interface IRezervacijaCreateService
    {
        Task<List<Trener>> GetTreneri();
        Task<bool> CreateRezervacija(int trenerId, DateTime datumRezervacije);
    }
}
