using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CalorieTracker.Data;
using CalorieTracker.Models;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;

namespace CalorieTracker.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly CalorieTrackerContext _context;

        public DashboardController(CalorieTrackerContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("dashboard")]
        public async Task<IActionResult> Index(int? year, int? month)
        {
            var username = User.Identity?.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return RedirectToAction("Login", "Account");

            var now = DateTime.Now;
            int targetYear = year ?? now.Year;
            int targetMonth = month ?? now.Month;

            var startDate = new DateOnly(targetYear, targetMonth, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var monthlyConsumptions = await _context.Consumptions
                .Include(c => c.Food)
                .Where(c => c.UserId == user.Id && c.Date >= startDate && c.Date <= endDate)
                .ToListAsync();

            var monthlyStats = monthlyConsumptions
                .GroupBy(c => c.Date)
                .ToDictionary(
                    g => g.Key,
                    g => new
                    {
                        Calories = g.Sum(c => (int)(c.Food.Calories * c.Amount)),
                        Protein = g.Sum(c => (c.Food.Protein ?? 0.0) * c.Amount),
                        Carbs = g.Sum(c => (c.Food.Carbs ?? 0.0) * c.Amount),
                        Fat = g.Sum(c => (c.Food.Fat ?? 0.0) * c.Amount)
                    }
                );

            var weeks = new List<List<CalendarDay?>>();
            var currentWeek = new List<CalendarDay?>();

            int firstDayVal = (int)startDate.DayOfWeek;
            int padding = (firstDayVal + 6) % 7; // Monday start padding

            for (int i = 0; i < padding; i++)
            {
                currentWeek.Add(null);
            }

            int daysInMonth = DateTime.DaysInMonth(targetYear, targetMonth);
            for (int day = 1; day <= daysInMonth; day++)
            {
                var date = new DateOnly(targetYear, targetMonth, day);
                string status = "no-data";
                int? total = null;
                double protein = 0.0, carbs = 0.0, fat = 0.0;

                if (monthlyStats.TryGetValue(date, out var stats))
                {
                    total = stats.Calories;
                    protein = stats.Protein;
                    carbs = stats.Carbs;
                    fat = stats.Fat;
                    status = total > user.DailyCalorieGoal ? "over-limit" : "under-limit";
                }

                currentWeek.Add(new CalendarDay(day, date, status, total, protein, carbs, fat));

                if (currentWeek.Count == 7)
                {
                    weeks.Add(currentWeek);
                    currentWeek = new List<CalendarDay?>();
                }
            }

            if (currentWeek.Count > 0)
            {
                while (currentWeek.Count < 7)
                {
                    currentWeek.Add(null);
                }
                weeks.Add(currentWeek);
            }

            var trLocale = new CultureInfo("tr-TR");
            string monthNameTr = startDate.ToString("MMMM", trLocale).ToUpper(trLocale);

            ViewBag.Username = username;
            ViewBag.CurrentMonthName = monthNameTr;
            ViewBag.CurrentYear = targetYear;
            ViewBag.Weeks = weeks;
            ViewBag.User = user;

            ViewBag.PrevMonth = targetMonth == 1 ? 12 : targetMonth - 1;
            ViewBag.PrevYear = targetMonth == 1 ? targetYear - 1 : targetYear;
            ViewBag.NextMonth = targetMonth == 12 ? 1 : targetMonth + 1;
            ViewBag.NextYear = targetMonth == 12 ? targetYear + 1 : targetYear;

            return View();
        }

        [HttpGet]
        [Route("day/{date}")]
        public async Task<IActionResult> DailyView(string date)
        {
            if (!DateOnly.TryParseExact(date, "yyyy-MM-dd", out var localDate))
            {
                return BadRequest("Geçersiz tarih formatı.");
            }

            var username = User.Identity?.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return RedirectToAction("Login", "Account");

            var consumptions = await _context.Consumptions
                .Include(c => c.Food)
                .Where(c => c.UserId == user.Id && c.Date == localDate)
                .OrderBy(c => c.MealType)
                .ToListAsync();

            var groupedConsumptions = new Dictionary<MealType, List<Consumption>>
            {
                { MealType.BREAKFAST, new List<Consumption>() },
                { MealType.LUNCH, new List<Consumption>() },
                { MealType.DINNER, new List<Consumption>() },
                { MealType.SNACK, new List<Consumption>() }
            };

            foreach (var c in consumptions)
            {
                groupedConsumptions[c.MealType].Add(c);
            }

            var mealTotals = new Dictionary<MealType, int>();
            foreach (var kvp in groupedConsumptions)
            {
                int sum = kvp.Value.Sum(c => (int)Math.Round(c.Food.Calories * c.Amount));
                mealTotals[kvp.Key] = sum;
            }

            int totalCalories = consumptions.Sum(c => (int)(c.Food.Calories * c.Amount));
            bool isOverLimit = totalCalories > user.DailyCalorieGoal;

            double totalProtein = consumptions.Sum(c => (c.Food.Protein ?? 0.0) * c.Amount);
            double totalCarbs = consumptions.Sum(c => (c.Food.Carbs ?? 0.0) * c.Amount);
            double totalFat = consumptions.Sum(c => (c.Food.Fat ?? 0.0) * c.Amount);

            var foods = await _context.Foods.OrderBy(f => f.Name).ToListAsync();

            ViewBag.Date = localDate.ToString("yyyy-MM-dd");
            ViewBag.GroupedConsumptions = groupedConsumptions;
            ViewBag.MealTotals = mealTotals;
            ViewBag.TotalCalories = totalCalories;
            ViewBag.TotalProtein = totalProtein.ToString("F1", CultureInfo.InvariantCulture);
            ViewBag.TotalCarbs = totalCarbs.ToString("F1", CultureInfo.InvariantCulture);
            ViewBag.TotalFat = totalFat.ToString("F1", CultureInfo.InvariantCulture);
            ViewBag.IsOverLimit = isOverLimit;
            ViewBag.Foods = foods;
            ViewBag.UserGoal = user.DailyCalorieGoal;

            return View();
        }

        [HttpPost]
        [Route("consumption/add")]
        public async Task<IActionResult> AddConsumption(string date, long foodId, MealType mealType, double amount)
        {
            if (!DateOnly.TryParseExact(date, "yyyy-MM-dd", out var localDate))
            {
                return BadRequest("Geçersiz tarih.");
            }

            var username = User.Identity?.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return RedirectToAction("Login", "Account");

            var food = await _context.Foods.FindAsync(foodId);
            if (food == null) return NotFound("Besin bulunamadı.");

            var consumption = new Consumption
            {
                UserId = user.Id,
                FoodId = food.Id,
                Date = localDate,
                MealType = mealType,
                Amount = amount
            };

            _context.Consumptions.Add(consumption);
            await _context.SaveChangesAsync();

            return RedirectToAction("DailyView", new { date = date });
        }

        [HttpPost]
        [Route("consumption/delete")]
        public async Task<IActionResult> DeleteConsumption(long id, string date)
        {
            var username = User.Identity?.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return RedirectToAction("Login", "Account");

            var consumption = await _context.Consumptions.FindAsync(id);
            if (consumption != null && consumption.UserId == user.Id)
            {
                _context.Consumptions.Remove(consumption);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("DailyView", new { date = date });
        }
    }

    public class CalendarDay
    {
        public int Day { get; set; }
        public DateOnly Date { get; set; }
        public string Status { get; set; } // 'no-data', 'over-limit', 'under-limit'
        public int? TotalCalories { get; set; }
        public double Protein { get; set; }
        public double Carbs { get; set; }
        public double Fat { get; set; }

        public CalendarDay(int day, DateOnly date, string status, int? totalCalories, double protein, double carbs, double fat)
        {
            Day = day;
            Date = date;
            Status = status;
            TotalCalories = totalCalories;
            Protein = protein;
            Carbs = carbs;
            Fat = fat;
        }
    }
}
