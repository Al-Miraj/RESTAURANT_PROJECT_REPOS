public class Dashboard
{
    public Account CurrentUser { get; set; }

    public Dashboard(Account account)
    {
        CurrentUser = account;
    }
//next
    public void RunDashboardMenu()
    {
        bool isAdmin = CurrentUser is AdminAccount;
        Console.Clear();
        Console.WriteLine($"Welcome {CurrentUser.Name}!");
        Console.WriteLine("This is your dashboard.");

        List<string> dashboardOptions = new List<string>()
        {
            "Reservation Management",
            isAdmin ? "Reservation Overview" : "Order History",
            isAdmin ? "Customer Management" : "Reservation Overview",
            "Exit to main menu",
            "Log out"
        };

        int selectedOption = MenuSelector.RunMenuNavigator(dashboardOptions);

        switch (selectedOption)
        {
            case 0:
                ReservationManager();
                break;
            case 1:
                if (isAdmin)
                { ReservationOverview(); }
                else
                { Console.Clear(); OrderHistory(); }
                break;
            case 2:
                if (isAdmin)
                { CustomerManager(); }
                else
                { ReservationOverview(); }
                break;
            case 3:
                OptionMenu.RunMenu();
                break;
            case 4:
                LoginSystem.Logout();
                return;
            default:
                break;
        }

        Console.WriteLine("\n[Press any key to return to your dashboard.]");
        Console.ReadKey();
        RunDashboardMenu();
    }


    private void ReservationManager()
    {
        ReservationManagement.CurrentUser = CurrentUser;
        ReservationManagement.Display();
    }

    private void CustomerManager()
    {
        CustomerManagement.CurrentAdmin = (CurrentUser as AdminAccount)!;
        CustomerManagement.Display();
    }

    private void OrderHistory()
    {
        Console.WriteLine("Your past reservations:\n");
        List<Reservation> reservations = ((CustomerAccount)CurrentUser).GetReservations();  // todo check if works
        if (reservations.Count == 0)
        {
            Console.WriteLine("You have not reservated at this restaurant yet.");
            return;
        }
        foreach (Reservation reservation in reservations)
        {
            Console.WriteLine(reservation.ToString());
            Console.WriteLine();
        }
    }
    private void ReservationOverview()
    {
        Console.Clear();
        Console.WriteLine("Reservation Overview\n");

        // Example values, replace these with appropriate values for your application
        (int, int) currentTableCoordinate = (1, 1);
        List<int> reservatedTableNumbers = new List<int>(); // Replace with actual values
        int numberOfPeople = 4; // Replace with actual value

        // Display the visual map of the restaurant with reserved/available tables
        ReservationSystem.PrintTablesMapClean(currentTableCoordinate, reservatedTableNumbers, numberOfPeople);

        Console.WriteLine("\n[Press any key to return to your dashboard.]");
        Console.ReadKey();
        RunDashboardMenu();
    }

}

