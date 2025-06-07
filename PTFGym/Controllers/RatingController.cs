using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PTFGym.Data;
using PTFGym.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace PTFGym.Controllers
{
    public class RatingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RatingController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Rating/Index
        public async Task<IActionResult> Index()
        {
            var trainers = await _context.Trener
                .Select(t => new TrainerViewModel
                {
                    Id = t.Id,
                    Name = t.Ime,
                    AverageRating = _context.Ratings
                        .Where(r => r.TrenerId == t.Id)
                        .Average(r => (double?)r.Score) ?? 0,
                    TotalRatings = _context.Ratings
                        .Count(r => r.TrenerId == t.Id)
                })
                .ToListAsync();

            return View(trainers);
        }

        // GET: Rating/RateTrainer/{id}
        public async Task<IActionResult> RateTrainer(int id)
        {
            var trainer = await _context.Trener
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trainer == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var hasRated = false;

            if (currentUser?.ClanId != null)
            {
                hasRated = await _context.Ratings
                    .AnyAsync(r => r.ClanId == currentUser.ClanId && r.TrenerId == id);
            }

            var viewModel = new RateTrainerViewModel
            {
                TrainerId = trainer.Id,
                TrainerName = trainer.Ime,
                HasRated = hasRated
            };

            return View(viewModel);
        }

        // POST: Rating/Submit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit([FromBody] RatingDto ratingDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser?.ClanId == null)
            {
                return Unauthorized("Only members can submit ratings.");
            }

            var existingRating = await _context.Ratings
                .FirstOrDefaultAsync(r => r.ClanId == currentUser.ClanId && r.TrenerId == ratingDto.TrenerId);

            if (existingRating != null)
            {
                return BadRequest("You have already rated this trainer.");
            }

            var rating = new Rating
            {
                ClanId = (int)currentUser.ClanId,
                TrenerId = ratingDto.TrenerId,
                Score = ratingDto.Score,
                Comment = ratingDto.Comment,
                Timestamp = DateTime.UtcNow
            };

            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Rating submitted successfully." });
        }

        // GET: Rating/Average/{trenerId}
        [HttpGet]
        [Route("[Controller]/[Action]/{trenerId}")]
        public async Task<IActionResult> Average(int trenerId)
        {
            var ratings = await _context.Ratings
                .Where(r => r.TrenerId == trenerId)
                .Select(r => (double?)r.Score)
                .ToListAsync(); // Fetch data first

            var averageRating = ratings.Any() ? ratings.Average() : 0;

            return Json(new { averageRating = Math.Round((double)averageRating, 2) });
        }


        // GET: Rating/Details/{trenerId}
        [HttpGet]
        [Route("[Controller]/[Action]/{trenerId}")]
        public async Task<IActionResult> Details(int trenerId)
        {
            try
            {
                Console.WriteLine($"Fetching ratings for Trainer ID: {trenerId}");

                // Validate trenerId
                if (trenerId <= 0)
                {
                    Console.WriteLine("Invalid trenerId value.");
                    return Json(new { message = "Invalid trainer ID." });
                }

                // Log the generated SQL query
                var sqlQuery = _context.Ratings
                    .Where(r => r.TrenerId == trenerId)
                    .Include(r => r.Clan)
                    .OrderByDescending(r => r.Timestamp)
                    .ToQueryString();

                Console.WriteLine($"Generated SQL Query: {sqlQuery}");

                // Fetch ratings for the given trainer ID
                var ratings = await _context.Ratings
                    .Where(r => r.TrenerId == trenerId)
                    .Include(r => r.Clan)
                    .OrderByDescending(r => r.Timestamp)
                    .Select(r => new
                    {
                        r.Score,
                        r.Comment,
                        r.Timestamp,
                        ClanName = r.Clan != null ? r.Clan.Ime : "Anonymous"
                    })
                    .ToListAsync();

                Console.WriteLine($"Found {ratings.Count} ratings for Trainer ID: {trenerId}");

                // If there are no ratings, return a message
                if (ratings.Count == 0)
                {
                    Console.WriteLine($"No ratings found for Trainer ID: {trenerId}");
                    return Json(new { message = "No ratings available for this trainer." });
                }

                // Return the ratings as JSON
                return Json(ratings);
            }
            catch (Exception ex)
            {
                // Log the error and return a message to the client
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { message = "An error occurred while fetching ratings." });
            }
        }



    }

    public class TrainerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double AverageRating { get; set; }
        public int TotalRatings { get; set; }
    }

    public class RateTrainerViewModel
    {
        public int TrainerId { get; set; }
        public string TrainerName { get; set; }
        public bool HasRated { get; set; }
    }

    public class RatingDto
    {
        public int TrenerId { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Score { get; set; }

        [MaxLength(500)]
        public string? Comment { get; set; }
    }
}