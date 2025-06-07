using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PTFGym.Models;
using SQLitePCL;
using System.Diagnostics;

namespace PTFGym.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("")]
        [Route("[Controller]/[Action]")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }


        [HttpGet]
        [Route("Onama")]
        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        [Route("Kontakt")]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpGet]
        [Route("[Controller]/[Action]/{clanId}")]
        public async Task<IActionResult> GetClanById(int clanId)
        {
            var clan = await _context.Clan.FindAsync(clanId);

            if (clan == null)
                return NotFound(new { message = "Clan not found" });

            return Ok(clan);
        }

        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllTermini()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            if (currentUser == null)
            {
                var termini = await _context.Termin
                    .Include(t => t.Trener)
                    .Include(t => t.Clanovi)
                    .Select(t => new
                    {
                        t.Id,
                        t.DatumVrijeme,
                        t.VrstaTreninga,
                        t.MaksimalniBrojClanova,
                        TrenerIme = t.Trener.Ime,
                        TrenutniBrojClanova = t.Clanovi.Count,
                        SlobodnaMjesta = t.MaksimalniBrojClanova - t.Clanovi.Count,
                        IsUserEnrolled = false,
                        Trener = new { t.Trener.Ime },
                        Clanovi = t.Clanovi.Select(c => new { c.Ime })
                    })
                    .ToListAsync();

                return Ok(termini);
            }
            else
            {
                var termini = await _context.Termin
                    .Include(t => t.Trener)
                    .Include(t => t.Clanovi)
                    .Select(t => new
                    {
                        t.Id,
                        t.DatumVrijeme,
                        t.VrstaTreninga,
                        t.MaksimalniBrojClanova,
                        TrenerIme = t.Trener.Ime,
                        TrenutniBrojClanova = t.Clanovi.Count,
                        SlobodnaMjesta = t.MaksimalniBrojClanova - t.Clanovi.Count,
                        IsUserEnrolled = t.Clanovi.Any(c => c.Id == currentUser.ClanId),
                        Trener = new { t.Trener.Ime },
                        Clanovi = t.Clanovi.Select(c => new { c.Ime })
                    })
                    .ToListAsync();

                return Ok(termini);
            }
        }

        [HttpGet]
        [Route("[Controller]/[Action]/{trenerId}")]
        public async Task<IActionResult> GetAllTerminTrener(int trenerId)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var termini = await _context.Termin
                .Include(t => t.Trener)
                .Include(t => t.Clanovi)
                .Where(t => t.TrenerId == trenerId)
                .Select(t => new
                {
                    t.Id,
                    t.DatumVrijeme,
                    t.VrstaTreninga,
                    t.MaksimalniBrojClanova,
                    TrenerIme = t.Trener.Ime,
                    TrenutniBrojClanova = t.Clanovi.Count,
                    SlobodnaMjesta = t.MaksimalniBrojClanova - t.Clanovi.Count,
                    IsUserEnrolled = currentUser != null && t.Clanovi.Any(c => c.Id == currentUser.ClanId),
                    Trener = new { t.Trener.Ime },
                    Clanovi = t.Clanovi.Select(c => new { c.Ime })
                })
                .ToListAsync();

            return Ok(termini);
        }

        [HttpGet]
        [Route("[Controller]/[Action]")]
        public ActionResult<IEnumerable<Trener>> GetAllTreneri()
        {
            return _context.Trener.ToList();
        }

        [HttpGet]
        [Route("[Controller]/[Action]")]
        public ActionResult<IEnumerable<Clan>> GetAllClanovi()
        {
            return _context.Clan.ToList();
        }

        [HttpGet]
        [Route("[Controller]/[Action]")]
        public ActionResult<IEnumerable<Clanarina>> GetAllClanarine()
        {
            return _context.Clanarina.ToList();
        }

        [HttpGet]
        [Route("[Controller]/[Action]")]
        public ActionResult<IEnumerable<Napredak>> GetAllNapredak()
        {
            return _context.Napredak.ToList();
        }

        [HttpGet]
        [Route("[Controller]/[Action]")]
        public ActionResult<IEnumerable<Rezervacija>> GetAllRezervacija()
        {
            return _context.Rezervacija.ToList();
        }

        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllRezervacije()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            if (currentUser == null)
            {
                var rezervacije = await _context.Rezervacija
                    .Include(r => r.Clan)
                    .Include(r => r.Trener)
                    .Select(r => new
                    {
                        r.Id,
                        r.DatumRezervacije,
                        ClanIme = r.Clan.Ime,
                        TrenerIme = r.Trener.Ime,
                        IsUserReservation = false
                    })
                    .ToListAsync();

                return Ok(rezervacije);
            }
            else
            {
                var rezervacije = await _context.Rezervacija
                    .Include(r => r.Clan)
                    .Include(r => r.Trener)
                    .Select(r => new
                    {
                        r.Id,
                        r.DatumRezervacije,
                        ClanIme = r.Clan.Ime,
                        TrenerIme = r.Trener.Ime,
                        IsUserReservation = r.ClanId == currentUser.ClanId
                    })
                    .ToListAsync();

                return Ok(rezervacije);
            }
        }

        [HttpGet]
        [Route("[Controller]/[Action]/{clanId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetClanRezervacije(int clanId)
        {
            var rezervacije = await _context.Rezervacija
                .Include(r => r.Trener)
                .Where(r => r.ClanId == clanId)
                .Select(r => new
                {
                    r.Id,
                    r.DatumRezervacije,
                    TrenerIme = r.Trener.Ime
                })
                .ToListAsync();
            return Ok(rezervacije);
        }


        [HttpGet]
        [Route("[Controller]/[Action]/{trenerId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetTrenerRezervacije(int trenerId)
        {
            var rezervacije = await _context.Rezervacija
                .Include(r => r.Clan)
                .Where(r => r.TrenerId == trenerId)
                .Select(r => new
                {
                    r.Id,
                    r.DatumRezervacije,
                    clanIme = r.Clan.Ime
                })
                .ToListAsync();
            return Ok(rezervacije);
        }

        

        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllNapredaks()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            if (currentUser == null)
            {
                var napredak = await _context.Napredak
                    .Include(n => n.Clan)
                    .Include(n => n.Trener)
                    .Select(n => new
                    {
                        n.Id,
                        n.DatumUnosa,
                        n.Tezina,
                        n.Biljeske,
                        ClanIme = n.Clan.Ime,
                        TrenerIme = n.Trener.Ime,
                        IsUserProgress = false
                    })
                    .ToListAsync();

                return Ok(napredak);
            }
            else
            {
                var napredak = await _context.Napredak
                    .Include(n => n.Clan)
                    .Include(n => n.Trener)
                    .Select(n => new
                    {
                        n.Id,
                        n.DatumUnosa,
                        n.Tezina,
                        n.Biljeske,
                        ClanIme = n.Clan.Ime,
                        TrenerIme = n.Trener.Ime,
                        IsUserProgress = n.ClanId == currentUser.ClanId
                    })
                    .ToListAsync();

                return Ok(napredak);
            }
        }

        [HttpGet]
        [Route("[Controller]/[Action]/{clanId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetClanNapredak(int clanId)
        {
            var napredak = await _context.Napredak
                .Include(n => n.Trener)
                .Where(n => n.ClanId == clanId)
                .Select(n => new
                {
                    n.Id,
                    n.DatumUnosa,
                    n.Tezina,
                    n.Biljeske,
                    TrenerIme = n.Trener.Ime
                })
                .ToListAsync();
            return Ok(napredak);
        }

        [HttpGet]
        [Route("[Controller]/[Action]/{trenerId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetTrenerNapredak(int trenerId)
        {
            var napredak = await _context.Napredak
                .Include(n => n.Clan)
                .Where(n => n.TrenerId == trenerId)
                .Select(n => new
                {
                    n.Id,
                    n.DatumUnosa,
                    n.Tezina,
                    n.Biljeske,
                    ClanIme = n.Clan.Ime
                })
                .ToListAsync();
            return Ok(napredak);
        }

        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllClanarina()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            if (currentUser == null)
            {
                var clanarine = await _context.Clanarina
                    .Include(c => c.Clan)
                    .Select(c => new
                    {
                        c.Id,
                        c.DatumPocetka,
                        c.DatumZavrsetka,
                        c.Iznos,
                        ClanIme = c.Clan.Ime,
                        IsUserMembership = false
                    })
                    .ToListAsync();

                return Ok(clanarine);
            }
            else
            {
                var clanarine = await _context.Clanarina
                    .Include(c => c.Clan)
                    .Select(c => new
                    {
                        c.Id,
                        c.DatumPocetka,
                        c.DatumZavrsetka,
                        c.Iznos,
                        ClanIme = c.Clan.Ime,
                        IsUserMembership = c.ClanId == currentUser.ClanId
                    })
                    .ToListAsync();

                return Ok(clanarine);
            }
        }

        [HttpGet]
        [Route("[Controller]/[Action]/{trenerId}")]
        public async Task<ActionResult<object>> GetTrainerDetails(int trenerId)
        {
            var trener = await _context.Trener
                .Include(t => t.Termini)
                .Include(t => t.Napredaks)
                .FirstOrDefaultAsync(t => t.Id == trenerId);

            if (trener == null)
                return NotFound();

            var terminCount = await _context.Termin
                .CountAsync(t => t.TrenerId == trenerId);

            var napredakCount = await _context.Napredak
                .CountAsync(n => n.TrenerId == trenerId);

            return Ok(new
            {
                trener.Id,
                trener.Ime,
                trener.Email,
                trener.Specijalnost,
                TerminCount = terminCount,
                NapredakCount = napredakCount,
                Termini = trener.Termini?.Select(t => new
                {
                    t.Id,
                    t.DatumVrijeme,
                    t.VrstaTreninga,
                    ClanoviCount = t.Clanovi?.Count ?? 0
                }),
                Napredaks = trener.Napredaks?.Select(n => new
                {
                    n.Id,
                    n.DatumUnosa,
                    n.Tezina,
                    ClanIme = n.Clan.Ime
                })
            });
        }

        [HttpGet]
        [Route("[Controller]/[Action]/{clanId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetMemberTermini(int clanId)
        {
            var termini = await _context.Termin
                .Include(t => t.Trener)
                .Include(t => t.Clanovi)
                .Where(t => t.Clanovi.Any(c => c.Id == clanId))
                .Select(t => new
                {
                    t.Id,
                    t.DatumVrijeme,
                    t.VrstaTreninga,
                    t.MaksimalniBrojClanova,
                    TrenerIme = t.Trener.Ime,
                    TrenutniBrojClanova = t.Clanovi.Count,
                    SlobodnaMjesta = t.MaksimalniBrojClanova - t.Clanovi.Count
                })
                .ToListAsync();

            return Ok(termini);
        }


        [HttpGet]
        [Route("[Controller]/[Action]/{terminId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetTerminClanovi(int terminId)
        {
            var termin = await _context.Termin
                .Include(t => t.Clanovi)
                .FirstOrDefaultAsync(t => t.Id == terminId);

            if (termin == null)
                return NotFound();

            var clanovi = termin.Clanovi.Select(c => new
            {
                c.Id,
                c.Ime,
                c.Email
            }).ToList();

            return Ok(clanovi);
        }


        [HttpGet]
        [Route("[Controller]/[Action]")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [Route("[Controller]/[Action]/{clanId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetClanClanarina(int clanId)
        {
            var clanarine = await _context.Clanarina
                .Where(c => c.ClanId == clanId)
                .Select(c => new
                {
                    c.Id,
                    c.DatumPocetka,
                    c.DatumZavrsetka,
                    c.Iznos,
                    CanRenew = c.DatumZavrsetka <= DateTime.Now.AddDays(7)
                })
                .ToListAsync();

            return Ok(clanarine);
        }

        [HttpPost]
        [Route("[Controller]/[Action]/{clanId}")]
        public async Task<IActionResult> RenewMembershipApi(int clanId)
        {
            try
            {
                if (clanId <= 0)
                {
                    return BadRequest(new { message = "Nevažeći ID člana!" });
                }

                var lastMembership = await _context.Clanarina
                    .Where(c => c.ClanId == clanId)
                    .OrderByDescending(c => c.DatumZavrsetka)
                    .FirstOrDefaultAsync();

                if (lastMembership == null)
                {
                    return NotFound(new { message = "Nije pronađena članarina za obnovu!" });
                }

                // Određivanje datuma početka nove članarine
                var renewalStartDate = lastMembership.DatumZavrsetka > DateTime.Now
                    ? lastMembership.DatumZavrsetka
                    : DateTime.Now;

                var newMembership = new Clanarina
                {
                    ClanId = clanId,
                    DatumPocetka = renewalStartDate,
                    DatumZavrsetka = renewalStartDate.AddMonths(1),
                    Iznos = lastMembership.Iznos
                };

                _context.Clanarina.Add(newMembership);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Uspješno obnovljena članarina!", newMembership });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška prilikom obnove članarine za člana ID: {ClanId}", clanId);
                return StatusCode(500, new { message = "Došlo je do greške prilikom obnove članarine!" });
            }
        }


        [HttpPost]
        [Route("[Controller]/CreateRezervacijaAPI")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> CreateRezervacijaAPI([FromBody] Rezervacija rezervacija)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(rezervacija);
                    await _context.SaveChangesAsync();
                    return Ok(new { success = true, message = "Reservation created successfully" });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { success = false, message = "Server error", details = ex.Message });
                }
            }

            return BadRequest(new { success = false, message = "Invalid reservation data" });
        }

        [HttpPost]
        [Route("[Controller]/CreateNapredakAPI")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> CreateNapredakAPI([FromBody] Napredak napredak)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Validate required fields
                    if (napredak.ClanId <= 0 || napredak.TrenerId <= 0)
                    {
                        return BadRequest(new { success = false, message = "ClanId and TrenerId are required" });
                    }

                    // Optional: Validate if Clan and Trener exist
                    var clanExists = await _context.Clan.AnyAsync(c => c.Id == napredak.ClanId);
                    var trenerExists = await _context.Trener.AnyAsync(t => t.Id == napredak.TrenerId);

                    if (!clanExists || !trenerExists)
                    {
                        return BadRequest(new { success = false, message = "Invalid ClanId or TrenerId" });
                    }

                    _context.Add(napredak);
                    await _context.SaveChangesAsync();
                    return Ok(new { success = true, message = "Napredak created successfully" });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { success = false, message = "Server error", details = ex.Message });
                }
            }

            return BadRequest(new { success = false, message = "Invalid napredak data", errors = ModelState.Values.SelectMany(v => v.Errors) });
        }



        [HttpGet]
        [Route("[Controller]/[Action]/{clanIme}")]
        public async Task<IActionResult> GetClanIdByName(string clanIme)
        {
            var clan = await _context.Clan
                .FirstOrDefaultAsync(c => c.Ime == clanIme);

            if (clan == null)
                return NotFound();

            return Ok(clan.Id);
        }

        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllClans()
        {
            try
            {
                // Get all trainers
                var trenerIds = await _context.Trener
                    .Select(t => t.Id)
                    .ToListAsync();

                // Get all users who have trainer records linked to their ApplicationUser
                var trenerUserIds = await _context.Users
                    .Where(u => u.TrenerId.HasValue)
                    .Select(u => u.ClanId)
                    .ToListAsync();

                // Get all Clan records that aren't trainers
                var clanovi = await _context.Clan
                    .Where(c => !trenerIds.Contains(c.Id) && !trenerUserIds.Contains(c.Id))
                    .Select(c => new
                    {
                        c.Id,
                        c.Ime,
                        c.Email,
                        c.DatumPocetkaClanstva,
                        c.DatumKrajaClanstva
                    })
                    .ToListAsync();

                return Ok(clanovi);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving clan list");
                return StatusCode(500, new { message = "An error occurred while retrieving the clan list", error = ex.Message });
            }
        }
        [HttpPut]
        [Route("[Controller]/[Action]")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> UpdateClan([FromBody] Clan clan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Invalid clan data", errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            try
            {
                // Find the existing Clan by ID
                var existingClan = await _context.Clan.FindAsync(clan.Id);
                if (existingClan == null)
                {
                    return NotFound(new { success = false, message = "Clan not found" });
                }

                // Log the existingClan and clan object for debugging purposes
                _logger.LogInformation("Existing Clan: {ExistingClanEmail}, Updated Clan Email: {UpdatedClanEmail}", existingClan.Email, clan.Email);

                // Update Clan table
                var updateClanSql = "UPDATE Clan SET Ime = {0}, Email = {1} WHERE Id = {2}";
                await _context.Database.ExecuteSqlRawAsync(updateClanSql,
                    clan.Ime,
                    clan.Email,
                    clan.Id);

                // Update AspNetUsers where the old email matches
                var updateUserSql = @"
            UPDATE AspNetUsers 
            SET 
                UserName = {0}, 
                NormalizedUserName = {1}, 
                Email = {2}, 
                NormalizedEmail = {3}
            WHERE Email = {4}"; // Match using the old email value from existingClan.Email

                _logger.LogInformation("Executing SQL: {UpdateUserSql}, Parameters: {Email}, {NormalizedEmail}, {ExistingEmail}",
                    updateUserSql, clan.Email, clan.Email.ToUpper(), existingClan.Email);

                // Execute the SQL to update AspNetUsers
                var rowsAffected = await _context.Database.ExecuteSqlRawAsync(updateUserSql,
                    clan.Email,
                    clan.Email.ToUpper(),
                    clan.Email,
                    clan.Email.ToUpper(),
                    existingClan.Email); // Match with the old email from the Clan object

                // Check if any rows were affected (to confirm the update worked)
                if (rowsAffected == 0)
                {
                    return NotFound(new { success = false, message = "No matching user found with the old email" });
                }

                return Ok(new { success = true, message = "Clan and user updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating clan with ID: {ClanId}", clan.Id);
                return StatusCode(500, new { success = false, message = "Server error", details = ex.Message });
            }
        }


    }
}
