using CalorieTracker.Models;

namespace CalorieTracker.Data
{
    public static class DbInitializer
    {
        public static void Initialize(CalorieTrackerContext context)
        {
            context.Database.EnsureCreated();

            // Look for any foods.
            if (context.Foods.Any())
            {
                return;   // DB has been seeded
            }

            var foods = new Food[]
            {
                new Food { Name = "Muz (100g)", Calories = 89, Protein = 1.1, Carbs = 22.8, Fat = 0.3 },
                new Food { Name = "Elma (100g)", Calories = 52, Protein = 0.3, Carbs = 13.8, Fat = 0.2 },
                new Food { Name = "Tavuk Göğsü (100g)", Calories = 165, Protein = 31.0, Carbs = 0.0, Fat = 3.6 },
                new Food { Name = "Beyaz Pirinç Pilavı (100g)", Calories = 130, Protein = 2.7, Carbs = 28.0, Fat = 0.3 },
                new Food { Name = "Haşlanmış Yumurta (Adet - 50g)", Calories = 78, Protein = 6.3, Carbs = 0.6, Fat = 5.3 },
                new Food { Name = "Yulaf Ezmesi (100g)", Calories = 389, Protein = 16.9, Carbs = 66.3, Fat = 6.9 },
                new Food { Name = "Tam Yağlı Süt (100ml)", Calories = 61, Protein = 3.2, Carbs = 4.8, Fat = 3.3 },
                new Food { Name = "Tam Buğday Ekmeği (Dilim - 30g)", Calories = 74, Protein = 3.9, Carbs = 12.3, Fat = 1.0 },
                new Food { Name = "Zeytinyağı (Yemek Kaşığı - 10ml)", Calories = 88, Protein = 0.0, Carbs = 0.0, Fat = 10.0 },
                new Food { Name = "Dana Biftek (100g)", Calories = 250, Protein = 26.0, Carbs = 0.0, Fat = 15.0 },
                new Food { Name = "Fıstık Ezmesi (Yemek Kaşığı - 15g)", Calories = 90, Protein = 3.8, Carbs = 3.0, Fat = 7.5 },
                new Food { Name = "Süzme Yoğurt (100g)", Calories = 97, Protein = 9.0, Carbs = 4.0, Fat = 5.0 }
            };

            context.Foods.AddRange(foods);
            context.SaveChanges();
        }
    }
}
