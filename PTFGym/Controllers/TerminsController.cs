using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PTFGym.Data;
using PTFGym.Models;

namespace PTFGym.Controllers
{
    public class TerminsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TerminsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Termins
        [HttpGet]
        [Route("Termin")]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Termin.Include(t => t.Trener);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Termins/Create
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Create()
        {
            await PopulateTreneriDropdown();
            return View();
        }

        // POST: Termins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Create([Bind("Id,DatumVrijeme,VrstaTreninga,MaksimalniBrojClanova,TrenerId")] Termin termin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(termin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(TrenerIndex));
            }

            await PopulateTreneriDropdown();
            return View(termin);
        }

        [HttpPost("TestCreate")]
        public async Task<IActionResult> TestCreate([FromBody] Termin termin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(termin);
                await _context.SaveChangesAsync();
                return Ok($"Termin created successfully: {termin.Id} | {termin.TrenerId} | {termin.DatumVrijeme} | {termin.VrstaTreninga}");
            }

            await PopulateTreneriDropdown();
            return View(termin);

        }

        // GET: Termins/Edit/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var termin = await _context.Termin
                .Include(t => t.Trener) // Include the related Trener entity
                .FirstOrDefaultAsync(m => m.Id == id); // Find the termin by ID

            if (termin == null)
            {
                return NotFound();
            }

            // Use TrenerIme instead of TrenerId for the dropdown
            ViewData["TrenerIme"] = new SelectList(_context.Trener, "Id", "Ime", termin.TrenerId);
            return View(termin);
        }

        // POST: Termins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DatumVrijeme,VrstaTreninga,MaksimalniBrojClanova,TrenerId")] Termin termin)
        {
            if (id != termin.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(termin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TerminExists(termin.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(TrenerIndex));
            }
            ViewData["TrenerId"] = new SelectList(_context.Trener, "Id", "Id", termin.TrenerId);
            return View(termin);
        }

        // GET: Termins/Delete/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var termin = await _context.Termin
                .Include(t => t.Trener)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (termin == null)
            {
                return NotFound();
            }

            return View(termin);
        }

        // POST: Termins/Delete/5
        [HttpPost, ActionName("Delete")]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var termin = await _context.Termin.FindAsync(id);
            if (termin != null)
            {
                _context.Termin.Remove(termin);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Route("[Controller]/[Action]/{id}")]
        public async Task<IActionResult> DeleteApi(int id)
        {
            var termin = await _context.Termin.FindAsync(id);
            if (termin != null)
            {
                _context.Termin.Remove(termin);
            }

            await _context.SaveChangesAsync();
            return Ok("Deleted successfully.");
        }
        private bool TerminExists(int id)
        {
            return _context.Termin.Any(e => e.Id == id);
        }


        // Method to populate the trainer dropdown (for the Create and Edit views)
        private async Task PopulateTreneriDropdown()
        {
            ViewBag.Treneri = await _context.Trener
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Ime // Assuming you have an 'Ime' property for trainer names
                })
                .ToListAsync();
        }

        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> ClanIndex()
        {
            // Get the current logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Korisnik nije pronađen.");
            }

            // Ensure the user is a Clan (group member)
            if (!user.ClanId.HasValue)
            {
                return Forbid("Nemate pristup");
            }

            // Retrieve the list of Termins where there are available spots
            var clanTermini = await _context.Termin
                .Include(t => t.Trener)  // Include the Trainer's data
                .Include(t => t.Clanovi)  // Include the members list
                .Where(t => t.MaksimalniBrojClanova > t.Clanovi.Count) // Ensure termins have available spots
                .ToListAsync();

            // Add a flag to indicate whether the user is already enrolled in each Termin
            foreach (var termin in clanTermini)
            {
                termin.IsUserEnrolled = termin.Clanovi.Any(c => c.Id == user.ClanId); // Check if the user is already enrolled
            }

            if (!clanTermini.Any())
            {
                ViewData["Message"] = "Trenutno nema termina";
            }

            return View(clanTermini);
        }


        [HttpPost]
        public async Task<IActionResult> Join(int terminId)
        {
            // Get the current user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            // Get the current user's Clan
            var clan = await _context.Clan.FirstOrDefaultAsync(c => c.Id == user.ClanId);
            if (clan == null)
            {
                return NotFound("Clan nije pronađen.");
            }

            // Get the Termin
            var termin = await _context.Termin.Include(t => t.Clanovi).FirstOrDefaultAsync(t => t.Id == terminId);
            if (termin == null)
            {
                return NotFound("Termin nije pronađen.");
            }

            // Check if the user is already enrolled in this Termin
            if (termin.Clanovi.Any(c => c.Id == user.ClanId))
            {
                return BadRequest("Već ste u ovom terminu.");
            }

            // Check if the Termin has available spots
            if (termin.Clanovi.Count >= termin.MaksimalniBrojClanova)
            {
                return BadRequest("Nema slobodnih mjesta.");
            }

            // Add the Clan to the Termin
            termin.Clanovi.Add(clan);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        [Route("[Controller]/[Action]/{clanId}/{terminId}")]
        public async Task<IActionResult> JoinApi(int clanId, int terminId)
        {
            // Validate input
            if (clanId <= 0 || terminId <= 0)
            {
                return BadRequest("Nevažeći ID.");
            }

            // Get the current user's Clan
            var clan = await _context.Clan.FirstOrDefaultAsync(c => c.Id == clanId);
            if (clan == null)
            {
                return NotFound("Clan nije pronađen.");
            }

            // Get the Termin
            var termin = await _context.Termin
                .Include(t => t.Clanovi) // Include the Clanovi collection
                .FirstOrDefaultAsync(t => t.Id == terminId);

            if (termin == null)
            {
                return NotFound("Termin nije pronađen.");
            }

            // Check if the user is already enrolled in this Termin
            if (termin.Clanovi.Any(c => c.Id == clanId))
            {
                return BadRequest("Već ste u ovom terminu.");
            }

            // Check if the Termin has available spots
            if (termin.Clanovi.Count >= termin.MaksimalniBrojClanova)
            {
                return BadRequest("Nema slobodnih mjesta.");
            }

            // Add the Clan to the Termin
            termin.Clanovi.Add(clan);
            await _context.SaveChangesAsync();

            return Ok();
        }


        // GET: Termins/Details/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Details(int id)
        {
            var termin = await _context.Termin
                .Include(t => t.Trener)
                .Include(t => t.Clanovi)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (termin == null)
            {
                return NotFound();
            }

            return View(termin);
        }


        // GET: Termins/TrenerIndex
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> TrenerIndex()
        {
            // Get the current logged-in user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Ensure the user is a trainer and has access
            if (!user.TrenerId.HasValue)
            {
                return Forbid("Nemate pristup.");
            }

            // Retrieve the list of Termins for the logged-in trainer
            var trenerTermini = await _context.Termin
                .Include(t => t.Clanovi) // Assuming there's a list of members (Clanovi) per Termin
                .Include(t => t.Trener)
                .Where(t => t.TrenerId == user.TrenerId.Value)
                .ToListAsync();

            if (!trenerTermini.Any())
            {
                ViewData["Message"] = "Trenutno nema zakazanih termina.";
            }

            return View(trenerTermini);
        }


    }
}
