using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CalorieTracker.Data;
using CalorieTracker.Models;
using Microsoft.AspNetCore.Authorization;

namespace CalorieTracker.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly CalorieTrackerContext _context;

        public ProfileController(CalorieTrackerContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("profile")]
        public async Task<IActionResult> Index(string? success)
        {
            var username = User.Identity?.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return RedirectToAction("Login", "Account");

            if (success != null)
            {
                ViewBag.SuccessMessage = "Hedefleriniz başarıyla güncellendi.";
            }

            return View(user);
        }

        [HttpPost]
        [Route("profile/update-goal")]
        public async Task<IActionResult> UpdateGoal(int dailyCalorieGoal, int? dailyProteinGoal, int? dailyCarbGoal, int? dailyFatGoal)
        {
            var username = User.Identity?.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return RedirectToAction("Login", "Account");

            user.DailyCalorieGoal = dailyCalorieGoal;
            user.DailyProteinGoal = dailyProteinGoal;
            user.DailyCarbGoal = dailyCarbGoal;
            user.DailyFatGoal = dailyFatGoal;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { success = "true" });
        }
    }
}
