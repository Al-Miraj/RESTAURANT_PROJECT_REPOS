using System.Runtime.CompilerServices;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

public static class ReservationManagement
{

    public static AdminAccount CurrentAdmin { private get; set; }

    public static void Display()
    {
        int selectedOption = 1;

        while (true)
        {
            Console.Clear();

            // Highlight the currently selected option
            for (int i = 1; i <= 6; i++)
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
                        Console.WriteLine(" View Reservations");
                        break;
                    case 2:
                        Console.WriteLine(" Add Reservation");
                        break;
                    case 3:
                        Console.WriteLine(" Update Reservation by number");
                        break;
                    case 4:
                        Console.WriteLine(" Cancel Reservation by number");
                        break;
                    case 5:
                        Console.WriteLine(" Search Reservations");
                        break;
                    case 6:
                        Console.WriteLine(" Back to Admin Dashboard");
                        break;
                }
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey();

            if (keyInfo.Key == ConsoleKey.UpArrow && selectedOption > 1)
            {
                selectedOption--;
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow && selectedOption < 6)
            {
                selectedOption++;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                if (selectedOption == 6)
                {
                    return;
                }
                HandleSelection(selectedOption);
            }
        }



        static void HandleSelection(int option)
        {
            Console.Clear();

            switch (option)
            {
                case 1:
                    ViewReservations();
                    break;
                case 2:
                    DateOnly date = DateOnly.ParseExact("20-12-2023", "dd-MM-yyyy", null);
                    Reservation reservation = new(1, 3, date, "Dinner", new Table(1, (1, 1), 2, 7.50, false)
                   {
                       TableNumber = 3,
                       Capacity = 1
                   });
                    ReservationSystem.reservations.Add(reservation);
                    break;
                case 3:
                    UpdateReservation();
                    break;
                case 4:
                    CancelReservation();
                    break;
                case 5:
                    SearchReservations();
                    break;
                case 6:
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                    Console.ReadLine();
                    break;
            }

            Console.WriteLine("\nPress any key to return to the main menu.");
            Console.ReadKey();
        }

        /*while (true)
        {
            Console.WriteLine("Reservation Management");
            Console.WriteLine("1. View Reservations");
            Console.WriteLine("2. Add Reservation");
            Console.WriteLine("3. Update Reservation by number");
            Console.WriteLine("4. Cancel Reservation by number");
            Console.WriteLine("5. Search Reservations");
            Console.WriteLine("6. Back to Admin Dashboard");

            switch (int.TryParse(Console.ReadLine(), out int choice) ? choice : -1) 
            {
                case 1:
                    
                    break;
                case 2:
                    
                    break;
                case 3:
                    
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    return;
            }
        }*/



    }

    private static void ViewReservations()
    {
        var reservations = CurrentAdmin.GetReservations();
        Console.WriteLine("Reservation Overview");
        Console.WriteLine();
        if (reservations == null)
        {
            Console.WriteLine("There are no reservations yet");
            Console.ReadLine();
            return;
        }
        foreach (var reservation in reservations)
        {
            Console.WriteLine(reservation.ToString());
            Console.WriteLine();
        }
        Console.ReadLine();
    }

    private static void UpdateReservation()
    {
        var reservations = CurrentAdmin.GetReservations();
        Console.WriteLine("Update Reservation");
        Console.WriteLine();
        Console.Write("Enter reservation number or -1 to return: ");
        int id = int.Parse(Console.ReadLine()!);
        if (id == -1)
        {
            return;
        }
        var reservation = reservations.FirstOrDefault(reservation => reservation.ReservationNumber == id);
        if (reservation != null)
        {
            ReservationSystem.reservations.Remove(reservation);
            Console.WriteLine("Old data for reservation:");
            Console.WriteLine(reservation.ToString());
            Console.WriteLine("New data for reservation:");
            Console.Write("Amount of people: ");
            int amount = int.Parse(Console.ReadLine()!);
            Console.Write("Date (dd-MM-yyyy): ");
            string dateStr = Console.ReadLine()!.Trim();
            DateOnly date = DateOnly.ParseExact(dateStr, "dd-MM-yyyy", null);
            Console.Write("Timeslot (Lunch, dinner): ");
            string timeslot = Console.ReadLine()!;
            var new_reservation = new Reservation(reservation.ReservationNumber, amount, date, timeslot, reservation.SelectedTable);
            ReservationSystem.reservations.Add(new_reservation);
            Console.WriteLine("Reservation updated!");
            Console.ReadLine();
        }

    }

    private static void CancelReservation()
    {
        var reservations = CurrentAdmin.GetReservations();
        Console.WriteLine("Cancel Reservation");
        Console.WriteLine();
        Console.Write("Enter reservation number or -1 to exit: ");
        int id = int.Parse(Console.ReadLine()!);
        if (id == -1)
            return;
        var reservation = reservations.FirstOrDefault(reservation => reservation.ReservationNumber == id);
        if (reservation != null)
        {
            ReservationSystem.reservations.Remove(reservation);
            Console.WriteLine("Reservation cancelled!");
            Console.ReadLine();
        }
    }

    private static void SearchReservations()
    {
        var reservations = CurrentAdmin.GetReservations();
        Console.WriteLine("Search Reservation");
        Console.WriteLine();
        Console.WriteLine("Search by: ");
        Console.WriteLine("1. Reservation number");
        Console.WriteLine("2. Date");
        switch (int.TryParse(Console.ReadLine(), out int choice) ? choice : -1)
        {
            case 1:
                Console.Write("Enter reservation number: ");
                int id = int.Parse(Console.ReadLine()!);
                var reservation = reservations.FirstOrDefault(reservation => reservation.ReservationNumber == id);
                if (reservation != null)
                {
                    Console.WriteLine(reservation.ToString());
                    Console.WriteLine();
                    Console.ReadLine();
                }

                return;
            case 2:
                Console.Write("Enter date (dd-MM-yyyy): ");
                string date = Console.ReadLine()!;
                var reservationsOnDate = reservations.FindAll(reservation => reservation.Date.ToString() == date);
                if (reservationsOnDate != null)
                {
                    foreach (var r in reservationsOnDate)
                    {
                        Console.WriteLine(r.ToString());
                        Console.WriteLine();
                    }
                    Console.ReadLine();
                }
                return;
        }

    }
}
