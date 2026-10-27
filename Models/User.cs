using System.ComponentModel.DataAnnotations;

namespace CalorieTracker.Models
{
    public class User
    {
        public long Id { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public int DailyCalorieGoal { get; set; }

        public int? DailyProteinGoal { get; set; }
        public int? DailyCarbGoal { get; set; }
        public int? DailyFatGoal { get; set; }

        // Navigation property
        public ICollection<Consumption> Consumptions { get; set; } = new List<Consumption>();
    }
}
