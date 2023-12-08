public class AboutUs
{
    public static string Location = "Wijnhaven 107, 3011 WN, Rotterdam, Zuid-Holland.";
    public static string RestaurantName = "Le Délice Français";
    public static string OpeningHours = "11 am - 10 pm.";
    public static string RestaurantEmail = "Delicemail@outlook.com";
    public static string PhoneNumber = "06113355749";
    public static List<string> SocialMedia = new List<string>() { "Restaurant: " };

    public static void travel()
    {

        Console.WriteLine();
        Console.WriteLine("Are you travelling with public transportation or by car?");
        Console.WriteLine("[1] public transportation, [2] car, [Enter] skip this part");
        string choice = Console.ReadLine();

        if (choice == "1")
        {
            publicTransport();
        }
        else if (choice == "2")
        {
            carTransport();
        }
    }

    public static void carTransport()
    {
        Console.WriteLine("Driving Directions to Wijnhaven 107, Rotterdam:");
        Console.WriteLine();

        // Directions from Blaak to Wijnhaven 107 (by car)
        Console.WriteLine("From Blaak:");
        Console.WriteLine("1. Start at Blaak Station.");
        Console.WriteLine("2. Head southeast on Blaak.");
        Console.WriteLine("3. Continue on Blaak until you reach Wijnhaven Street.");
        Console.WriteLine("4. Turn right onto Wijnhaven Street.");
        Console.WriteLine("5. Continue on Wijnhaven Street, and Wijnhaven 107 will be on your right.");
        Console.WriteLine();

        // Directions from Beurs to Wijnhaven 107 (by car)
        Console.WriteLine("From Beurs:");
        Console.WriteLine("1. Start at Beurs Metro Station.");
        Console.WriteLine("2. Head northeast on Westblaak Street.");
        Console.WriteLine("3. Continue on Westblaak Street until you reach Wijnhaven Street.");
        Console.WriteLine("4. Turn right onto Wijnhaven Street.");
        Console.WriteLine("5. Continue on Wijnhaven Street, and Wijnhaven 107 will be on your right.");
        Console.WriteLine();

        // Directions from Rotterdam Centraal to Wijnhaven 107 (by car)
        Console.WriteLine("From Rotterdam Centraal:");
        Console.WriteLine("1. Start at Rotterdam Centraal Station.");
        Console.WriteLine("2. Head southeast on Proveniersplein.");
        Console.WriteLine("3. Continue on Proveniersplein until you reach Schiekade.");
        Console.WriteLine("4. Turn right onto Schiekade.");
        Console.WriteLine("5. Continue on Schiekade until you reach Wijnhaven Street.");
        Console.WriteLine("6. Turn left onto Wijnhaven Street.");
        Console.WriteLine("7. Wijnhaven 107 will be on your left.");
        Console.WriteLine();
    }

    public static void publicTransport()
    {
        Console.WriteLine("Directions to Wijnhaven 107, Rotterdam:");
        Console.WriteLine();

        // Directions from Blaak to Wijnhaven 107
        Console.WriteLine("From Blaak:");
        Console.WriteLine("1. Start at Blaak Station.");
        Console.WriteLine("2. Take Metro Line A (Direction: Schiedam Centrum) or Metro B (Direction: Steendijkpolder) or Metro C (Direction: De Akkers).");
        Console.WriteLine("3. Get off at Beurs metro stop.");
        Console.WriteLine("4. Wijnhaven 107 is within walking distance from the Metro stop.");
        Console.WriteLine("5. See directions from Beurs.");
        Console.WriteLine();

        // Directions from Beurs to Wijnhaven 107
        Console.WriteLine("From Beurs:");
        Console.WriteLine("1. Start at Beurs Metro Station.");
        Console.WriteLine("2. Wijnhaven 107 is within walking distance from the tram stop.");
        Console.WriteLine("3. Walk to the on Mariteum Museum (can't miss it) and turn left.");
        Console.WriteLine("4. Continue walking and use the stairs when you see them.");
        Console.WriteLine("5. You should see the building right now.");
        Console.WriteLine("6. The building has a big logo in red.");
        Console.WriteLine("7. Make your way to the building, the entrance is right around the corner.");
        Console.WriteLine();

        // Directions from Rotterdam Centraal to Wijnhaven 107
        Console.WriteLine("From Rotterdam Centraal:");
        Console.WriteLine("1. Start at Rotterdam Centraal Station.");
        Console.WriteLine("2. Take Metro Line D (Direction: De Akkers) or Line E (Direction: Slinge).");
        Console.WriteLine("3. Get off at Beurs Metro Station.");
        Console.WriteLine("4. Walk southeast on Wijnhaven Street.");
        Console.WriteLine("5. Wijnhaven 107 is within walking distance from the Metro stop.");
        Console.WriteLine("6. See directions from Beurs.");
        Console.WriteLine();
    }
}

/*"-add option “info” to the interface to see the information
- add the address, contact information and opening/closing hours.
-add email, phone number and maybe social media to contact information.

"1. The restaurant's contact information, including phone number and email address, should be prominently displayed on the official website or mobile application's
""Contact Us"" or ""About Us"" page.
2. The restaurant's physical address, including street name and number, city, state, and postal code, should be clearly visible on the website or mobile application.
3. The contact information and address should be easily accessible from the main navigation menu or footer of the website or mobile application,
ensuring that customers can quickly find this information without unnecessary navigation or searching.
"
"*/

/*"-add option “info” to the interface to see the information
- add the address, contact information and opening/closing hours.
-add email, phone number and maybe social media to contact information.

"1. The restaurant's contact information, including phone number and email address, should be prominently displayed on the official website or mobile application's
""Contact Us"" or ""About Us"" page.
2. The restaurant's physical address, including street name and number, city, state, and postal code, should be clearly visible on the website or mobile application.
3. The contact information and address should be easily accessible from the main navigation menu or footer of the website or mobile application,
ensuring that customers can quickly find this information without unnecessary navigation or searching.
"
"*/