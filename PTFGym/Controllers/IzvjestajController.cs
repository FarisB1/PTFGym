using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PTFGym.Data;
using PTFGym.Models;

namespace PTFGym.Controllers
{
    public class IzvjestajController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IzvjestajController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Izvjestaj
        [HttpGet]
        [Route("Izvjestaj")]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Index()
        {
            var report = new IzvjestajViewModel
            {
                NumberOfUsers = await _context.Users.CountAsync(),
                NumberOfRezervacijas = await _context.Rezervacija.CountAsync(),
                TerminiPerMonth = await GetTerminiPerMonthAsync(),
                ClanarinePerMonth = await GetClanarinePerMonthAsync()
            };

            return View(report);
        }

        private async Task<Dictionary<string, int>> GetTerminiPerMonthAsync()
        {
            var terminiPerMonth = await _context.Rezervacija
                .GroupBy(r => new { r.DatumRezervacije.Year, r.DatumRezervacije.Month })
                .Select(g => new
                {
                    Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                    Count = g.Count()
                })
                .ToDictionaryAsync(g => g.Month, g => g.Count);

            return terminiPerMonth;
        }

        private async Task<Dictionary<string, ClanarinaSummary>> GetClanarinePerMonthAsync()
        {
            var clanarinePerMonth = await _context.Clanarina
                .GroupBy(c => new { c.DatumPocetka.Year, c.DatumPocetka.Month })
                .Select(g => new
                {
                    Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                    Count = g.Count(),
                    TotalAmount = g.Sum(c => c.Iznos)
                })
                .ToDictionaryAsync(g => g.Month, g => new ClanarinaSummary
                {
                    Count = g.Count,
                    TotalAmount = (int)g.TotalAmount
                });

            return clanarinePerMonth;
        }
    }

    public class IzvjestajViewModel
    {
        public int NumberOfUsers { get; set; }
        public int NumberOfRezervacijas { get; set; }
        public Dictionary<string, int> TerminiPerMonth { get; set; }
        public Dictionary<string, ClanarinaSummary> ClanarinePerMonth { get; set; }
    }

    public class ClanarinaSummary
    {
        public int Count { get; set; }
        public decimal TotalAmount { get; set; }
    }
}