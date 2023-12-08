using Newtonsoft.Json;
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
        

        Console.Write("Enter a date (dd-mm-yyyy): ");
        DateOnly date = GetReservationDate();
        Console.Clear();

        Console.WriteLine("Choose your timeslot:");
        string timeslot = GetTimeslot();

        Table table = GetChosenTable(numberOfPeople);
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
        bool IsIncorrectFormat;
        bool IsSmallerThan0;
        bool IsBiggerThan6;

        do
        {
            string number = Console.ReadLine().Trim();
            IsIncorrectFormat = !int.TryParse(number, out numberOfPeople); //true if the format is incorrect
            IsSmallerThan0 = numberOfPeople <= 0;
            IsBiggerThan6 = numberOfPeople > 6;
            if (IsIncorrectFormat)
                Console.WriteLine("Invalid input. Please enter a valid number of people like: 1, 4, 6, etc");
            else if (IsSmallerThan0)
                Console.WriteLine("Invalid input. Please enter a number greater than 0.");
            else if (IsBiggerThan6 && !isAdmin)
                Console.WriteLine("Our biggest table has 6 seats. Enter a number smaller than 6 or contact us for more information.");
            else if (IsBiggerThan6 && isAdmin)
                break;
        }
        while (IsIncorrectFormat || IsSmallerThan0 || IsBiggerThan6);
        return numberOfPeople;
    }

    public static DateOnly GetReservationDate()
    {
        DateOnly reservationDate;
        DateOnly today = DateOnly.FromDateTime(DateTime.Today);
        DateOnly yearFromNow = today.AddYears(1);
        bool formatIsIncorrect;
        bool dateHasPassed;
        bool dateIsMoreThanYearFromNow;

        do
        {
            string date = Console.ReadLine().Trim();

            // formatIsIncorrect is true if the format is incorrect
            formatIsIncorrect = !DateOnly.TryParseExact(date, "d-M-yyyy", out reservationDate);
            dateHasPassed = reservationDate < today;
            dateIsMoreThanYearFromNow = reservationDate > yearFromNow;
            if (formatIsIncorrect)
            {
                Console.WriteLine($"Invalid date format '{date}'. Please enter a date in the dd-mm-yyyy format.");
            }
            else if (dateHasPassed)
            {
                Console.WriteLine($"Invalid date '{date}'. Please enter a date that has not already passed.");
            }
            else if (dateIsMoreThanYearFromNow)
            {
                Console.WriteLine($"Invalid date '{date}'. Please enter a date that has not more than a year from today.");
            }

        }
        while (formatIsIncorrect || dateHasPassed);

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

    public static void DisplayTablesMap()
    {
        string door = "Entrance";
        string aisle = "Main Aisle";

        // First row of tables (2 seats)
        DisplayTableRange(1, 4); // Tables 1-4

        Console.WriteLine("  {0}  ", door.PadRight(10)); // Entrance representation

        // Second row of tables (4 seats)
        DisplayTableRange(5, 8); // Tables 5-8

        Console.WriteLine("  {0}  ", new string(' ', 10)); // Space representing an aisle

        // Third row of tables (2 seats)
        DisplayTableRange(9, 12); // Tables 9-12

        Console.WriteLine("  {0}  ", aisle.PadRight(10)); // Main Aisle

        // Fourth row of tables (6 seats)
        DisplayTableRange(13, 15); // Tables 13-15
    }

    private static void DisplayTableRange(int startTable, int endTable)
    {
        // Display a range of tables
        for (int tableNumber = startTable; tableNumber <= endTable; tableNumber++)
        {
            Table table = Restaurant.Tables.FirstOrDefault(t => t.TableNumber == tableNumber);
            if (table != null)
            {
                if (table.IsReservated)
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ForegroundColor = ConsoleColor.Green;

                // Display a single table with the appropriate number of seats
                if (tableNumber <= 4 || (tableNumber >= 9 && tableNumber <= 12)) // 2-seat tables
                    Console.Write($"  [ {table.TableNumber} ]  ");
                else if (tableNumber >= 5 && tableNumber <= 8) // 4-seat tables
                    Console.Write($"  [ {table.TableNumber} ]-[ {table.TableNumber} ]  ", table.TableNumber);
                else // 6-seat tables
                    Console.Write($"  [ {table.TableNumber} ]-[ {table.TableNumber} ]-[ {table.TableNumber} ]  ");
            }
            Console.ResetColor();
        }
        Console.WriteLine("\n");
    }


    //private static (int, int) DetermineMaxCoordinates(List<Table> tables)
    //{
    //    int maxX = tables.Max(t => t.Coordinate.Item1);
    //    int maxY = tables.Max(t => t.Coordinate.Item2);
    //    return (maxX, maxY);
    //}

    public static Table GetChosenTable(int numberOfPeople)
    {
        Console.CursorVisible = false;
        ConsoleKeyInfo keyInfo;

        int xc = 1;
        int yc = 1;
        Table selectedTable;

        while (true)
        {
            Console.Clear();
            DisplayTablesMap();
            selectedTable = ShowSelectedTable(xc, yc, numberOfPeople);

            if (selectedTable != null)
            {
                TableSelectionFeedback(selectedTable, numberOfPeople);
                keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.UpArrow && yc > 1)
                {
                    yc--;
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow && yc < 5)
                {
                    yc++;
                }
                else if (keyInfo.Key == ConsoleKey.LeftArrow && xc > 1)
                {
                    xc--;
                }
                else if (keyInfo.Key == ConsoleKey.RightArrow && xc < 3)
                {
                    xc++;
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    if (!selectedTable.IsReservated) // the table has not been reservated yet
                    {
                        if (numberOfPeople <= selectedTable.Capacity) // the table has enough seats
                        {
                            selectedTable.IsReservated = true; // table at that coordinate has now been reservated
                            JsonFileHandler.WriteToFile(Restaurant.Tables, Restaurant.TablesJsonFileName);  // todo: remove this
                            break;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Sorry! We are booked!");
                break;
            }

        }


        return selectedTable;
    }

    public static void TableSelectionFeedback(Table selectedTable, int numberOfPeople)
    {
        if (selectedTable.IsReservated)
        {
            Console.WriteLine($"\nTable {selectedTable.TableNumber} has already been reservated. try another!");
        }
        else if (numberOfPeople > selectedTable.Capacity)
        {
            Console.WriteLine($"\nTable {selectedTable.TableNumber} does not have enough seats for you. Try another!");
        }
    }

    public static Table ShowSelectedTable(int xc, int yc, int numberOfPeople)
    {
        Table selectedTable = null;
        int availableTablesCount = 0;

        // colors tables that are already booked red
        // tables with too little seats gray
        // puts [] around the selected table
        foreach (Table table in Restaurant.Tables)
        {
            string ws = table.TableNumber < 10 ? " " : ""; // ws = whitespace to format the table options
            string wst = table.TablePrice < 10 ? " " : ""; // wst = whitespace table to format the table options

            if (table.IsReservated)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                availableTablesCount++;
            }
            else if (numberOfPeople > table.Capacity)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                availableTablesCount++;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green; // Green for available
            }

            string tablePrice = table.TablePrice.ToString("F2");
            if (table.Coordinate.Item1 == xc && table.Coordinate.Item2 == yc)
            {
                Console.Write($"[Table {table.TableNumber} {ws} {table.Capacity}   Seats ${tablePrice}]  {wst}");
                selectedTable = table;
            }
            else
            {
                Console.Write($" Table {table.TableNumber} {ws} {table.Capacity}   Seats ${tablePrice}   {wst}");
            }
            Console.ResetColor();

            if (table.Coordinate.Item1 % 3 == 0)
            {
                Console.WriteLine(); //creates new row after every 3 tables
            }
        }

        if (availableTablesCount >= Restaurant.Tables.Count) // None of the tables are available for this reservation
        {
            return null;
        }
        return selectedTable;
    }

    public static void DisplayReservationDetails(Reservation R)
    {
        Table T = R.SelectedTable;
        string numOfPeople = R.NumberOfPeople > 1 ? $"{R.NumberOfPeople} guests" : $"{R.NumberOfPeople} guest";

        Console.Clear();
        Console.WriteLine("R E S E R V A T I O N   D E T A I L S\n");
        Console.WriteLine($"You ({R.CustomerID}) reservated Table {T.TableNumber} for {numOfPeople} on {R.Date} during {R.TimeSlot}.");
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
        Console.WriteLine($"Total Price: {R.GetTotalPrice()}");
        
    }

    public static int GenerateReservationNumber()
    {
        return Random.Next(10000, 100000); // Generates a random number between 10 000 and 99 999 (inclusive).
    }
}