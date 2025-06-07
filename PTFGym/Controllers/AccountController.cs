using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PTFGym.Data;
using PTFGym.Models;
using Microsoft.Extensions.Logging;
using PTFGym.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace PTFGym.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 ApplicationDbContext context,
                                 ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _logger = logger;
        }

        // API Registration
        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterApi([FromBody] RegisterRequest model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid registration details.");
            }

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

                return Ok(new { message = "Registration successful." });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest("Registration failed.");
        }

        // API Login
        [HttpPost("login")]
        public async Task<IActionResult> LoginApi([FromBody] LoginRequest model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid login details.");
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var clan = await _context.Clan.FirstOrDefaultAsync(c => c.Id == user.ClanId);
                
                var uloga = "";

                if (await _userManager.IsInRoleAsync(user, "Administartor"))
                {
                    uloga = "Administrator";
                }
                else if (await _userManager.IsInRoleAsync(user, "Trener"))
                {
                    uloga = "Trener";
                }
                else if (await _userManager.IsInRoleAsync(user, "Clan"))
                {
                    uloga = "Clan";
                }

                return Ok(new
                {
                    message = "Login successful.",
                    userId = user.Id,
                    clanId = user.ClanId,
                    email = user.Email,
                    ime = clan?.Ime,
                    role = uloga
                });
            }
            else if (result.IsLockedOut)
            {
                return Forbid("Account locked.");
            }
            else
            {
                return Unauthorized("Invalid login attempt.");
            }
        }
    }
}
