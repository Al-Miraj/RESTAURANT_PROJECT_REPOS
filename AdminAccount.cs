using Newtonsoft.Json;

public sealed class AdminAccount : Account
{
    public AdminAccount(int id, string name, string email, string password) : base(id, name, email, password)
    {
        //if (LoginSystem.AdminAccounts != null)
        //{
        //    LoginSystem.AdminAccounts.Add(this);
        //    Utensils.WriteJson("Admins.json", LoginSystem.AdminAccounts);
        //}
        //else
        //{
        //    LoginSystem.AdminAccounts = new List<AdminAccount>();
        //}
    }

    public AdminAccount() : base() { }

    public override List<Reservation> GetReservations()
    {
        return Restaurant.Reservations;
    }

    public List<Account>? GetAccounts()
    {
        return Restaurant.Accounts;
    }
}


 