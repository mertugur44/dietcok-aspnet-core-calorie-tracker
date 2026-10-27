using System.ComponentModel.DataAnnotations;

namespace CalorieTracker.Models
{
    public class Consumption
    {
        public long Id { get; set; }

        [Required]
        public long UserId { get; set; }
        public User User { get; set; } = null!;

        [Required]
        public long FoodId { get; set; }
        public Food Food { get; set; } = null!;

        [Required]
        public DateOnly Date { get; set; }

        [Required]
        public MealType MealType { get; set; }

        [Required]
        public double Amount { get; set; }
    }

    public enum MealType
    {
        BREAKFAST,
        LUNCH,
        DINNER,
        SNACK
    }
}
