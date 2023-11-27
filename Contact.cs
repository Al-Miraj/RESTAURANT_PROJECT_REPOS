public class Contact : AboutUs
{
    public static void ContactInformation()
    {
        Console.Clear();
        Console.WriteLine($"Restaurant: {RestaurantName}");
        Console.WriteLine($"Phonenumber: {PhoneNumber}");
        Console.WriteLine($"Email: {RestaurantEmail}");
        Console.WriteLine($"Location: {Location}");
        Console.WriteLine($"Openinghours: {OpeningHours}");
        /*foreach (string item in SocialMedia)
        {
            Console.WriteLine(item);
        }*/
    }
}
