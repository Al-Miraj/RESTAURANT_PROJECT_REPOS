﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.ComponentModel.Design;
using System.Diagnostics.Tracing;
using System.Text.Json;

public static class ReservationSystem // Made class static so loginsystem and dashboard don't rely on instances
{
    public static Random Random = new Random();

    public static void RunSystem()
    {
        
        Reservate(false);
    }

    public static void Reservate(bool isAdmin)
    {
        int reservatorID = GetCustomerID();

        Console.Write("Enter the number of people in your group: ");
        int numberOfPeople = GetNumberOfPeople(isAdmin);

        // checks if party deal is applicable and turns addPartyDeal to true if it is.
        bool addPartyDeal = false;
        PartyDeal partyDeal = (Restaurant.GetDealByName("Party Deal") as PartyDeal)!;
        if (partyDeal != null)
        {
            bool isApplicable = partyDeal.DealIsApplicable(numberOfPeople);
            if (isApplicable)
            {
                partyDeal.DisplayDealIsAplied();
                addPartyDeal = true;
                Console.WriteLine("\n [Press any key to continue]");
                Console.ReadKey();
                Console.Clear();
            }
        } 

        Console.Clear();
        Console.Write("Enter a date (dd-mm-yyyy): ");
        DateOnly date = GetReservationDate();

        Console.Clear();
        Console.WriteLine("Choose your timeslot:");
        string timeslot = GetTimeslot();

        bool restaurantIsFull = IsFullyReservated(GetAvailability(date, timeslot), numberOfPeople);
        if (restaurantIsFull)
        {
            WaitingListOffer();
            return;
        }

        Table? table = GetChosenTable(numberOfPeople, date, timeslot);
        if (table != null)
        {
            int reservationNumber = GenerateReservationNumber();
            Reservation reservation = new Reservation(reservatorID, reservationNumber, numberOfPeople, date, timeslot, table, table.TablePrice);
            if (addPartyDeal)
            {
                reservation.DealsApplied.Add(partyDeal);
                reservation.DiscountFactor = partyDeal.DiscountFactor;
            }
            if (OptionMenu.CurrentUser is CustomerAccount cAccount)
            {
                cAccount.Reservations.Add(reservation);
            }
            Restaurant.Reservations.Add(reservation);
            DisplayReservationDetails(reservation);
        }
        else
        {
            Console.WriteLine("You weren't able to finish the reservation.");
            return;
        }

        Restaurant.UpdateRestaurantFiles();
    }


    public static int GetCustomerID()  // in ReservationSystem or somewhere else?
    {
        // if a customer is logged in and reservating, the reservation is made under their ID
        // if a non logged in customer or admin is reservating, the reservation is made under the CustomerId 0
        if (OptionMenu.IsUserLoggedIn)
        {
            if (OptionMenu.CurrentUser is CustomerAccount cAccount)
            {
                return cAccount.ID;
            }
        }
        return 0; // todo: revisit
    }

    public static int GetNumberOfPeople(bool isAdmin)
    {
        int numberOfPeople;
        bool hasCorrectFormat;
        bool IsSmallerThan0;
        bool IsBiggerThan6;

        do
        {
            string number = Console.ReadLine().Trim();
            (hasCorrectFormat, numberOfPeople) = GetValidatedNumberFormat(number);
            IsSmallerThan0 = numberOfPeople <= 0;
            IsBiggerThan6 = numberOfPeople > 6;
            if (!hasCorrectFormat)
                Console.WriteLine("Invalid input. Please enter a valid number of people like: 1, 4, 6, etc");
            else if (IsSmallerThan0)
                Console.WriteLine("Invalid input. Please enter a number greater than 0.");
            else if (IsBiggerThan6 && !isAdmin)
                Console.WriteLine("Our biggest table has 6 seats. Enter a number smaller than 6 or contact us for more information.");
            else if (IsBiggerThan6 && isAdmin)
                break;
            }
        while (!hasCorrectFormat || IsSmallerThan0 || IsBiggerThan6);
        return numberOfPeople;
    }

    public static bool NumInRange(int start, int end, int num) => num >= start && num <= end;

    public static (bool, int) GetValidatedNumberFormat(string input)
    {
        int output;
        bool HasCorrectFormat = int.TryParse(input, out output);
        return (HasCorrectFormat, output);
    }

    public static (bool, DateOnly) GetValidatedReservationDateFormat(string date)
    {
        DateOnly reservationDate;
        bool isCorrectFormat = DateOnly.TryParseExact(date, "d-M-yyyy", out reservationDate);
        return (isCorrectFormat, reservationDate);
    }

