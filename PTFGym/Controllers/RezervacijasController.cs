using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PTFGym.Data;
using PTFGym.Models;

namespace PTFGym.Controllers
{
    public class RezervacijasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RezervacijasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rezervacijas
        [HttpGet]
        [Route("Rezervacija")]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Rezervacija.Include(r => r.Clan).Include(r => r.Trener);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Rezervacijas/Details/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var rezervacija = await _context.Rezervacija
                .Include(r => r.Clan)
                .Include(r => r.Trener)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (rezervacija == null)
                return NotFound();

            return View(rezervacija);
        }

        // GET: Rezervacijas/Create
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Create()
        {
            await PopulateRezervacija();
            return View();
        }

        // POST: Rezervacijas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Create([Bind("Id,ClanId,TrenerId,DatumRezervacije")] Rezervacija rezervacija)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rezervacija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await PopulateRezervacija();
            return View(rezervacija);
        }

        // GET: Rezervacijas/Edit/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var rezervacija = await _context.Rezervacija.FindAsync(id);
            if (rezervacija == null)
                return NotFound();

            await PopulateRezervacija();
            return View(rezervacija);
        }

        // POST: Rezervacijas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClanId,TrenerId,DatumRezervacije")] Rezervacija rezervacija)
        {
            if (id != rezervacija.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rezervacija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RezervacijaExists(rezervacija.Id))
                        return NotFound();

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            await PopulateRezervacija();
            return View(rezervacija);
        }

        // GET: Rezervacijas/Delete/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var rezervacija = await _context.Rezervacija
                .Include(r => r.Clan)
                .Include(r => r.Trener)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (rezervacija == null)
                return NotFound();

            return View(rezervacija);
        }

        // POST: Rezervacijas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rezervacija = await _context.Rezervacija.FindAsync(id);
            if (rezervacija != null)
                _context.Rezervacija.Remove(rezervacija);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ClanIndex));
        }

        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> ClanIndex()
        {
            // Retrieve the current user's email or username
            var userEmail = User.Identity?.Name;

            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized("User is not authenticated.");
            }

            // Find the user's `Clan` based on the email
            var clan = await _context.Clan.FirstOrDefaultAsync(c => c.Email == userEmail);
            if (clan == null)
            {
                return NotFound("Clan not found.");
            }

            // Get reservations for the clan
            var rezervacije = await _context.Rezervacija
                .Include(r => r.Trener)
                .Where(r => r.ClanId == clan.Id)
                .ToListAsync();

            ViewBag.Rezervacije = rezervacije;  // Pass reservations to the view

            // Populate the dropdown for Treneri (trainers)
            await PopulateRezervacija();

            return View();
        }

        // POST: Rezervacija/ClanIndex
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> ClanIndex(Rezervacija rezervacija)
        {
            // Retrieve the current user's email or username
            var userEmail = User.Identity?.Name;

            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized("User is not authenticated.");
            }

            // Find the user's `Clan` based on the email
            var clan = await _context.Clan.FirstOrDefaultAsync(c => c.Email == userEmail);
            if (clan == null)
            {
                return NotFound("Clan not found.");
            }

            // Associate the clan with the reservation
            rezervacija.ClanId = clan.Id;

            if (ModelState.IsValid)
            {
                _context.Rezervacija.Add(rezervacija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ClanIndex));  // Redirect to the same page to see the updated reservations
            }

            // If model state is invalid, populate the trainers dropdown again
            await PopulateRezervacija();
            return View(rezervacija);
        }



        // GET: Rezervacija/TrenerIndex
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> TrenerIndex()
        {
            // Retrieve the current user's ID from the claims
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in claims.");
            }

            // Find the trainer using the userId (assuming it's linked to the Trener model)
            var trener = await _context.Trener.FirstOrDefaultAsync(t => t.UserId == userId);
            if (trener == null)
            {
                return NotFound("Trener not found.");
            }

            // Get reservations for the trainer
            var rezervacije = await _context.Rezervacija
                .Include(r => r.Clan)
                .Where(r => r.TrenerId == trener.Id)
                .ToListAsync();

            return View(rezervacije);
        }


        // POST: Rezervacija/ChangeTime
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> ChangeReservationTime(int id, DateTime newDate)
        {
            var rezervacija = await _context.Rezervacija.FindAsync(id);
            if (rezervacija == null)
                return NotFound();

            rezervacija.DatumRezervacije = newDate;
            _context.Rezervacija.Update(rezervacija);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(TrenerIndex));
        }

        private bool RezervacijaExists(int id)
        {
            return _context.Rezervacija.Any(e => e.Id == id);
        }

        private async Task PopulateRezervacija()
        {
            ViewBag.Clanovi = await _context.Clan
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Ime
                })
                .ToListAsync();

            ViewBag.Treneri = await _context.Trener
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Ime
                })
                .ToListAsync();
        }
    }
}
