using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PTFGym.Data;
using PTFGym.Models;

namespace PTFGym.Controllers
{
    public class ClansController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClansController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Clans
        [HttpGet]
        [Route("Clan")]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clan.ToListAsync());
        }

        // GET: Clans/Details/5
        [HttpGet]
        [Route("[Controller]/[Action]")]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clan = await _context.Clan
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clan == null)
            {
                return NotFound();
            }

            return View(clan);
        }

        // GET: Clans/Create
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Create([Bind("Id,Ime,Email,DatumPocetkaClanstva,DatumKrajaClanstva")] Clan clan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(clan);
        }

        // GET: Clans/Edit/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clan = await _context.Clan.FindAsync(id);
            if (clan == null)
            {
                return NotFound();
            }
            return View(clan);
        }

        // POST: Clans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ime,Email,DatumPocetkaClanstva,DatumKrajaClanstva")] Clan clan)
        {
            if (id != clan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClanExists(clan.Id))
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
            return View(clan);
        }

        // GET: Clans/Delete/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clan = await _context.Clan
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clan == null)
            {
                return NotFound();
            }

            return View(clan);
        }

        // POST: Clans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Find the Clan to be deleted
            var clan = await _context.Clan.FindAsync(id);

            if (clan != null)
            {
                // Fetch all users related to this Clan
                var relatedUsers = _context.Users
                    .OfType<ApplicationUser>() // Cast to ApplicationUser
                    .Where(u => u.ClanId == id)
                    .ToList();

                // Nullify ClanId for related users
                foreach (var user in relatedUsers)
                {
                    user.ClanId = null;
                }

                // Save changes for Users
                await _context.SaveChangesAsync();

                // Now remove the Clan
                _context.Clan.Remove(clan);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }



        private bool ClanExists(int id)
        {
            return _context.Clan.Any(e => e.Id == id);
        }
    }
}
