using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PTFGym.Data;
using PTFGym.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PTFGym.Controllers
{
    public class NapredaksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public NapredaksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: Napredaks
        [HttpGet]
        [Route("Napredak")]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Index()
        {
            var napredaks = _context.Napredak.Include(n => n.Clan).Include(n => n.Trener);
            return View(await napredaks.ToListAsync());
        }

        // GET: Napredaks/Details/5

        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var napredak = await _context.Napredak
                .Include(n => n.Clan)
                .Include(n => n.Trener)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (napredak == null)
            {
                return NotFound();
            }

            return View(napredak);
        }

        // GET: Napredaks/Create
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || !user.TrenerId.HasValue)
            {
                return NotFound("User not found or not authorized");
            }

            var users = await _userManager.Users.ToListAsync();
            var currentUserId = user.Id;


            // Filter out users with roles "Administrator" or "Trener", excluding the current user
            var filteredUsers = new List<ApplicationUser>();

            foreach (var u in users)
            {
                // Check if user is not the current user
                if (u.Id != currentUserId)
                {
                    var isAdmin = await _userManager.IsInRoleAsync(u, "Administrator");
                    var isTrener = await _userManager.IsInRoleAsync(u, "Trener");

                    // Only add the user if they're not an Administrator or Trener
                    if (!isAdmin && !isTrener)
                    {
                        filteredUsers.Add(u);
                    }
                }
            }
            // Assuming ClanId is part of the ApplicationUser and we need to fetch the related Clan for the Ime
            var selectListItems = new List<SelectListItem>();

            foreach (var u in filteredUsers)
            {
                // Fetch the Clan associated with the user based on ClanId (Assuming ClanId is a property of ApplicationUser)
                var clan = await _context.Clan
                                           .FirstOrDefaultAsync(c => c.Id == u.ClanId); // Fetch the Clan by ClanId

                if (clan != null)
                {
                    selectListItems.Add(new SelectListItem
                    {
                        Value = clan.Id.ToString(),
                        Text = clan.Ime // Assuming Ime is a property of Clan model
                    });
                }
            }

            // Add this line to populate the Clan dropdown
            ViewData["ClanId"] = selectListItems;
            ViewData["DefaultDate"] = DateTime.Now;

            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Create([Bind("ClanId,DatumUnosa,Tezina,Biljeske")] Napredak napredak)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null || !user.TrenerId.HasValue)
                {
                    return NotFound("User not found or not authorized");
                }

                // Create new Napredak instance and set properties manually
                var newNapredak = new Napredak
                {
                    ClanId = napredak.ClanId,
                    TrenerId = user.TrenerId.Value,
                    DatumUnosa = napredak.DatumUnosa,
                    Tezina = napredak.Tezina,
                    Biljeske = napredak.Biljeske ?? string.Empty // Handle null Biljeske
                };

                System.Diagnostics.Debug.WriteLine($"Adding new Napredak: ClanId={newNapredak.ClanId}, " +
                    $"TrenerId={newNapredak.TrenerId}, " +
                    $"DatumUnosa={newNapredak.DatumUnosa}, " +
                    $"Tezina={newNapredak.Tezina}, " +
                    $"Biljeske={newNapredak.Biljeske}");

                _context.Napredak.Add(newNapredak);
                await _context.SaveChangesAsync();

                System.Diagnostics.Debug.WriteLine("Successfully saved to database");
                return RedirectToAction(nameof(TrenerIndex));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception occurred: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                ModelState.AddModelError("", $"Unable to save changes: {ex.Message}");
            }

            // If we got this far, something failed
            ViewData["ClanId"] = new SelectList(_context.Clan, "Id", "Ime", napredak.ClanId);
            return View(napredak);
        }

        // GET: Napredaks/Edit/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var napredak = await _context.Napredak.FindAsync(id);
            if (napredak == null)
            {
                return NotFound();
            }

            // Populate dropdown lists for Clan and Trener
            ViewData["ClanId"] = new SelectList(_context.Clan, "Id", "Ime", napredak.ClanId);
            ViewData["TrenerId"] = new SelectList(_context.Trener, "Id", "Ime", napredak.TrenerId);
            return View(napredak);
        }

        // POST: Napredaks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClanId,TrenerId,DatumUnosa,Tezina,Biljeske")] Napredak napredak)
        {
            if (id != napredak.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(napredak);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NapredakExists(napredak.Id))
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

            // If model state is not valid, repopulate the dropdown lists
            ViewData["ClanId"] = new SelectList(_context.Clan, "Id", "Ime", napredak.ClanId);
            ViewData["TrenerId"] = new SelectList(_context.Trener, "Id", "Ime", napredak.TrenerId);
            return View(napredak);
        }

        // GET: Napredaks/Delete/5
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var napredak = await _context.Napredak
                .Include(n => n.Clan)
                .Include(n => n.Trener)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (napredak == null)
            {
                return NotFound();
            }

            return View(napredak);
        }

        // POST: Napredaks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var napredak = await _context.Napredak.FindAsync(id);
            if (napredak != null)
            {
                _context.Napredak.Remove(napredak);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NapredakExists(int id)
        {
            return _context.Napredak.Any(e => e.Id == id);
        }

        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> ClanIndex()
        {
            // Get the current user (assuming ClanId is related to the user)
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // Retrieve Napredak entries for the current user's Clan
            var napredakForClan = await _context.Napredak
                .Where(n => n.ClanId == user.ClanId)  // Assuming the user has a ClanId
                .Include(n => n.Clan)
                .Include(n => n.Trener)
                .ToListAsync();

            var clan = await _context.Clan.FindAsync(user.ClanId);
            if (clan == null)
            {
                return NotFound();
            }

            // Pass the Clan's name and the Napredak entries to the view
            ViewData["ClanName"] = clan.Ime;
            return View(napredakForClan);
        }

        /// GET: Napredaks/TrenerIndex
        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<IActionResult> TrenerIndex()
        {
            // Get the current user (assuming TrenerId is related to the user)
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // Retrieve Napredak entries for the current user's Trener
            var napredakForTrener = await _context.Napredak
                .Where(n => n.TrenerId == user.TrenerId)  // Assuming the user has a TrenerId
                .Include(n => n.Clan)
                .Include(n => n.Trener)
                .ToListAsync(); // Await here

            var trener = await _context.Trener.FindAsync(user.TrenerId);
            if (trener == null)
            {
                return NotFound();
            }

            ViewData["TrenerName"] = trener.Ime;
            return View(napredakForTrener); // Now passing the actual list to the view
        }


    }
}
