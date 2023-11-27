public class Dashboard
{
    public Account CurrentUser { get; set; }

    public Dashboard(Account account)
    {
        CurrentUser = account;
    }

    int selectedOption = 1;

    public void Display()
    {
        bool isAdmin = CurrentUser is AdminAccount;
        while (true)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine($"Welcome {CurrentUser.Email}!");
            Console.WriteLine("This is your dashboard.");

            // Highlight the currently selected option
            for (int i = 1; i <= 4; i++)
            {
                if (i == selectedOption)
                {
                    Console.Write(">");
                }
                else
                {
                    Console.Write(" ");
                }

                // Display text labels for options
                switch (i)
                {
                    case 1:
                        Console.WriteLine(!isAdmin ? " Order History" : " Reservation Management");
                        break;
                    case 2:
                        Console.WriteLine(!isAdmin ? " Cancel Reservation" : " Customer Management");
                        break;
                    case 3:
                        Console.WriteLine(" Log out");
                        break;
                    case 4:
                        Console.WriteLine(" Exit to main menu");
                        break;
                }
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey();

            if (keyInfo.Key == ConsoleKey.UpArrow && selectedOption > 1)
            {
                selectedOption--;
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow && selectedOption < 4)
            {
                selectedOption++;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                HandleSelection(selectedOption, isAdmin);
            }
        }
    }

    void HandleSelection(int option, bool isAdmin)
    {
        Console.Clear();

        switch (option)
        {
            case 1:
                if (!isAdmin)
                    OrderHistory();
                else
                    ReservationManager();
                break;
            case 2:
                if (!isAdmin)
                    CancelCustomerReservation();
                else
                    CustomerManager();
                break;
            case 3:
                LoginSystem.Logout(CurrentUser);
                Menu x = new Menu();
                x.RunMenu();
                break;
            case 4:
                Menu y = new Menu();
                y.RunMenu();
                break;
            default:
                return;
        }
    }


    private void ReservationManager()
    {
        ReservationManagement.CurrentAdmin = (CurrentUser as AdminAccount)!;
        ReservationManagement.Display();
    }

    private void CustomerManager()
    {
        CustomerManagement.CurrentAdmin = (CurrentUser as AdminAccount)!;
        CustomerManagement.Display();
    }

    private void OrderHistory()
    {
        Console.WriteLine("Your past reservations:");
        foreach (var reservation in CurrentUser.GetReservations())
        {
            Console.WriteLine(reservation.ToString());
            Console.WriteLine();
        }
    }

    private void CancelCustomerReservation()
    {
        Console.WriteLine("Which reservation would you like to cancel?");
        var reservations = CurrentUser.GetReservations();
        foreach (var item in reservations)
        {
            Console.WriteLine(item.ToString());
            Console.WriteLine();
        }
        do
        {
            Console.Write("Enter Reservation Number: ");
            int id = int.Parse(Console.ReadLine()!);
            var reservation_ids = reservations.Select(reservation => reservation.ReservationNumber);
            if (reservation_ids.Contains(id))
            {
                var reservation = reservations.FirstOrDefault(reservation => reservation.ReservationNumber == id);
                ReservationSystem.reservations.Remove(reservation!);
                Console.WriteLine("Reservation cancelled.");
                Console.ReadLine();
                return;
            }
            Console.WriteLine("Reservation not found.");
        } while (true);
    }
}
