using System.Reflection.Metadata.Ecma335;

public static class SortFoodMenu
{
    private static int selectedOption = 1;
    private static int selectedTimeSlotOption = 1;
    public static List<MenuItem> menuItems = new List<MenuItem>();

    /*public static int SelectedTimeSlotOption
    {
        get { return selectedTimeSlotOption; }
        set { selectedTimeSlotOption = value; }
    }*/

    //private static List<string> ingredients = new List<string>();
    /*public static List<MenuItem> SortMenu()
    {
        List<MenuItem> sortedMenu = new List<MenuItem>();
        List<MenuItem> categories = new List<MenuItem>();
        //List<MenuItem> rawMenu = new List<MenuItem>();

        while (true)
        {
            sortedMenu.Clear();  ingredients.Clear(); 
            Console.WriteLine("What menu do you want to sort?");
            Console.WriteLine("1. Lunch, 2. Dinner");
            string choiceMenu = Console.ReadLine();

            if (choiceMenu == "1" || choiceMenu == "2")
            {
                //rawMenu = choiceMenu == "2" ? FoodMenu.GetDinnerMenu() : FoodMenu.GetLunchMenu();

                Console.WriteLine("What do you want to sort the menu on?");
                Console.WriteLine("These are the available options:");
                Console.WriteLine("1. ingredients, 2. price, 3. category, 4. exit menu and save your changes.");
                string sortChoice = Console.ReadLine();

                switch (sortChoice)
                {
                    case "1":
                        Console.WriteLine("Enter the ingredients (use comma):");
                        string ingredientRaw = Console.ReadLine().ToLower();
                        string[] ingredientsArray = ingredientRaw.Split(", ");
                        ingredients.AddRange(ingredientsArray);
                        sortedMenu.AddRange(SortIngredients(ingredients, choiceMenu));
                        break;

                    case "2":
                        Console.WriteLine("Enter the maximum price:");
                        if (double.TryParse(Console.ReadLine(), out double maxPrice))
                        {
                            sortedMenu.AddRange(SortPrice(choiceMenu, maxPrice));
                        }
                        else
                        {
                            Console.WriteLine("Invalid price input. Please enter a valid price.");
                        }
                        break;

                    case "3":
                        sortedMenu.AddRange(SortCategory(choiceMenu == "2" ? true : false));
                        break;

                    case "4":
                        Console.WriteLine("Exiting sorting menu. Returning to the main menu.");
                        return sortedMenu;

                    default:
                        Console.WriteLine("Invalid choice. Please enter 1, 2, 3, or 4");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid choice. Please enter 1 or 2");
            }

            return sortedMenu;
            
        }
    }*/

