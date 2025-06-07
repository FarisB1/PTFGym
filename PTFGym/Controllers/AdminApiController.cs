using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PTFGym.Data;
using PTFGym.Models;

namespace PTFGym.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrator")]
    public class AdminApiController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AdminApiController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public class AssignTrenerRequest
        {
            public string ClanId { get; set; } // ID from Clan table
            public string Ime { get; set; }
            public string Specijalnost { get; set; }
        }

        [HttpPost]
        [Route("AssignTrenerRole")]
        public async Task<IActionResult> AssignTrenerRole([FromBody] AssignTrenerRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Specijalnost))
                {
                    return BadRequest("Trainer specialty is required");
                }

                // Find user by Clan ID (assuming ClanId corresponds to UserId in AspNetUsers)
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.ClanId.ToString() == request.ClanId);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                // Check if the user is already a trainer
                if (await _userManager.IsInRoleAsync(user, "Trener"))
                {
                    return BadRequest("User is already a trainer");
                }

                // Create Trener entity (storing the UserId from AspNetUsers)
                var trener = new Trener
                {
                    Ime = request.Ime,
                    Specijalnost = request.Specijalnost,
                    UserId = user.Id // Assign AspNetUsers ID
                };

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Save the trainer record in the database
                        _context.Trener.Add(trener);
                        await _context.SaveChangesAsync();

                        // Ensure Trener role exists
                        var roleExists = await _context.Roles.AnyAsync(r => r.Name == "Trener");
                        if (!roleExists)
                        {
                            var role = new IdentityRole("Trener");
                            await _context.Roles.AddAsync(role);
                            await _context.SaveChangesAsync();
                        }

                        // Assign the "Trener" role to the user
                        var roleResult = await _userManager.AddToRoleAsync(user, "Trener");
                        if (!roleResult.Succeeded)
                        {
                            throw new Exception("Failed to assign trainer role");
                        }

                        // Associate the user with the Trener entity
                        user.TrenerId = trener.Id;
                        var updateResult = await _userManager.UpdateAsync(user);
                        if (!updateResult.Succeeded)
                        {
                            throw new Exception("Failed to update user");
                        }

                        await transaction.CommitAsync();

                        return Ok(new
                        {
                            message = "Successfully assigned trainer role",
                            trenerId = trener.Id
                        });
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }
    }
}
