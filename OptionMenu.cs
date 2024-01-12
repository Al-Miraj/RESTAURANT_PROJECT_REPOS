﻿
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

            Console.ForegroundColor = ConsoleColor.Red;

            // Inside the while loop so that the name gets displayed each time the options menu is shown
            Console.WriteLine("    __            ____       ___              ______                            _     ");
            Console.WriteLine("   / /   ___     / __ \\___  / (_)_______     / ____/________ _____  _________ _(_)____");
            Console.WriteLine("  / /   / _ \\   / / / / _ \\/ / / ___/ _ \\   / /_  / ___/ __ `/ __ \\/ ___/ __ `/ / ___/");
            Console.WriteLine(" / /___/  __/  / /_/ /  __/ / / /__/  __/  / __/ / /  / /_/ / / / / /__/ /_/ / (__  ) ");
            Console.WriteLine("/_____/\\___/  /_____/\\___/_/_/\\___/\\___/  /_/   /_/   \\__,_/_/ /_/\\___/\\__,_/_/____/ \n ");
            Console.WriteLine();
            Console.ResetColor();


            int selectedOption = MenuSelector.RunMenuNavigator(menuOptions);
            Console.Clear();
            switch (selectedOption)
            {
                case 0:
                    ReservationSystem.RunSystem();
                    Console.WriteLine("\n\n[Press any key to return to the main menu.]");
                    Console.ReadKey();
                    break;
                case 1:
                    About.RestaurantInformation();
                    Console.WriteLine("\n\n[Press any key to return to the main menu.]");
                    Console.ReadKey();
                    break;
                case 2:
                    Contact.ContactInformation();
                    AboutUs.travel();
                    Console.WriteLine("\n\n[Press any key to return to the main menu.]");
                    Console.ReadKey();
                    break;
                case 3:
                    FoodDrinkEntryPoint foodDrinkEntryPoint = new FoodDrinkEntryPoint();
                    foodDrinkEntryPoint.GetCorrectMenu();
                    break;
                case 4:
                    DrinksMenu drinksMenu = new DrinksMenu();
                    while (true)
                    {
                        drinksMenu.SelectOption();
                        string option = drinksMenu.HandleSelection();
                        if (option == "")
                            break;
                        drinksMenu.PrintCorrectMenu(option);
                        
                    }
                    break;
                case 5:
                    Restaurant.DisplayDeals();
                    Console.WriteLine("\n\n[Press any key to return to the main menu.]");
                    Console.ReadKey();
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
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                    Console.WriteLine("\n\n[Press any key to return to the main menu.]");
                    Console.ReadKey();
                    Console.ReadLine();
                    break;
            }
        }
    }
}