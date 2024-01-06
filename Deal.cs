
using System.Xml.Serialization;

[XmlInclude(typeof(PartyDeal))]
public class Deal //make this class abstract
{
    //public int ID;
    public string Name;
    public string Description;
    public double DiscountFactor;
    public Deal(string name, string description, double discountFactor)
    {
        Name = name;
        Description = description;
        DiscountFactor = discountFactor;
    }
    public Deal()
    {
        Name = "";
        Description = "";
        DiscountFactor = 0;
    }

    public void DisplayDealIsAplied()
    {
        Console.WriteLine($"\nThe {Name} has just been added to your reservation!");
        Console.WriteLine(Description + "\n");
    }
}