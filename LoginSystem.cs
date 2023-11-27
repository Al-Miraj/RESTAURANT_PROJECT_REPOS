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

static class LoginSystem
{
    public static List<Account> Accounts = new List<Account>(); // Utensils.ReadJson<List<Account>>("accounts.json") ?? 
    public static List<AdminAccount> AdminAccounts = new List<AdminAccount>(); // Utensils.ReadJson<List<AdminAccount>>("admins.json") ?? 

    static LoginSystem() => new AdminAccount(1, "admin@work.com", "Admin-123");

    public static void Start()
    {
        Console.WriteLine("Sign In");
        Console.WriteLine("For register menu, enter 'R'. Otherwise just enter.");
        string input = Console.ReadLine()!.ToLower();
        if (input == "r")
        {
            Register();
        }
        else
        {
            Login();
        }
    }

    public static void Login()
    {
        Console.WriteLine("Log in screen");
        Console.WriteLine();
        int attempts = 0;
        do
        {
            var (email, password) = ReadUserInfo(false);
            if (Accounts == null)
            {
                Console.WriteLine("There aren't any accounts registered yet. Please consider registering.");
                return;
            }
            if (AdminAccounts == null)
            {
                Console.WriteLine("There aren't any Admins yet.");
                return;
            }
            if (AdminAccounts.Any(account => account.Email == email && account.Password == password))
            {
                Console.WriteLine("Logging in...");
                var account = FindAccount(email, password);
                if (account != null)
                {
                    var dashboard = new Dashboard(account);
                    account.IsLoggedIn = true;
                    Console.WriteLine("Login succesful!");
                    Console.ReadLine();
                    dashboard.Display();
                }
            }
            else if (Accounts.Any(account => account.Email == email && account.Password == password))
            {
                Console.WriteLine("Logging in...");
                var account = FindAccount(email, password);
                if (account != null)
                {
                    var dashboard = new Dashboard(account);
                    account.IsLoggedIn = true;
                    Console.WriteLine("Login successful!");
                    Console.ReadLine();
                    dashboard.Display();
                }
            }
            else
            {
                Console.WriteLine("Login failed. Try again.");
                attempts++;
            }
        } while (attempts < 3);
        Console.WriteLine("Too many failed attempts. Please try again later.");
    }

    public static void Register()
    {
        Console.WriteLine("Register");
        Console.WriteLine();
        try
        {
            var (email, password) = ReadUserInfo(true);
            int id = (Accounts != null) ? GetLatestId() + 1 : 1;
            var account = new Account(id, email, password);
            Accounts?.Add(account);
            /*UpdateJson();*/
            account.IsLoggedIn = true;
            var dashboard = new Dashboard(account);
            Console.ReadLine();
            dashboard.Display();
        }
        catch (NullReferenceException ex)
        {
            Console.WriteLine($"An error occured: {ex.Message}");
        }
    }

    public static void Logout(Account account)
    {
        Console.WriteLine("Logging out...");
        account.IsLoggedIn = false;
        Environment.Exit(0);
    }

    public static int GetLatestId() => Accounts != null ? Accounts.Max(account => account.Id) : 0;
    private static Account? FindAccount(string email, string password) => Accounts?.FirstOrDefault(account => account.Email == email && account.Password == password);
    public static (string, string) ReadUserInfo(bool register)
    {
        do
        {
            Console.WriteLine("Enter your email and password");
            Console.WriteLine();
            Console.Write("\nEmail: ");
            string? email = Console.ReadLine();

            if (email != null && IsEmail(email))
            {
                if (register == true)
                {
                    Console.WriteLine("A password must meet the following requirements:");
                    Console.WriteLine("- The length of the password should be between 7 and 16 characters.");
                    Console.WriteLine("- The password should contain at least one uppercase letter.");
                    Console.WriteLine("- The password should contain at least one lowercase letter.");
                    Console.WriteLine("- The password should contain at least one digit.");
                    Console.WriteLine("- The password should contain at least one special character (such as !, @, #, $, %, etc.).");
                    Console.WriteLine("- The password should not contain any characters that are not digits, letters, or special characters.");
                }
                Console.Write("\nPassword: ");
                string? password = Console.ReadLine();
                if (password != null && IsValidPassword(password))
                {
                    return (email, password);
                }
                else
                {
                    Console.WriteLine("Please enter a valid password.");
                }
            }
            else
            {
                Console.WriteLine("Please enter a valid email. (i.e. john@email.com");
            }
        } while (true);
    }

    public static bool IsValidPassword(string? password)
    {
        if (password != null)
        {
            bool result = password.Length >= 7 && password.Length <= 16
            && Regex.IsMatch(password, "[A-Z]")
            && Regex.IsMatch(password, "[a-z]")
            && Regex.IsMatch(password, @"\d")
            && Regex.IsMatch(password, @"[!-/:-@\[-_{-~]")
            && !Regex.IsMatch(password, @"[^\dA-Za-z!-/:-@\[-_{-~]");
            return result;
        }
        else
        {
            return false;
        }
    }

    public static bool IsEmail(string? text)
    {
        if (text == null)
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

    public static void UpdateJson()
    {
        using StreamWriter writer = new("accounts.json");
        string json = JsonConvert.SerializeObject(Accounts);
        writer.Write(json);
    }


}
