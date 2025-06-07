using System;
using System.ComponentModel.DataAnnotations;

namespace PTFGym.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }

        [Required]
        public string SenderId { get; set; } // User ID of the sender (Clan or Trener)

        [Required]
        public string ReceiverId { get; set; } // User ID of the receiver (Clan or Trener)

        [Required]
        public string Message { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false; // To track if the message has been read
    }
}