using Microsoft.EntityFrameworkCore;
using CalorieTracker.Models;

namespace CalorieTracker.Data
{
    public class CalorieTrackerContext : DbContext
    {
        public CalorieTrackerContext(DbContextOptions<CalorieTrackerContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Food> Foods { get; set; } = null!;
        public DbSet<Consumption> Consumptions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<Consumption>()
                .HasOne(c => c.User)
                .WithMany(u => u.Consumptions)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Consumption>()
                .HasOne(c => c.Food)
                .WithMany(f => f.Consumptions)
                .HasForeignKey(c => c.FoodId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
