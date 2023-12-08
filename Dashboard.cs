public class Dashboard
{
    public Account CurrentUser { get; set; }

    public Dashboard(Account account)
    {
        CurrentUser = account;
    }

    public void RunDashboardMenu()
    {
        bool isAdmin = CurrentUser is AdminAccount;
        Console.Clear();
        Console.WriteLine($"Welcome {CurrentUser.Name}!");
        Console.WriteLine("This is your dashboard.");

        List<string> dashboardOptions = new List<string>()
        { isAdmin ? "Reservation Management" : "Order History", // todo: ipv order history misschien view profile details ofzo?
          isAdmin ? "Customer Management" : "Reservation Manager",
          "Exit to main menu",
          "Log out"
        };
        int selectedOption = MenuSelector.RunMenuNavigator(dashboardOptions);
        switch (selectedOption)
        {
            case 0:
                if (isAdmin)
                { ReservationManager(); }
                else
                { Console.Clear(); OrderHistory(); }
                break;
            case 1:
                if (isAdmin)
                { CustomerManager(); }
                else
                { ReservationManager(); }
                break;
            case 2:
                OptionMenu.RunMenu(); // loop warning
                break;
            case 3:
                LoginSystem.Logout();
                return;
            default:
                break;
        }

        Console.WriteLine("\n[Press any key to return to the your dashboard.]");
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
            Console.WriteLine(); //test
        }
    }
}
