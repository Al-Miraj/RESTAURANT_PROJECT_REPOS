public class Drinks
{
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string Category { get; set; }
    public double Alcohol { get; set; }

    public Drinks(string name, string description, double price, string category, double alcohol)
    {
        Name = name;
        Description = description;
        Price = price;
        Category = category;
        Alcohol = alcohol;
    }

}