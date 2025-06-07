namespace PTFGym.Models
{
    public class TrainerViewModel
    {
        public List<Trener> Treners { get; set; } // List of all Trainers
        public int SelectedTrenerId { get; set; } // Selected Trainer for rating or chat
        public List<ChatMessage> ChatMessages { get; set; } // Chat messages for the selected Trainer
        public double AverageRating { get; set; } // Average rating for the selected Trainer
        public List<Rating> Ratings { get; set; } // Rating details for the selected Trainer
    }
}