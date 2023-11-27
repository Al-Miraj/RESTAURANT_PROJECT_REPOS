using System.ComponentModel.DataAnnotations;

public static class CustomerManagement
{
    public static AdminAccount CurrentAdmin { private get; set; }

    public static void Display()
    {
        int selectedOption = 1;
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Customer Account Management");
            Console.WriteLine();

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
                        Console.WriteLine(" Customer Overview");
                        break;
                    case 2:
                        Console.WriteLine(" View Customer Details");
                        break;
                    case 3:
                        Console.WriteLine(" Add Customer");
                        break;
                    case 4:
                        Console.WriteLine(" Update Customer");
                        break;
                    case 5:
                        Console.WriteLine(" Delete Customer");
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
    }

    static void HandleSelection(int option)
    {
        Console.Clear();

        switch (option)
        {
            case 1:
                CustomerOverview();
                break;
            case 2:
                ViewCustomer();
                break;
            case 3:
                AddCustomer();
                break;
            case 4:
                UpdateCustomer();
                break;
            case 5:
                DeleteCustomer();
                break;
            case 6:
                return;
            default:
                Console.WriteLine("Invalid choice. Please select a valid option.");
                Console.ReadLine();
                break;
        }
    }


    private static void AddCustomer()
    {
        Console.WriteLine("Add Customer");
        Console.WriteLine();
        Console.WriteLine("Enter customer details: ");
        var (email, password) = LoginSystem.ReadUserInfo(true);
        int id = (LoginSystem.Accounts != null) ? LoginSystem.GetLatestId() + 1 : 1;
        var customer = new Account(id, email, password);
        /*LoginSystem.UpdateJson();*/
        Console.WriteLine("Customer added!");
        Console.WriteLine(customer.ToString());
        Console.ReadLine();
    }

    private static void ViewCustomer()
    {
        Console.WriteLine("See Customer Overview for list of customers");
        Console.WriteLine("Enter customer id: ");
        int id = Convert.ToInt32(Console.ReadLine());
        var customer = CurrentAdmin.GetAccounts()?.FirstOrDefault(c => c.Id == id);
        if (customer == null)
        {
            Console.WriteLine("Customer not found");
            return;
        }
        Console.WriteLine("Customer Info:");
        Console.WriteLine(customer.ToString());
        Console.ReadLine();
    }


    private static void CustomerOverview()
    {
        var customers = CurrentAdmin.GetAccounts();
        Console.WriteLine("Customer Overview");
        Console.WriteLine();
        if (customers == null)
        {
            Console.WriteLine("There are no customers yet");
            return;
        }
        foreach (var customer in customers)
        {
            Console.WriteLine(customer.ToString());
            Console.WriteLine();
        }
        Console.ReadLine();
    }

    private static void UpdateCustomer()
    {
        Console.WriteLine("Enter customer id: ");
        int id = Convert.ToInt32(Console.ReadLine());
        var customer = CurrentAdmin.GetAccounts()?.FirstOrDefault(c => c.Id == id);
        if (customer == null)
        {
            Console.WriteLine("Customer not found");
            return;
        }
        string custStr = customer.ToString();
        Console.WriteLine(custStr);
        (string email, string password) = LoginSystem.ReadUserInfo(false);
        customer.Email = email;
        customer.Password = password;
        LoginSystem.UpdateJson();
        Console.WriteLine("Customer updated!");
        Console.WriteLine("Before\n" + custStr + "After\n" + customer.ToString());
        Console.ReadLine();
    }

    private static void DeleteCustomer()
    {
        Console.WriteLine("See Customer Overview for list of customers");
        Console.WriteLine("Enter customer id: ");
        int id = Convert.ToInt32(Console.ReadLine());
        var customer = CurrentAdmin.GetAccounts()?.FirstOrDefault(c => c.Id == id);
        if (customer == null)
        {
            Console.WriteLine("Customer not found");
            return;
        }
        CurrentAdmin.GetAccounts()?.Remove(customer);
        LoginSystem.UpdateJson();
        Console.WriteLine("Customer deleted!");
        Console.ReadLine();
    }
}
