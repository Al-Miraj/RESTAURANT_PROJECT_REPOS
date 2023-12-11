using Newtonsoft.Json;

public static class Restaurant
{
    public static string DealsXmlFileName = "Deals.xml";
    public static string TablesJsonFileName = "Tables.json";
    public static string ReservationsXmlFileName = "Reservations.xml";
    public static string AccountsXmlFileName = "Accounts.xml";
    public static List<Account> Accounts = InitializeAccounts();
    public static List<AdminAccount> AdminAccounts { get { return GetAdminAccounts(); } }
    public static List<CustomerAccount> CustomerAccounts { get { return GetCustomerAccounts(); } }
    public static List<Deal> Deals = InitializeDeals();
    public static List<Table> Tables = InitializeTables();
    public static List<Reservation> Reservations = InitializeReservations();

    // i.e first row has 4, the last has 2 tables in it
    public static List<int> NumOfTablesPerRow = new List<int>() { 4, 4, 0, 5, 2 };


    private static List<Account> InitializeAccounts()
    {
        List<Account> accounts;
        if (File.Exists(AccountsXmlFileName))
        {
            accounts = XmlFileHandler.ReadFromFile<Account>(AccountsXmlFileName);
        }
        else
        {
            // there is always 1 default admin account
            AdminAccount admin = new AdminAccount(1, "Admin", "admin@work.com", "Admin-123");
            accounts = new List<Account>() { admin };
            XmlFileHandler.WriteToFile(accounts, AccountsXmlFileName);
        }
        return accounts;
    }

    public static List<AdminAccount> GetAdminAccounts()
    {
        List<AdminAccount> adminAccounts = new List<AdminAccount>();
        foreach (Account account in Accounts)
        {
            if (account is AdminAccount admin)
            {
                adminAccounts.Add(admin);
            }
        }
        return adminAccounts;
    }

    public static List<CustomerAccount> GetCustomerAccounts()
    {
        List<CustomerAccount> customerAccounts = new List<CustomerAccount>();
        foreach (Account account in Accounts)
        {
            if (account is CustomerAccount customer)
            {
                customerAccounts.Add(customer);
            }
        }
        return customerAccounts;
    }

    private static List<Deal> InitializeDeals()
    {
        List<Deal> deals;
        if (File.Exists(DealsXmlFileName))
        {
            deals = XmlFileHandler.ReadFromFile<Deal>(DealsXmlFileName);
        }
        else
        {
            // Party Deal is always running
            string dealName = "Party Deal";
            string dealDescription = "For groups of 6 people or more, you get a 10% discount."; //todo: discount on what tho?
            double dealDiscountFactor = 0.10; // 10% discount
            int minAmountGuests = 6;
            deals = new List<Deal>()
            {
                new PartyDeal(dealName, dealDescription, dealDiscountFactor, minAmountGuests)
            };
            XmlFileHandler.WriteToFile(deals, DealsXmlFileName);
        }
        return deals;
    }

    private static List<Table> InitializeTables()
    {
        List<Table> tables;

        if (File.Exists(TablesJsonFileName))
        {
            tables = JsonFileHandler.ReadFromFile<Table>(TablesJsonFileName);
        }
        else
        {
            tables = new List<Table>()
            {
                new Table(1, (1, 1), 2, 7.50, false),
                new Table(2, (2, 1), 2, 7.50, false),
                new Table(3, (3, 1), 2, 7.50, false),
                new Table(4, (4, 1), 2, 7.50, false),
                new Table(5, (1, 2), 2, 7.50, false),
                new Table(6, (2, 2), 2, 7.50, false),
                new Table(7, (3, 2), 2, 7.50, false),
                new Table(8, (4, 2), 2, 7.50, false),
                new Table(9, (1, 3), 4, 10.0, false),
                new Table(10, (2, 3), 4, 10.0, false),
                new Table(11, (3, 3), 4, 10.0, false),
                new Table(12, (4, 3), 4, 10.0, false),
                new Table(13, (5, 3), 4, 10.0, false),
                new Table(14, (2, 4), 6, 15.0, false),
                new Table(15, (4, 4), 6, 15.0, false),
            };
            JsonFileHandler.WriteToFile(tables, TablesJsonFileName);
        }
        return tables;
    }

    private static List<Reservation> InitializeReservations()
    {
        List<Reservation> reservations = new List<Reservation>();

        if (File.Exists(ReservationsXmlFileName))
        {
            reservations.AddRange(XmlFileHandler.ReadFromFile<Reservation>(ReservationsXmlFileName));
        }
        else
        {
            XmlFileHandler.WriteToFile(reservations, ReservationsXmlFileName);
        }
        foreach (Reservation reservation in reservations)
        {
            Account account = Accounts.FirstOrDefault(account => reservation.CustomerID == account.ID);
            if (account is CustomerAccount cAccount)
            {
                cAccount.Reservations.Add(reservation);
            }
        }
        return reservations;
    }

    public static void UpdateRestaurantFiles()
    {
        XmlFileHandler.WriteToFile(Accounts, AccountsXmlFileName);
        XmlFileHandler.WriteToFile(Deals, DealsXmlFileName);
        JsonFileHandler.WriteToFile(Tables, TablesJsonFileName);
        XmlFileHandler.WriteToFile(Reservations, ReservationsXmlFileName);
    }  // todo: check the references for potential excessive updating

    public static Deal? GetDealByName(string dealName)
    {
        foreach (Deal deal in Deals)
        {
            if (deal.Name == dealName)
            {
                return deal;
            }
        }
        return null;
    } // todo maybe have a deal handler or put this in Deal.cs

    public static void DisplayDeals()
    {
        Console.Clear();

        //prints the title "DEALS"
        Console.WriteLine(" ___   ____   __    _     __ ");
        Console.WriteLine("| | \\ | |_   / /\\  | |   ( (` ");
        Console.WriteLine("|_|_/ |_|__ /_/--\\ |_|__ _)_) ");
        Console.WriteLine("-------------------------------\n");
        Console.WriteLine("Deals that we are currently offering!\n");

        if (Deals.Count <= 0)
        {
            Console.WriteLine("We are currently offering 0 Deals. Come back later or contact us for more information!");
            return;
        }
        else
        {
            foreach (Deal deal in Deals)
            {
                Console.WriteLine(deal.Name);
                Console.WriteLine(deal.Description);
                Console.WriteLine("===========================================\n");
            }
        }
    } // todo maybe have a deal handler or put this in Deal.cs

}