public class SuperAdminAccount : AdminAccount
{
    private static SuperAdminAccount instance;

    private SuperAdminAccount(int id, string name, string email, string password)
        : base(id, name, email, password)
    {
    }

    public static SuperAdminAccount GetInstance()
    {
        if (instance == null)
        {
            instance = new SuperAdminAccount(0, "SuperAdmin", "superadmin@hotmail.com", "Admin-1234");
        }
        return instance;
    }

    public bool DeleteAdminByEmail(string email)
    {
        var adminAccount = LoginSystem.Accounts.FirstOrDefault(a => a.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && a is AdminAccount);
        if (adminAccount != null)
        {
            LoginSystem.Accounts.Remove(adminAccount);
            return true;
        }
        return false;
    }

    public static bool AddAdmin(string name, string email, string password)
    {
        if (LoginSystem.Accounts.Any(acc => acc.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
        {
            return false;
        }

        int newId = LoginSystem.GenerateNewID();
        AdminAccount newAdmin = new AdminAccount(newId, name, email, password);
        LoginSystem.Accounts.Add(newAdmin);
        return true;
    }

    // Removed DeleteAdmin method
}
