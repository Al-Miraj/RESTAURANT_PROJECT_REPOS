
public static class OptionMenu // Made class static so LoginSystem and Dashboard files don't rely on instances
{
    public static bool IsUserLoggedIn = false;
    public static Account? CurrentUser;
    public static Dashboard? UserDashboard;

    public static void RunMenu()
    {
        List<string> menuOptions = new List<string>()
        {
            "Reservation",
            "About Us",
            "Contact Us",
            "Menu",
            "Drinks",
            "Deals",
            "Login/Register",
            "Exit"
        };
        while (true)
        {
            Console.Clear();
            int selectedOption = MenuSelector.RunMenuNavigator(menuOptions);
            Console.Clear();
            switch (selectedOption)
            {
                case 0:
                    ReservationSystem.RunSystem();
                    break;
                case 1:
                    About.RestaurantInformation();
                    break;
                case 2:
                    Contact.ContactInformation();
                    AboutUs.travel();
                    break;
                case 3:
                    HolidayMenu.getHoliday();
                    break;
                case 4:
                    DrinksMenu.Display();
                    break;
                case 5:
                    Restaurant.DisplayDeals();
                    break;
                case 6:
                    if (IsUserLoggedIn)
                        UserDashboard!.RunDashboardMenu();
                    else
                        LoginSystem.Start();
                    break;
                case 7:
                    Restaurant.UpdateRestaurantFiles();
                    Console.WriteLine("Goodbye! Thank you for visiting.");
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                    Console.ReadLine();
                    break;
            }

            Console.WriteLine("\n\n[Press any key to return to the main menu.]");
            Console.ReadKey();
        }
    }
}