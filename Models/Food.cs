using System.ComponentModel.DataAnnotations;

namespace CalorieTracker.Models
{
    public class Food
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public int Calories { get; set; } // per 100g
        
        public double? Protein { get; set; }
        
        public double? Carbs { get; set; }
        
        public double? Fat { get; set; }

        // Navigation property
        public ICollection<Consumption> Consumptions { get; set; } = new List<Consumption>();
    }
}
