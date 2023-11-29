﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;

public static class FoodMenu
{
    public static List<MenuItem> MenuItems = new();

    public static List<MenuItem>? LoadFoodMenuData()
    {
        try
        {
            using StreamReader reader = new StreamReader("items.json");
            string json = reader.ReadToEnd();
            var items = JsonConvert.DeserializeObject<List<MenuItem>>(json);
            return items;
        }
        catch (JsonReaderException)
        {
            Console.WriteLine("reader exp");
            return null; }
        catch (FileNotFoundException)
        {
            Console.WriteLine("file not not found");
            return null; }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Unauthorized acces");
            return null; }
    }

    public static void Display()
    {
        MenuItems = GetDefaultMenu();
        while (true)
        {
            Console.WriteLine(); Console.WriteLine("==================================================================================================================");
            for (int i = 0; i < MenuItems.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {MenuItems[i].Name,-20} {MenuItems[i].Price,74}");
                if (MenuItems[i].Description.Length > 52)
                {
                    Console.WriteLine($"{MenuItems[i].Description.Substring(0, 50)}- {MenuItems[i].AllergensInfo,60}");
                    Console.WriteLine(MenuItems[i].Description.Substring(50, MenuItems[i].Description.Length - 50));
                }
                else
                {
                    Console.WriteLine($"{MenuItems[i].Description,-50} {MenuItems[i].AllergensInfo,60}");
                }
                Console.WriteLine($"Ingredients: {string.Join(", ", MenuItems[i].Ingredients)}");
                Console.WriteLine();
            }
            Console.WriteLine("==================================================================================================================");
            Console.WriteLine();
            Console.WriteLine("Would you like to see the other menu?");
            Console.WriteLine("1. Lunch");
            Console.WriteLine("2. Dinner");
            Console.WriteLine("3. Exit");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    MenuItems = GetLunchMenu();
                    break;
                case "2":
                    MenuItems = GetDinnerMenu();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please enter 1, 2, or 3.");
                    break;
            }
        }
    }

    static List<MenuItem> GetDefaultMenu()
    {

<<<<<<< Updated upstream
        var allItems = FoodMenu.LoadFoodMenuData();
=======
        switch (selectedOption)
        {
            case 1:
                PrintInfo(GetLunchMenu(), "Lunch");
                break;
            case 2:
                PrintInfo(GetDinnerMenu(), "Dinner");
                break;
            case 3:
                string timeSlot = FilterFoodMenu.cursoroptionTimeSlot();
                PrintInfo(FilterFoodMenu.cursoroptionMenu(), timeSlot);
                //PrintInfo(SortFoodMenu.menuItems, SortFoodMenu.SelectedTimeSlotOption == 2 ? "Dinner" : "Lunch");
                break;
            case 4:
                Environment.Exit(0);
                break;
        }
    }

    public static (string timeslot, List<MenuItem> timeslotMenu) GetDefaultMenu()
    {
        string x = "";
        var allItems = LoadFoodMenuData();
>>>>>>> Stashed changes
        List<MenuItem> timeslotMenu = new List<MenuItem>();

        var dt = SetTime();
        DateOnly date = dt.date;
        TimeOnly time = dt.time;



        if (ifDinner(time) && allItems != null)
        {
            var dinnerMenuItems = allItems.FindAll(x => x.Timeslot == "Dinner");
            timeslotMenu.AddRange(dinnerMenuItems);

        }
        else
        {
            if (allItems != null)
            {
                var lunchMenuItems = allItems.FindAll(x => x.Timeslot == "Lunch");
                timeslotMenu.AddRange(lunchMenuItems);
            }
        }

        FoodMenu.MenuItems.AddRange(timeslotMenu);

        return timeslotMenu;
    }

    static bool ifDinner(TimeOnly time)
    {
        TimeOnly startTime = new TimeOnly(18, 0);
        TimeOnly endTime = new TimeOnly(22, 0);

        if (time >= startTime && time <= endTime)
        {
            return true;
        }

        return false;
    }

    static (DateOnly date, TimeOnly time) SetTime()
    {
        DateTime now = DateTime.Now;

        DateOnly date = DateOnly.FromDateTime(now);
        TimeOnly time = TimeOnly.FromDateTime(now);

        return (date, time);

    }

    static List<MenuItem> GetLunchMenu()
    {
        var allItems = FoodMenu.LoadFoodMenuData();
        List<MenuItem> tempMenu = new List<MenuItem>();

        var lunchMenuItems = allItems.FindAll(x => x.Timeslot == "Lunch");
        tempMenu.AddRange(lunchMenuItems);

        return tempMenu;
    }

    static List<MenuItem> GetDinnerMenu()
    {
        var allItems = FoodMenu.LoadFoodMenuData();
        List<MenuItem> tempMenu = new List<MenuItem>();

        var lunchMenuItems = allItems.FindAll(x => x.Timeslot == "Dinner");
        tempMenu.AddRange(lunchMenuItems);

        return tempMenu;
    }
}
