using Newtonsoft.Json;
using System.Net.Mail;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Data;
using System.Xml.Linq;


static class LoginSystem
{
    public static void Start()
    {
        Console.WriteLine("Welcome!\n\nWhat do you want to do:");
        List<string> LoginRegisterOptions = new List<string>() { "Log into existing account", "Create new account" };
        int selectedOption = MenuSelector.RunMenuNavigator(LoginRegisterOptions);
        switch (selectedOption)
        {
            case 0: Console.Clear(); Login(); break;
            case 1: Console.Clear(); Register(); break;
        }
    }

    public static void Login()
    {
        int attempts = 3;

        Console.WriteLine("Log in screen\n");
        do
        {
            string email = GetAccountEmail();
            string password = GetAccountPassword(false);
            Account account = FindAccount(email, password);
            if (account != null)
            {
                Console.WriteLine("Logging in...");
                Console.WriteLine("Login successful!");
                Dashboard dashboard = new Dashboard(account);
                ConnectUser(account, dashboard);
                Console.WriteLine("\n[Press any key to continue to the dashboard.]");
                Console.ReadKey();
                dashboard.RunDashboardMenu();
                return;
            }
            else
            {
                attempts--;
                Console.WriteLine($"Login failed. You have {attempts} left\n"); // todo give reason why login failed
            }
        } while (attempts > 0);
        Console.WriteLine("Too many failed attempts. Please try again later.");
    }

    public static void Register()
    {
        Console.WriteLine("Create account\n");
        CustomerAccount account = CreateCustomerAccount();
        Console.WriteLine("Logging in...");
        Console.WriteLine("Login successful!");
        Dashboard dashboard = new Dashboard(account);
        ConnectUser(account, dashboard);
        Console.WriteLine("\n[Press any key to continue to the dashboard.]");
        Console.ReadKey();
        dashboard.RunDashboardMenu();
    }

    public static CustomerAccount CreateCustomerAccount()
    {
        string name = GetAccountName();
        string email = GetAccountEmail();
        string password = GetAccountPassword(true);

        // Accounts zou nooit null moeten zijn (anders gaat er echt wat fout)
        // en er is altijd een default admin met id 1 aanwezig
        int id = (Restaurant.Accounts.Count > 1) ? GetLatestId() + 1 : 2;

        Console.WriteLine("Creating account...");
        CustomerAccount account = new CustomerAccount(id, name, email, password);
        Restaurant.Accounts.Add(account);
        Restaurant.UpdateRestaurantFiles();
        Console.WriteLine("Account creation successful!");
        return account;
    }

    public static void Logout()
    {
        Console.WriteLine("Logging out...");
        ResetMenu();
        Console.WriteLine("You're now logged out");
    }

    private static void ResetMenu()
    {
        OptionMenu.IsUserLoggedIn = false;
        OptionMenu.CurrentUser = null;
        OptionMenu.UserDashboard = null;
    }

    public static int GetLatestId() => Restaurant.Accounts != null ? Restaurant.Accounts.Max(account => account.ID) : 0;
    
    private static Account? FindAccount(string email, string password) => Restaurant.Accounts.FirstOrDefault(account => account.Email == email && account.Password == password);

    public static string GetAccountName()
    {
        string name;
        Console.WriteLine("Please enter your name: ");
        while (true)
        {
            name = Console.ReadLine()!;
            if (name.Length == 0)
            {
                Console.WriteLine("Invalid input. Your name must at least be 1 character.\n");
                continue;
            }
            else if (name.Any(char.IsDigit))
            {
                Console.WriteLine("Invalid input. Your name must not contain any digits.\n");
                continue;
            }
            else if (char.IsLower(name[0]))
            {
                Console.WriteLine("Invalid input. Your name must start with a capital letter.\n");
                continue;
            }
            break;
        }
        return name;
    }

    public static string GetAccountEmail()
    {
        string email;
        Console.WriteLine("Please enter your email: ");
        while (true)
        {
            email = Console.ReadLine()!;
            if (IsValidEmail(email))
            {
                break;
            }
            else
            {
                Console.WriteLine("Please enter a valid email. (i.e. john@email.com");
            }
        }
        return email;
    }

    public static string GetAccountPassword(bool registering)
    {
        string password;
        if (registering) { DisplayPasswordRequirements(); }
        Console.WriteLine("Please enter the password: ");
        while (true)
        {
            password = Console.ReadLine()!;
            if (!IsValidPassword(password))
            {
                Console.WriteLine("Please enter a valid password.");
                continue;
            }
            break;
        }
        return password;
    }

    public static void DisplayPasswordRequirements()
    {
        Console.WriteLine("\nA password must meet the following requirements:");
        Console.WriteLine("- The length of the password should be between 7 and 16 characters.");
        Console.WriteLine("- The password should contain at least one uppercase letter.");
        Console.WriteLine("- The password should contain at least one lowercase letter.");
        Console.WriteLine("- The password should contain at least one digit.");
        Console.WriteLine("- The password should contain at least one special character (such as !, @, #, $, %, etc.).");
        Console.WriteLine("- The password should not contain any characters that are not digits, letters, or special characters.\n");
    }

    public static bool IsValidPassword(string password)
    {
        bool result =
                password.Length >= 7
                && password.Length <= 16
                && Regex.IsMatch(password, "[A-Z]")
                && Regex.IsMatch(password, "[a-z]")
                && Regex.IsMatch(password, @"\d")
                && Regex.IsMatch(password, @"[!-/:-@\[-_{-~]")
                && !Regex.IsMatch(password, @"[^\dA-Za-z!-/:-@\[-_{-~]");
        return result;
    }

    public static bool IsValidEmail(string? text)
    {
        if (text =="")
        {
            return false;
        }
        try                                             // Met System.Net.Mail kijken of de tekst in de goede format is
        {
            var mailAddress = new MailAddress(text);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    private static void ConnectUser(Account account, Dashboard dashboard)
    {
        OptionMenu.IsUserLoggedIn = true;
        OptionMenu.CurrentUser = account;
        OptionMenu.UserDashboard = dashboard;
    }

}