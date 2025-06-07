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
    public class TrenersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrenersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Treners

        [HttpGet]
        [Route("Treneri")]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Trener.ToListAsync());
        }

        // GET: Treners/Details/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trener = await _context.Trener
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trener == null)
            {
                return NotFound();
            }

            return View(trener);
        }

        // GET: Treners/Create
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Treners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Create([Bind("Id,Ime,Specijalnost")] Trener trener)
        {
            if (!ModelState.IsValid)
            {
                foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                {
                    // Log or debug write the error
                    System.Diagnostics.Debug.WriteLine(modelError.ErrorMessage);
                }
            }

            if (ModelState.IsValid)
            {
                try {
                _context.Add(trener);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); 
                }
                catch (Exception ex)
                {
                    // Log the exception
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    // Add to ModelState
                    ModelState.AddModelError("", "Unable to save changes. Try again.");
                }
            }


            return View(trener);
        }

        // GET: Treners/Edit/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trener = await _context.Trener.FindAsync(id);
            if (trener == null)
            {
                return NotFound();
            }
            return View(trener);
        }

        // POST: Treners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int id, [Bind("Ime,Specijalnost")] Trener trener)
        {
            if (id != trener.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trener);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrenerExists(trener.Id))
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
            return View(trener);
        }

        // GET: Treners/Delete/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trener = await _context.Trener
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trener == null)
            {
                return NotFound();
            }

            return View(trener);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trener = await _context.Trener.FindAsync(id);
            if (trener == null)
            {
                return NotFound();
            }

            // Find the associated ApplicationUser record
            var associatedUser = await _context.Users
                .FirstOrDefaultAsync(u => u.TrenerId == trener.Id);

            if (associatedUser != null)
            {
                // Remove all role assignments for this user
                var userRoles = await _context.UserRoles
                    .Where(ur => ur.UserId == associatedUser.Id)
                    .ToListAsync();

                _context.UserRoles.RemoveRange(userRoles);
            }

            // Manually detach the users by setting the TrenerId property to null
            var usersWithTrener = _context.Set<ApplicationUser>().Where(u => u.TrenerId == trener.Id).ToList();
            foreach (var user in usersWithTrener)
            {
                user.TrenerId = null;  // Detach the user from the Trener
                _context.Users.Update(user);  // Update the user entity
            }

            // Delete all Termin records associated with this Trener
            var terminRecords = _context.Termin.Where(t => t.TrenerId == trener.Id).ToList();
            _context.Termin.RemoveRange(terminRecords);  // Delete the Termin records

            // Now delete the trainer
            _context.Trener.Remove(trener);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [Route("[Controller]/[Action]/{id}")]  // New API route
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> TrenersDelete(int id)
        {
            var trener = await _context.Trener.FindAsync(id);
            if (trener == null)
            {
                return NotFound();
            }

            // Find the associated ApplicationUser record
            var associatedUser = await _context.Users
                .FirstOrDefaultAsync(u => u.TrenerId == trener.Id);

            if (associatedUser != null)
            {
                // Remove all role assignments for this user
                var userRoles = await _context.UserRoles
                    .Where(ur => ur.UserId == associatedUser.Id)
                    .ToListAsync();

                _context.UserRoles.RemoveRange(userRoles);
            }

            // Manually detach the users by setting the TrenerId property to null
            var usersWithTrener = _context.Set<ApplicationUser>().Where(u => u.TrenerId == trener.Id).ToList();
            foreach (var user in usersWithTrener)
            {
                user.TrenerId = null;
                _context.Users.Update(user);
            }

            // Delete all Termin records associated with this Trener
            var terminRecords = _context.Termin.Where(t => t.TrenerId == trener.Id).ToList();
            _context.Termin.RemoveRange(terminRecords);

            // Now delete the trainer
            _context.Trener.Remove(trener);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Trainer deleted successfully" });
        }


        private bool TrenerExists(int id)
        {
            return _context.Trener.Any(e => e.Id == id);
        }

    }
}
