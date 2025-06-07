using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PTFGym.Models
{
    public class Rating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ClanId { get; set; } // The user (Clan) who gave the rating

        [Required]
        public int TrenerId { get; set; } // The Trainer being rated

        [Required]
        [Range(1, 5)] // Ratings are between 1 and 5
        public int Score { get; set; } // The rating score (1 to 5)

        public string? Comment { get; set; } // Optional comment

        public DateTime Timestamp { get; set; } = DateTime.UtcNow; // When the rating was given

        // Navigation properties
        [ForeignKey("ClanId")]
        public virtual Clan Clan { get; set; }

        [ForeignKey("TrenerId")]
        public virtual Trener Trener { get; set; }
    }
}