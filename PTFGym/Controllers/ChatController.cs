using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PTFGym.Data;
using PTFGym.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PTFGym.Controllers
{
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public ChatController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Chat/Index
        public async Task<IActionResult> Index(string receiverId = null)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Check if the current user is a trainer
            var isTrainer = await _context.Trener
                .AnyAsync(t => t.UserId == currentUserId);

            if (isTrainer)
            {
                return RedirectToAction("TrainerChat");
            }

            // Fetch all trainers directly from the database
            var treners = await _context.Trener.ToListAsync();
            ViewBag.Treners = new SelectList(treners, "UserId", "Ime");

            var messages = new List<ChatMessage>();
            if (!string.IsNullOrEmpty(receiverId))
            {
                messages = await _context.ChatMessages
                    .Where(m => (m.SenderId == currentUserId && m.ReceiverId == receiverId) ||
                               (m.SenderId == receiverId && m.ReceiverId == currentUserId))
                    .OrderBy(m => m.Timestamp)
                    .ToListAsync();
            }

            return View(messages);
        }

        public async Task<IActionResult> TrainerChat(string clientId = null)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Get all clients who have interacted with this trainer
            var userIds = await _context.ChatMessages
                .Where(m => m.SenderId == currentUserId || m.ReceiverId == currentUserId)
                .Select(m => m.SenderId == currentUserId ? m.ReceiverId : m.SenderId)
                .Distinct()
                .ToListAsync();

            var clients = await _userManager.Users
                .Where(u => userIds.Contains(u.Id))
                .Select(u => new { Id = u.Id, Name = u.UserName })
                .ToListAsync();

            // Convert to SelectList for the dropdown
            ViewBag.Clients = new SelectList(clients, "Id", "Name");

            var messages = new List<ChatMessage>();
            if (!string.IsNullOrEmpty(clientId))
            {
                messages = await _context.ChatMessages
                    .Where(m => (m.SenderId == currentUserId && m.ReceiverId == clientId) ||
                               (m.SenderId == clientId && m.ReceiverId == currentUserId))
                    .OrderBy(m => m.Timestamp)
                    .ToListAsync();
            }

            return View(messages);
        }

        [HttpGet]
        [Route("[Controller]/[Action]")]
        public async Task<ActionResult<IEnumerable<object>>> GetUsers()
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // First get the trainer record for the current user
            var trainer = await _context.Trener
                .FirstOrDefaultAsync(t => t.UserId == currentUserId);

            if (trainer == null)
            {
                return BadRequest("Trainer not found");
            }

            // Get all user IDs from chat messages
            var userIds = await _context.ChatMessages
                .Where(m => m.SenderId == currentUserId || m.ReceiverId == currentUserId)
                .Select(m => m.SenderId == currentUserId ? m.ReceiverId : m.SenderId)
                .Distinct()
                .ToListAsync();

            // Get the user details from AspNetUsers
            var users = await _userManager.Users
                .Where(u => userIds.Contains(u.Id))
                .Select(u => new
                {
                    id = u.Id,
                    ime = u.UserName  // or u.Email depending on what you want to display
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages(string receiverId)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var messages = await _context.ChatMessages
                .Where(m => (m.SenderId == currentUserId && m.ReceiverId == receiverId) ||
                           (m.SenderId == receiverId && m.ReceiverId == currentUserId))
                .OrderBy(m => m.Timestamp)
                .Select(m => new
                {
                    senderId = m.SenderId,
                    receiverId = m.ReceiverId,
                    message = m.Message,
                    timestamp = m.Timestamp
                })
                .ToListAsync();

            return Json(messages);
        }

        // POST: Chat/SendMessage
        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] ChatMessageDto message)
        {
            if (string.IsNullOrEmpty(message.ReceiverId) || string.IsNullOrEmpty(message.Message))
            {
                return BadRequest("Both receiver ID and message are required");
            }

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var chatMessage = new ChatMessage
            {
                SenderId = currentUserId,
                ReceiverId = message.ReceiverId,  // This will be the AspNetUsers Id
                Message = message.Message,
                Timestamp = DateTime.UtcNow,
                IsRead = false
            };

            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();

            return Json(new
            {
                senderId = chatMessage.SenderId,
                receiverId = chatMessage.ReceiverId,
                message = chatMessage.Message,
                timestamp = chatMessage.Timestamp
            });
        }

        public class ChatMessageDto
        {
            public string ReceiverId { get; set; }
            public string Message { get; set; }
        }
    }
}