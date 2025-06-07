using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PTFGym.Data;
using PTFGym.Models;

namespace PTFGym.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AdminController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // Display all users in the Admin View
        public IActionResult AssignTrenerRole()
        {
            var users = _userManager.Users.ToList();
            return View(users); // Pass users to view
        }

        // Handle the form submission and assign role
        [HttpPost]
        public async Task<IActionResult> AssignTrenerRole(string userId, string ime, string specijalnost)
        {
            // Find user by ID
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Create Trener entity
            var trener = new Trener
            {
                Ime = ime,
                Specijalnost = specijalnost,
                UserId = userId
            };

            // Save Trener to database
            _context.Trener.Add(trener);
            await _context.SaveChangesAsync();

            // Assign the "Trener" role to the user
            var roleExists = await _context.Roles.AnyAsync(r => r.Name == "Trener");
            if (!roleExists)
            {
                var role = new IdentityRole("Trener");
                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();
            }

            await _userManager.AddToRoleAsync(user, "Trener");

            // Associate user with the Trener
            user.TrenerId = trener.Id;
            await _userManager.UpdateAsync(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "");
        }
    }
}