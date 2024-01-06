




using System.Reflection.Metadata.Ecma335;

public static class ReservationManagement
{
    public static Account CurrentUser { get; set; } = Restaurant.AdminAccounts[0];
    private static bool IsAdmin { get { return CurrentUser is AdminAccount; } }
    private static List<Reservation> ReservationsOfUser
    { get { return GetReservationsOfUser(); } }


    public static void Display()
    {
        string accountType = IsAdmin ? "Admin" : "Customer";
        List<string> RMOptions = new List<string>() // RM == ReservationManager
        {
            "View Reservations",
            "Add Reservation",
            "Update Reservation by number",
            "Cancel Reservation by number",
            "Search Reservations",
            $"Back to {accountType} Dashboard"
        };
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Reservation Manager:");
            int selectedOption = MenuSelector.RunMenuNavigator(RMOptions);
            switch (selectedOption)
            {
                case 0:
                    Console.Clear();
                    ViewReservations();
                    break;
                case 1:
                    Console.Clear();
                    AddReservation();
                    break;
                case 2:
                    Console.Clear();
                    UpdateReservation();
                    break;
                case 3:
                    Console.Clear();
                    CancelReservation();
                    break;
                case 4:
                    Console.Clear();
                    SearchReservations();
                    break;
                case 5:
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                    break;
            }

            Restaurant.UpdateRestaurantFiles();
            Console.WriteLine("\n[Press any key to return to the Reservation Manager menu.]");
            Console.ReadKey();
        }
    }

    private static List<Reservation> GetReservationsOfUser()
    {
        List<Reservation> reservations;
        if (IsAdmin)
        {
            reservations = Restaurant.Reservations;
        }
        else
        {
            CustomerAccount customer = (CurrentUser as CustomerAccount)!;
            reservations = customer.Reservations;
        }
        return reservations;
    }

    private static void ViewReservations()
    {
        Console.WriteLine("Reservation Overview:\n");

        if (ReservationsOfUser.Count == 0)
        {
            Console.WriteLine("There are no reservations yet");
            return;
        }
        foreach (Reservation reservation in ReservationsOfUser)
        {
            Console.WriteLine(reservation.ToString());
            Console.WriteLine();
        }
    }

    private static void AddReservation()
    {
        Console.WriteLine("Add Reservation manualy");
        Console.WriteLine();
        ReservationSystem.Reservate(IsAdmin);
    }

    private static void UpdateReservation()
    {
        Console.WriteLine("Update Reservation\n");
        if (ReservationsIsEmpty(ReservationsOfUser)) { return; }
        int reservationNumber = GetReservationNumber();
        Reservation? reservation = ReservationsOfUser.FirstOrDefault(reservation => reservation.ReservationNumber == reservationNumber);
        if (reservation == null)
        {
            Console.WriteLine($"No reservation with the reservation number \"{reservationNumber}\" was found.");
            return;
        }
        List<string> updateOptions = new List<string>()
            {
                "Amount of people",
                "Date",
                "Timeslot",
                "Done"
            };
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"Current data for reservation {reservationNumber}:");
            Console.WriteLine(reservation.ToString());
            Console.WriteLine("\nUpdate:");
            int selectedOption = MenuSelector.RunMenuNavigator(updateOptions);
            switch (selectedOption)
            {
                case 0:
                    Console.Clear();
                    UpdateAmountOfPeople(reservation);
                    break;
                case 1:
                    Console.Clear();
                    Console.WriteLine("Enter the new reservation date (dd-MM-yyyy):");
                    reservation.Date = ReservationSystem.GetReservationDate();
                    break;
                case 2:
                    Console.Clear();
                    reservation.TimeSlot = ReservationSystem.GetTimeslot();
                    break;
                case 3:
                    Console.WriteLine("Reservation updated!");
                    return;
                default:
                    break;
            }
            Console.Clear();
            Console.WriteLine("Updated reservation:\n");
            Console.WriteLine(reservation.ToString() + "\n");
            Console.WriteLine("\n[Press any key to continue updating this reservation.]");
            Console.ReadKey();
        }
    }


    public static void UpdateAmountOfPeople(Reservation reservation)
    {
        while (true)
        {
            Console.WriteLine("Enter the new amount of people that are coming:");
            int numberOfPeople = ReservationSystem.GetNumberOfPeople(IsAdmin);
            if (numberOfPeople > reservation.SelectedTable.Capacity)
            {
                Console.WriteLine("The table you reservated does not have enough seats.\n");
                continue;
            }
            break;
        }
    }

    private static void CancelReservation()
    {
        Console.WriteLine("Cancel Reservation\n");
        if (ReservationsIsEmpty(ReservationsOfUser)) { return; }
        int reservationNumber = GetReservationNumber();

        Reservation? reservation = ReservationsOfUser.FirstOrDefault(reservation => reservation.ReservationNumber == reservationNumber);
        if (reservation == null)
        {
            Console.WriteLine($"No reservation with the reservation number \"{reservationNumber}\" was found.");
            return;
        }
        Console.WriteLine();
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        if (reservation.Date < today)
        {
            Console.WriteLine("It is not possible to cancel a reservation that has already passed.");
            return;
        }
        Console.WriteLine(reservation.ToString() + "\n");
        Console.WriteLine("Are you sure you want to cancel this reservation?");
        int selectedOption = MenuSelector.RunMenuNavigator(new List<string>() { "Yes", "No" });
        if (selectedOption == 0)
        {
            bool removed = Restaurant.Reservations.Remove(reservation);
            if (!removed)
            { throw new Exception($"Reservation \"{reservationNumber}\" wasn't removed succesfully"); }
        }
        else
        { Console.WriteLine("This reservation will not be cancelled."); }
    }

    private static void SearchReservationByNumber()
    {
        int reservationNumber = GetReservationNumber();
        Reservation reservation = ReservationsOfUser.FirstOrDefault(reservation => reservation.ReservationNumber == reservationNumber);
        if (reservation != null)
        {
            Console.WriteLine(reservation.ToString());
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine($"No reservation with the reservation number \"{reservationNumber}\" was found.");
        }
    }

    private static void SearchReservationByDate()
    {
        DateOnly date;
        Console.Write("Enter date (dd-MM-yyyy): ");
        string dateStr = Console.ReadLine().Trim();
        bool correctDateFormat = DateOnly.TryParseExact(dateStr, "d-M-yyyy", out date);
        if (correctDateFormat)
        {
            List<Reservation> reservationsOnDate = ReservationsOfUser.FindAll(reservation => reservation.Date == date);
            if (reservationsOnDate.Count != 0)
            {
                foreach (Reservation r in reservationsOnDate)
                {
                    Console.WriteLine(r.ToString());
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine($"No reservation were made on the date \"{date}\".");
            }
        }
    }

    private static void SearchReservations()
    {
        Console.WriteLine("Search Reservation\n");
        if (ReservationsIsEmpty(ReservationsOfUser)) { return; }
        Console.WriteLine("Search by:");
        int selectedOption = MenuSelector.RunMenuNavigator(new List<string>() { "Reservation number", "Date" });
        if (selectedOption == 0)
        {
            SearchReservationByNumber();
        }
        else
        {
            SearchReservationByDate();
        }
    }

    private static bool ReservationsIsEmpty(List<Reservation> reservations)
    {
        if (reservations.Count == 0)
        {
            Console.WriteLine("\nThere are no reservations yet.");
            return true;
        }
        return false;
    }

    private static int GetReservationNumber()
    {
        int reservationNumber;
        Console.WriteLine("Enter reservation number:");
        while (true)
        {
            string input = Console.ReadLine();
            if (!input.All(char.IsDigit))
            { Console.WriteLine("Reservation numbers must only contain digits."); continue; }
            else if (input.Length != 5)
            { Console.WriteLine("Reservations numbers must have 5 digits."); continue; }
            reservationNumber = Convert.ToInt32(input);
            break;
        }
        return reservationNumber;
    }
}