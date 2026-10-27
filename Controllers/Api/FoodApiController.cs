using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CalorieTracker.Data;
using CalorieTracker.Models;

namespace CalorieTracker.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodApiController : ControllerBase
    {
        private readonly CalorieTrackerContext _context;

        public FoodApiController(CalorieTrackerContext context)
        {
            _context = context;
        }

        // GET: api/FoodApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Food>>> GetFoods()
        {
            return await _context.Foods
                .Select(f => new Food
                {
                    Id = f.Id,
                    Name = f.Name,
                    Calories = f.Calories,
                    Protein = f.Protein,
                    Carbs = f.Carbs,
                    Fat = f.Fat
                })
                .ToListAsync();
        }

        // GET: api/FoodApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Food>> GetFood(long id)
        {
            var food = await _context.Foods.FindAsync(id);

            if (food == null)
            {
                return NotFound("Besin bulunamadı.");
            }

            return food;
        }

        // POST: api/FoodApi
        [HttpPost]
        public async Task<ActionResult<Food>> PostFood(Food food)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Foods.Add(food);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFood), new { id = food.Id }, food);
        }

        // DELETE: api/FoodApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFood(long id)
        {
            var food = await _context.Foods.FindAsync(id);
            if (food == null)
            {
                return NotFound("Besin bulunamadı.");
            }

            _context.Foods.Remove(food);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
