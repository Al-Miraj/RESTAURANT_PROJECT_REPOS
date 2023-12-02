using Newtonsoft.Json;

public static class DrinksMenu
{
    private static int selectedOption = 1;

    private static List<Drinks> drinksMenu = new List<Drinks>();

    public static List<Drinks>? LoadDrinksData()
    {
        try
        {
            using StreamReader reader = new StreamReader("Drinks.json");
            string json = reader.ReadToEnd();
            var items = JsonConvert.DeserializeObject<List<Drinks>>(json);
            return items;
        }
        catch (JsonReaderException)
        { return null; }
        catch (FileNotFoundException)
        { return null; }
        catch (UnauthorizedAccessException)
        { return null; }
    }

    public static void Display()
    {
        Console.CursorVisible = false;
        drinksMenu = LoadDrinksData();
        PrintInfo(GetDefaultMenu());

        while (true)
        {
            Console.Clear();
            DisplayMenuOptions();

            ConsoleKeyInfo keyInfo = Console.ReadKey();

            if (keyInfo.Key == ConsoleKey.UpArrow && selectedOption > 1)
            {
                selectedOption--;
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow && selectedOption < 7)
            {
                selectedOption++;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                if (selectedOption == 7) { return; }
                HandleSelection();
            }
        }
    }

    private static void DisplayMenuOptions()
    {
        for (int i = 1; i <= 7; i++)
        {
            if (i == selectedOption)
            {
                Console.Write(">");
            }
            else
            {
                Console.Write(" ");
            }

            switch (i)
            {
                case 1:
                    Console.WriteLine(" Drink Menu");
                    break;
                case 2:
                    Console.WriteLine(" Soda");
                    break;
                case 3:
                    Console.WriteLine(" Wine");
                    break;
                case 4:
                    Console.WriteLine(" Whiskey");
                    break;
                case 5:
                    Console.WriteLine(" Cognac");
                    break;
                case 6:
                    Console.WriteLine(" Beer");
                    break;
                case 7:
                    Console.WriteLine(" Exit");
                    break;
            }
        }
    }

    private static void HandleSelection()
    {
        switch (selectedOption)
        {
            case 1:
                PrintInfo(GetDefaultMenu());
                drinksMenu.Clear();
                break;
            case 2:
                GetSoda();
                PrintInfo(drinksMenu);
                drinksMenu.Clear();
                break;
            case 3:
                GetWine();
                PrintInfo(drinksMenu);
                drinksMenu.Clear();
                break;
            case 4:
                GetWhiskey();
                PrintInfo(drinksMenu);
                drinksMenu.Clear();
                break;
            case 5:
                GetCognac();
                PrintInfo(drinksMenu);
                drinksMenu.Clear();
                break;
            case 6:
                GetBeer();
                PrintInfo(drinksMenu);
                drinksMenu.Clear();
                break;
            case 7:
                break;
        }
    }

    public static void PrintInfo(List<Drinks> drinks, bool KeyContinue = true)
    {
        Console.Clear();
        string CurrentCategory = "";

        foreach (var drink in drinks)
        {
            Console.WriteLine($"Name: {drink.Name}");
            Console.WriteLine($"Description: {drink.Description}");
            Console.WriteLine($"Price: {drink.Price}");
            Console.WriteLine($"Category: {drink.Category}");
            Console.WriteLine($"Alcohol percentage: {drink.Alcohol}");
            Console.WriteLine();

            if (CurrentCategory != drink.Category)
            {
                Console.WriteLine("------------------------------------------------------------------------------------");
                Console.WriteLine();
                CurrentCategory = drink.Category;
            }
        }

        if (KeyContinue)
        {
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }
    }

    public static List<Drinks>? GetDefaultMenu()
    {
        var sodas = GetSoda(true);
        var wine = GetWine(true);
        var whiskey = GetWhiskey(true);
        var cognac = GetCognac(true);
        var beer = GetBeer(true);

        List<Drinks> defaultMenu = new List<Drinks>();
        if (sodas != null)
        {
            defaultMenu.AddRange(sodas);
        }
        if (wine != null)
        {
            defaultMenu.AddRange(wine);
        }
        if (whiskey != null)
        {
            defaultMenu.AddRange(whiskey);
        }
        if (cognac != null)
        {
            defaultMenu.AddRange(cognac);
        }
        if (beer != null)
        {
            defaultMenu.AddRange(beer);
        }

        return defaultMenu;
    }

    public static List<Drinks>? GetSoda(bool returnable = false)
    {
        var sodaDrinks = LoadDrinksData()?.FindAll(x => x.Category == "Soda");
        if (sodaDrinks != null)
        {
            drinksMenu.AddRange(sodaDrinks);

            if (returnable)
            {
                return sodaDrinks;
            }

            else
            {
                return null;
            }
        }

        return null;
    }

    public static List<Drinks>? GetWine(bool returnable = false)
    {
        var wineDrinks = LoadDrinksData()?.FindAll(x => x.Category == "Wine");
        if (wineDrinks != null)
        {
            drinksMenu.AddRange(wineDrinks);

            if (returnable)
            {
                return wineDrinks;
            }
        }

        return null;
    }

    public static List<Drinks>? GetWhiskey(bool returnable = false)
    {
        var whiskeyDrinks = LoadDrinksData()?.FindAll(x => x.Category == "Whiskey");
        if (whiskeyDrinks != null)
        {
            drinksMenu.AddRange(whiskeyDrinks);

            if (returnable)
            {
                return whiskeyDrinks;
            }
        }

        return null;
    }

    public static List<Drinks>? GetCognac(bool returnable = false)
    {
        var cognacDrinks = LoadDrinksData()?.FindAll(x => x.Category == "Cognac");
        if (cognacDrinks != null)
        {
            drinksMenu.AddRange(cognacDrinks);

            if (returnable)
            {
                return cognacDrinks;
            }
        }

        return null;
    }

    public static List<Drinks>? GetBeer(bool returnable = false)
    {
        var beerDrinks = LoadDrinksData()?.FindAll(x => x.Category == "Beer");
        if (beerDrinks != null)
        {
            drinksMenu.AddRange(beerDrinks);

            if (returnable)
            {
                return beerDrinks;
            }
        }

        return null;
    }
}