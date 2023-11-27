public class Deal //make this class abstract
{
    //public int ID;
    public string Name;
    public string Description;
    public double DiscountFactor;
    //public bool CurrentlyRunning; will be used by the admin later
    //public DateOnly FromDate;
    //public DateOnly ToDate;
    public Deal(string name, string description, double discountFactor)
    {
        Name = name;
        Description = description;
        DiscountFactor = discountFactor;
    }

    public void DisplayDealIsAplied()
    {
        Console.WriteLine($"The {Name} has just been added to your reservation!");
        Console.WriteLine(Description);
    }

    public bool PartyDealIsApplicable(int amountOfPeople)
    {
        return amountOfPeople >= 10;
    }
}