    public static List<MenuItem> cursoroptionMenu()
    {
        Console.CursorVisible = false;

        while (true)
        {
            menuItems.Clear();
            Console.Clear();
            DisplayMenuOptions();

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
                return HandleSelection();
            }
        }
    }

    private static void DisplayMenuOptions()
    {

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

            switch (i)
            {
                case 1:
                    Console.WriteLine(" Sort by Ingredients");
                    break;
                case 2:
                    Console.WriteLine(" Sort by Price");
                    break;
                case 3:
                    Console.WriteLine(" Sort by Category");
                    break;
                case 4:
                    Console.WriteLine(" Exit and Save");
                    break;
            }
        }

    }

    private static List<MenuItem> HandleSelection()
    {
        Console.Clear();
        List<string> ingredients = new List<string>();

        switch (selectedOption)
        {
            case 1:
                Console.WriteLine("Enter the ingredients (use comma):");
                string ingredientRaw = Console.ReadLine().ToLower();
                ingredients.AddRange(ingredientRaw.Split(", "));

                menuItems.AddRange(SortIngredients(ingredients, selectedTimeSlotOption.ToString()));
                break;

            case 2:
                Console.WriteLine("Enter the maximum price:");
                if (double.TryParse(Console.ReadLine(), out double maxPrice))
                {
                    menuItems.AddRange(SortPrice(selectedTimeSlotOption.ToString(), maxPrice));
                }
                else
                {
                    Console.WriteLine("Invalid price input. Please enter a valid price.");
                }
                break;

            case 3:
                menuItems.AddRange(SortCategory(selectedTimeSlotOption == 2 ? true : false));
                break;
            case 4:
                Environment.Exit(0);
                break;
        }
        return menuItems;
    }

    public static List<MenuItem> SortCategory(bool isDinner)
    {
        List<MenuItem> unsortedMenu = isDinner ? FoodMenu.GetDinnerMenu() : FoodMenu.GetLunchMenu();
        List<MenuItem> selectedCategories = new List<MenuItem>();

        Console.WriteLine("Categories:");
        foreach (var cat in FoodMenu.categoryEmojis)
        {
            Console.WriteLine($"{cat.Key}: {cat.Value}");
        }

        Console.WriteLine("Enter the category (1. Meat, 2. Chicken, 3. Fish, 4. Vegetarian, 5. Exit):");
        string[] categoryArray = Console.ReadLine().Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string category in categoryArray)
        {
            switch (category)
            {
                case "1":
                    selectedCategories.AddRange(unsortedMenu.FindAll(x => x.Category == "Meat"));
                    break;
                case "2":
                    selectedCategories.AddRange(unsortedMenu.FindAll(x => x.Category == "Chicken"));
                    break;
                case "3":
                    selectedCategories.AddRange(unsortedMenu.FindAll(x => x.Category == "Fish"));
                    break;
                case "4":
                    selectedCategories.AddRange(unsortedMenu.FindAll(x => x.Category == "Vegetarian"));
                    break;
                case "5":
                    return new List<MenuItem>(); // Return an empty list to indicate the user's exit choice.
                default:
                    Console.WriteLine("Input incorrect. Please enter 1, 2, 3, 4, or 5.");
                    break;
            }
        }

        return selectedCategories;
    }

    public static List<MenuItem> SortPrice(string menuType, double price)
    {
        List<MenuItem> finalMenu = new List<MenuItem>();
        Console.WriteLine(menuType);
        finalMenu = menuType == "2" ? FoodMenu.GetDinnerMenu() : FoodMenu.GetLunchMenu();

        List<MenuItem> sortedMenu = finalMenu.Where(x => x.Price <= price).ToList();
        sortedMenu = sortedMenu.OrderBy(x => x.Price).ToList();

        //in sortedmenu zitten alleen de prijzen als deel van het object
        return (sortedMenu);
    }

    public static List<MenuItem> SortIngredients(List<string> ingredients, string menuType)
    {
        List<MenuItem> unsortedItems = menuType == "2" ? FoodMenu.GetDinnerMenu() : FoodMenu.GetLunchMenu();
        List<MenuItem> temp = unsortedItems.Where(x => x.Ingredients.Select(i => i.ToLower()).Any(ingredient => ingredients.Contains(ingredient))).ToList();
        temp = temp.OrderBy(x => x.Price).ToList();
        return temp;
    }

    private static void DisplayTimeSlotMenuOptions()
    {
        for (int i = 1; i <= 3; i++)
        {
            if (i == selectedTimeSlotOption)
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
                    Console.WriteLine(" Lunch");
                    break;
                case 2:
                    Console.WriteLine(" Dinner");
                    break;
                case 3:
                    Console.WriteLine(" Exit");
                    break;
            }
        }
    }

    public static string cursoroptionTimeSlot()
    {
        Console.CursorVisible = false;

        while (true)
        {
            Console.Clear();
            DisplayTimeSlotMenuOptions();

            ConsoleKeyInfo keyInfo = Console.ReadKey();

            if (keyInfo.Key == ConsoleKey.UpArrow && selectedTimeSlotOption > 1)
            {
                selectedTimeSlotOption--;
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow && selectedTimeSlotOption < 3)
            {
                selectedTimeSlotOption++;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                return HandleTimeSlotSelection();
            }
        }
    }

    private static string HandleTimeSlotSelection()
    {
        Console.Clear();

        switch (selectedTimeSlotOption)
        {
            case 1:
                return "Lunch";
            case 2:
                return "Dinner";
            case 3:
                break;
        }

        return "";
    }

}