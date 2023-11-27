using Newtonsoft.Json;
using System.Text;

public class Account
{
    public int Id { get; }
    public string Email { get; set; }
    public string Password { get; set; }
    public List<Reservation> Reservations = new List<Reservation>();
    public bool IsLoggedIn;

    public Account(int id, string email, string password)
    {
        Id = id;
        Email = email;
        Password = password;
        if (LoginSystem.Accounts != null)
        {
            LoginSystem.Accounts.Add(this);
        }
        else
        {
            LoginSystem.Accounts = new List<Account>();
        }
    }

    public virtual List<Reservation> GetReservations() => Reservations;

    public override string ToString()
    {
        return $"ID: {Id}\nEmail: {Email}\n Password: {Utensils.HashPassword(Password)}";   //Password hashed for privacy of customers
    }
}
