using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PTFGym.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PTFGym.Controllers
{
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);  // Get the current logged-in user
            var roles = await _userManager.GetRolesAsync(user);  // Get the roles for the user

            // Pass the roles to the Razor view using a ViewModel
            var model = new DashboardViewModel
            {
                Roles = roles.ToList()  // Assign roles to the ViewModel
            };

            return View(model);  // Return the view with the roles
        }
    }
}
