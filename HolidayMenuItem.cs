public class HolidayMenuItem
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string[] Ingredients { get; set; }
    public double Price { get; set; }
    public string Timeslot { get; set; }
    public string Category { get; set; }
    public string[] PotentialAllergens { get; set; }
    public string? Icon;
    public string Holiday;
    private static readonly string[] Allergens = new string[] { "Milk", "Eggs", "Peanuts", "Tree Nuts", "Fish", "Shellfish", "Soybeans", "Wheat", "Sesame" };

    public HolidayMenuItem(string name, string description, List<string> ingredients, string timeslot, double price,List<string> potentialAllergens, string icon, string holiday)
    {
        Name = name;
        Description = description;
        Ingredients = ingredients.ToArray();
        Price = price;
        Timeslot = timeslot;
        Category = icon;
        PotentialAllergens = potentialAllergens.ToArray();
        Holiday = holiday;
        Icon = icon switch  // Easier then regular switch 
        {
            "Meat" => "🥩",
            "Chicken" => "🍗",
            "Fish" => "🐟",
            "Vegetarian" => "🥦",
            _ => "",
        };
    }
}