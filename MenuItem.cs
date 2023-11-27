using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Security;

public class MenuItem
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string[] Ingredients { get; set; }
    public double Price { get; set; }
    public string Timeslot { get; set; }
    public string Category { get; set; }
    public string? Icon;
    private static readonly string[] Allergens = new string[] { "Milk", "Eggs", "Peanuts", "Tree Nuts", "Fish", "Shellfish", "Soybeans", "Wheat", "Sesame" };

    public MenuItem(string name, string description, List<string> ingredients, double price, string timeslot, string icon)
    {
        Name = name;
        Description = description;
        Ingredients = ingredients.ToArray();
        Price = price;
        Timeslot = timeslot;
        Category = icon;
        Icon = icon switch  // Easier then regular switch 
        {
            "Meat" => "🥩",
            "Chicken" => "🍗",
            "Fish" => "🐟",
            "Vegetarian" => "🥦",
            _ => "",
        };
    }

    public string AllergensInfo // Property of the allergens
    {
        get
        {
            // Iterating over the lowercased Allergens array to find match and 
            var lowerAllergens = Array.ConvertAll(Allergens, s => s.ToLower());
            var allergensInfo = Ingredients.Where(ingredient => lowerAllergens.Contains(ingredient.ToLower()));
            if (!allergensInfo.Any())
            {
                return "No known allergens.";
            }
            else
            {
                return $"Allergens: {string.Join(", ", allergensInfo)}";
            }
        }
    }
}
