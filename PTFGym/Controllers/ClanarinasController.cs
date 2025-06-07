using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PTFGym.Data;
using PTFGym.DTO;
using PTFGym.Extensions;
using PTFGym.Models;

namespace PTFGym.Controllers
{
    public class ClanarinasController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;
        public ClanarinasController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Clanarinas
        [HttpGet]
        [Route("Clanarina")]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Clanarina.Include(c => c.Clan);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Clanarinas/Details/5

        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clanarina = await _context.Clanarina
                .Include(c => c.Clan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clanarina == null)
            {
                return NotFound();
            }

            return View(clanarina);
        }

        // GET: Clanarinas/Create
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public IActionResult Create()
        {
            ViewData["ClanId"] = new SelectList(_context.Clan, "Id", "Id");
            return View();
        }

        // POST: Clanarinas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Create([Bind("Id,ClanId,DatumPocetka,DatumZavrsetka,Iznos")] Clanarina clanarina)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clanarina);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClanId"] = new SelectList(_context.Clan, "Id", "Id", clanarina.ClanId);
            return View(clanarina);
        }

        // GET: Clanarinas/Edit/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clanarina = await _context.Clanarina.FindAsync(id);
            if (clanarina == null)
            {
                return NotFound();
            }
            ViewData["ClanId"] = new SelectList(_context.Clan, "Id", "Id", clanarina.ClanId);
            return View(clanarina);
        }

        // POST: Clanarinas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClanId,DatumPocetka,DatumZavrsetka,Iznos")] Clanarina clanarina)
        {
            if (id != clanarina.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clanarina);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClanarinaExists(clanarina.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClanId"] = new SelectList(_context.Clan, "Id", "Id", clanarina.ClanId);
            return View(clanarina);
        }

        // GET: Clanarinas/Delete/5

        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clanarina = await _context.Clanarina
                .Include(c => c.Clan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clanarina == null)
            {
                return NotFound();
            }

            return View(clanarina);
        }

        // POST: Clanarinas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clanarina = await _context.Clanarina.FindAsync(id);
            if (clanarina != null)
            {
                _context.Clanarina.Remove(clanarina);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClanarinaExists(int id)
        {
            return _context.Clanarina.Any(e => e.Id == id);
        }

        [HttpGet]
        [Route("ClanIndex")]
        public async Task<IActionResult> ClanIndex()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Check if user has any membership
            var anyMembership = await _context.Clanarina
                .AnyAsync(c => c.ClanId == currentUser.ClanId);

            if (!anyMembership)
            {
                // Create initial membership
                var newMembership = new Clanarina
                {
                    ClanId = (int)currentUser.ClanId,
                    DatumPocetka = DateTime.Now,
                    DatumZavrsetka = DateTime.Now.AddDays(1),
                    Iznos = 50 // Default membership price
                };
                _context.Clanarina.Add(newMembership);
                await _context.SaveChangesAsync();
            }

            var activeClanarina = await _context.Clanarina
                .Where(c => c.ClanId == currentUser.ClanId && c.DatumZavrsetka >= DateTime.Now)
                .OrderByDescending(c => c.DatumZavrsetka)
                .FirstOrDefaultAsync();

            var clanarinaHistory = await _context.Clanarina
                .Where(c => c.ClanId == currentUser.ClanId)
                .OrderByDescending(c => c.DatumZavrsetka)
                .ToListAsync();

            var viewModel = new ClanarinaViewModel
            {
                ActiveMembership = activeClanarina,
                MembershipHistory = clanarinaHistory,
                CanRenew = activeClanarina?.DatumZavrsetka <= DateTime.Now.AddDays(7)
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("RenewMembership")]
        public async Task<IActionResult> RenewMembership()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var lastMembership = await _context.Clanarina
                    .Where(c => c.ClanId == currentUser.ClanId)
                    .OrderByDescending(c => c.DatumZavrsetka)
                    .FirstOrDefaultAsync();

                if (lastMembership == null)
                {
                    TempData["ErrorMessage"] = "Nije pronađena članarina za obnovu";
                    return RedirectToAction(nameof(ClanIndex));
                }

                // Determine renewal start date
                var renewalStartDate = lastMembership.DatumZavrsetka > DateTime.Now
                    ? lastMembership.DatumZavrsetka
                    : DateTime.Now;

                var newMembership = new Clanarina
                {
                    ClanId = (int)currentUser.ClanId,
                    DatumPocetka = renewalStartDate,
                    DatumZavrsetka = renewalStartDate.AddMonths(1),
                    Iznos = lastMembership.Iznos
                };

                _context.Clanarina.Add(newMembership);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Uspješno obnovljena članarina!";
                return RedirectToAction(nameof(ClanIndex));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Greška!";
                return RedirectToAction(nameof(ClanIndex));
            }
        }

     
    }
}