    public static bool ReservationDateIsWithinRange(DateOnly reservationDate)
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Today);
        DateOnly yearFromNow = today.AddYears(1);
        bool dateHasNotPassed = today <= reservationDate;
        bool dateIsWithinYearFromNow = reservationDate < yearFromNow;

        return dateHasNotPassed && dateIsWithinYearFromNow;
    }

    public static DateOnly GetReservationDate()
    {
        DateOnly reservationDate;
        bool formatIsCorrect;
        bool dateIsWithinRange;

        do
        {
            string date = Console.ReadLine().Trim();

            (formatIsCorrect, reservationDate) = GetValidatedReservationDateFormat(date);
            dateIsWithinRange = ReservationDateIsWithinRange(reservationDate);
            if (!formatIsCorrect)
            {
                Console.WriteLine($"Invalid date format '{date}'. Please enter a date in the dd-mm-yyyy format.");
            }
            else if (!dateIsWithinRange)
            {
                Console.WriteLine($"Invalid date '{date}'. Please enter a date that has not already passed and is not more than a year from today.");
            }
        }
        while (!formatIsCorrect || !dateIsWithinRange);

        return reservationDate;
    }

    public static string GetTimeslot()
    {
        List<string> timeslots = new List<string>()
        {   "Lunch (12:00-14:00)",
            "The Afternoon (14:00-16:00)",
            "The Evening (16:00-18:00)",
            "Dinner (18:00-20:00)",
            "Late Dinner (20:00-22:00)"
        };
        
        int selectedOption = MenuSelector.RunMenuNavigator(timeslots );
        return timeslots[selectedOption];
    }

    public static List<int> GetAvailability(DateOnly date, string timeslot) // todo: improve method + var name: changed name to GetAvailability()
    {
        // find all reservations made at the date and time of the new reservation
        // select all the tables numbers those reservation were made at
        // convert IEnumerable to List
        List<int> availability = Restaurant.Reservations
            .FindAll(reservation => reservation.Date == date && reservation.TimeSlot == timeslot)
            .Select(reservation => reservation.SelectedTable.TableNumber)
            .ToList();
        return availability;
    }

    private static void WaitingListOffer()
    {
        Console.Clear();
        Console.WriteLine("We're sorry. We don't have any space for that date and timeslot.\n");
        Console.WriteLine("Would you like to receive an email in case a table becomes available?");
        List<string> options = new() { "Yes", "Reserve another timeslot/date", "No, thanks" };
        int option = MenuSelector.RunMenuNavigator(options);
        if (option == 2)
        {
            return;
        }
        else if (option == 1)
        {
            Reservate(false);
            return;
        }
        Console.Clear();
        if (OptionMenu.IsUserLoggedIn)
        { 
            Console.WriteLine($"An email will be send to {OptionMenu.CurrentUser!.Email} if a table is free"); 
            return; 
        }
        else
        {
            Console.Clear();
            string email = LoginSystem.GetAccountEmail();
            Console.WriteLine("Thank you for using the waiting list!");
            Console.WriteLine($"An email will be send to {email} if a table is free.");
            // maybe add function to send email when the Reservated tables at the right date and timeslot are below 15;
            /*SendEmailIfAvailable(email);*/
            return;
        }
    }

    private static bool IsFullyReservated(List<int> reservatedTablesNumbers, int numberOfPeople)
    {
        // Filter out all tables that don't have enough seats.
        List<Table> tablesWithCapacity = Restaurant.Tables.FindAll(table => table.Capacity >= numberOfPeople);
        List<int> tableNumbersWithCapacity = tablesWithCapacity.Select(table => table.TableNumber).ToList();
        List<int> availableTableNumbers = tableNumbersWithCapacity.Except(reservatedTablesNumbers).ToList();
        return availableTableNumbers.Count <= 0;
    }

    


    private static Table? GetChosenTable(int numberOfPeople, DateOnly date, string timeslot)
    {
        List<int> reservatedTablesNumbers = GetAvailability(date, timeslot);
        Console.CursorVisible = false;
        ConsoleKeyInfo keyInfo;

        (int x, int y) currentTableCoordinate = (1, 1);
        Table? chosenTable = null;

        while (true)
        {
            Console.Clear();
            PrintBarDisplay();
            PrintTablesMapClean(currentTableCoordinate, reservatedTablesNumbers, numberOfPeople);
            PrintTableInfo(currentTableCoordinate);

            

            //TableSelectionFeedback(selectedTable, numberOfPeople);
            keyInfo = Console.ReadKey();
            if (keyInfo.Key == ConsoleKey.UpArrow)
            { currentTableCoordinate = GetNewCoordinate(currentTableCoordinate, "Up"); }
            else if (keyInfo.Key == ConsoleKey.DownArrow)
            { currentTableCoordinate = GetNewCoordinate(currentTableCoordinate, "Down"); }
            else if (keyInfo.Key == ConsoleKey.LeftArrow)
            { currentTableCoordinate = GetNewCoordinate(currentTableCoordinate, "Left"); }
            else if (keyInfo.Key == ConsoleKey.RightArrow)
            { currentTableCoordinate = GetNewCoordinate(currentTableCoordinate, "Right"); }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                chosenTable = Restaurant.Tables.Find(table => table.Coordinate == currentTableCoordinate)!;
                if (!reservatedTablesNumbers.Contains(chosenTable.TableNumber)) // the table has not been reservated yet
                {
                    if (numberOfPeople <= chosenTable.Capacity) // the table has enough seats
                    {
                        break;
                    }
                }
            }
        }

        return chosenTable;
    }

    public static void PrintTablesMapClean((int, int) currentTableCoordinate, List<int> reservatedTableNumbers, int numberOfPeople)
    {
        int windowWidth = Console.WindowWidth / 100 * 75; // 75% of terminal width
        int xConsolePosition = Console.CursorLeft;
        int yConsolePosition = Console.CursorTop;
        int firstTableInRowIndex = 0;

        foreach (int numOfTablesInRow in Restaurant.NumOfTablesPerRow)
        {
            // Get Table objects of the table in the current row
            List<Table> tablesInRow = Restaurant.Tables.GetRange(firstTableInRowIndex, numOfTablesInRow);

            foreach (Table table in tablesInRow)
            {
                // Calculate the spacing between each table
                int totalTablesWidth = table.Width * numOfTablesInRow;
                int totalSpaces = windowWidth - totalTablesWidth;
                int spacesBetweenAmount = totalSpaces / (numOfTablesInRow + 1);
                xConsolePosition += spacesBetweenAmount;

                // print the user cursor and the table display
                if (table.Coordinate == currentTableCoordinate)
                {
                    Console.SetCursorPosition(xConsolePosition, yConsolePosition);
                    Console.WriteLine("   \\/   ");
                }
                SetColor(table, reservatedTableNumbers, numberOfPeople);
                table.PrintAt((xConsolePosition, yConsolePosition + 1));
                Console.ResetColor();
                xConsolePosition += table.Width;
            }

            // update the Console Cursor position for the next row
            if (numOfTablesInRow == 0)
            {
                PrintEntranceDisplay();
                yConsolePosition += 4;  // 4 == height of the door display.
            }
            else
            {
                yConsolePosition += 5 + 2;  // 5 == height of the table display, 2 == spaces between each row
            }

            xConsolePosition = 0;
            firstTableInRowIndex += numOfTablesInRow;
        }
    }

    private static void PrintEntranceDisplay()
    {
        Console.WriteLine("\n");
        Console.WriteLine("\\");
        Console.WriteLine(" \\~");
        Console.WriteLine();
        Console.WriteLine(" /~");
        Console.WriteLine("/");
    }

    public static void PrintBarDisplay()
    {
        int windowWidth = Console.WindowWidth / 100 * 75;

        int barSeatAmount = 8;
        string barSeatStr = "()";
        int totalBarSeatWidth = barSeatStr.Length * barSeatAmount;
        int totalSpaces = windowWidth - totalBarSeatWidth;
        int spacesBetweenAmount = totalSpaces / (barSeatAmount + 1);

        string barTitle = "B A R";
        int barCounterLength = spacesBetweenAmount * (barSeatAmount + 1) + totalBarSeatWidth;
        string barCounter = new string('=', barCounterLength);

        Console.SetCursorPosition((barCounterLength - barTitle.Length) / 2, Console.CursorTop);
        Console.WriteLine(barTitle);

        Console.WriteLine(barCounter);

        for (int barSeat = 1; barSeat <= barSeatAmount; barSeat++)
        {
            Console.SetCursorPosition(Console.CursorLeft + spacesBetweenAmount, Console.CursorTop);
            Console.Write(barSeatStr);
        }
        Console.WriteLine("\n\n");
    }

    private static void SetColor(Table table, List<int> reservatedTableNumbers, int numberOfPeople)
    {
        if (reservatedTableNumbers.Contains(table.TableNumber))
        { Console.ForegroundColor = ConsoleColor.Red; }
        else if (table.Capacity < numberOfPeople)
        { Console.ForegroundColor = ConsoleColor.DarkGray; }
        else
        { Console.ForegroundColor = ConsoleColor.DarkGreen; }
    }

    private static (int, int) GetNewCoordinate((int x, int y) coordinate, string direction)
    {
        HashSet<(int, int)> validCoordinates = new HashSet<(int, int)>()
        {
            (1,1), (2,1), (3,1), (4,1),
            (1,2), (2,2), (3,2), (4,2),
            (1,3), (2,3), (3,3), (4,3), (5,3),
                   (2,4),        (4,4),
        };

        (int, int) coordinateIndirection = (0, 0);
        (int, int) newCoordinate;
        if (direction == "Up")
        { coordinateIndirection = (coordinate.x, coordinate.y - 1); }
        else if (direction == "Down")
        { coordinateIndirection = (coordinate.x, coordinate.y + 1); }
        else if (direction == "Left")
        { coordinateIndirection = (coordinate.x - 1, coordinate.y); }
        else if (direction == "Right")
        { coordinateIndirection = (coordinate.x + 1, coordinate.y); }

        bool containsValue = validCoordinates.TryGetValue(coordinateIndirection, out newCoordinate);
        if (containsValue) { return newCoordinate; }

        List<(int, int)> coordinatesToRedirect = new List<(int, int)>() { (1, 4), (3, 4), (5, 4), (5, 2) };
        if (coordinatesToRedirect.Contains(coordinateIndirection))
        {
            return coordinateIndirection.Item2 == 2 ? (4, 2) : (coordinateIndirection.Item1, 3);
        }

        return coordinate;
    }

    private static void PrintTableInfo((int, int) tableCoordinate)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        (int x, int y) currentCursorPosition = Console.GetCursorPosition();
        int windowWidth = Console.WindowWidth / 100 * 90;
        int windowHeight = Console.WindowHeight * 25 / 100;

        Table table = Restaurant.Tables.Find(table => table.Coordinate == tableCoordinate)!;
        List<string> infos = new List<string>()
        {
            $"table number: {table.TableNumber}",
            $"Price: €{table.TablePrice:0.00}",
            $"Seats: {table.Capacity}",
        };

        PrintLegend((windowWidth, 5));

        int xCursor = windowWidth;
        int yCursor = windowHeight;
        foreach (string info in infos)
        {
            Console.SetCursorPosition(xCursor, yCursor);
            Console.Write(info);
            yCursor++;
        }
        Console.SetCursorPosition(currentCursorPosition.x, currentCursorPosition.y);
    }

    private static void PrintLegend((int x, int y) coordinate)
    {
        var legend = new[]
        {
            (ConsoleColor.Red, "Red: Already booked"),
            (ConsoleColor.DarkGray, "Gray: Not enough seats"),
            (ConsoleColor.Green, "Green: Available")
        };

        foreach (var (color, text) in legend)
        {
            Console.SetCursorPosition(coordinate.x, coordinate.y);
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
            coordinate.y++;
        }
    }

    private static void DisplayReservationDetails(Reservation R)
    {
        Table T = R.SelectedTable;
        string numOfPeople = R.NumberOfPeople > 1 ? $"{R.NumberOfPeople} guests" : $"{R.NumberOfPeople} guest";

        Console.Clear();
        Console.WriteLine("R E S E R V A T I O N   D E T A I L S\n");
        if (OptionMenu.IsUserLoggedIn)
        {
            Console.WriteLine($"You ({R.CustomerID}) reservated Table {T.TableNumber} for {numOfPeople} on {R.Date} during {R.TimeSlot}.");
        }
        else
        {
            Console.WriteLine($"You reservated Table {T.TableNumber} for {numOfPeople} on {R.Date} during {R.TimeSlot}.");
        }
        Console.WriteLine($"Your reservation number: {R.ReservationNumber}");
        Console.WriteLine($"Deals applied:");
        if (R.DealsApplied.Count == 0)
        {
            Console.WriteLine("  > No deals were applied to this reservation.");
        }
        else
        {
            foreach (Deal deal in R.DealsApplied)
            {
                Console.WriteLine($"  > {deal.Name} ({deal.DiscountFactor * 100}% discount)");
            }
        }
        Console.WriteLine($"Total Price: €{R.GetTotalPrice():0.00}");
        
    }

    /// <summary>
    /// Generates returns a random 32-bit signed integer between 10 000 and 99 999 (inclusive).
    /// </summary>
    /// <returns></returns>
    public static int GenerateReservationNumber()  // todo: make it so that there is no chance for 2 reservations to have the same number
    {
        return Random.Next(10000, 100000); // 
    }
}