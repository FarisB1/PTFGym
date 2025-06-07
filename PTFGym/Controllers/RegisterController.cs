using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PTFGym.Data;
using PTFGym.Models;

namespace PTFGym.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public RegisterController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        // Registration for Clan
        [HttpGet]
        [Route("Register")]
        [Route("Account/[Action]")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Account/[Action]")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Create and link Clan
                    var clan = new Clan
                    {
                        Ime = model.Ime,
                        Email = model.Email,
                        DatumPocetkaClanstva = DateTime.Now,
                        DatumKrajaClanstva = DateTime.Now.AddMonths(1) // Example duration
                    };

                    _context.Clan.Add(clan);
                    await _context.SaveChangesAsync();

                    user.ClanId = clan.Id;
                    await _userManager.AddToRoleAsync(user, "Clan");
                    await _context.SaveChangesAsync();

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
    }
}