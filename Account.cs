using Newtonsoft.Json;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

[XmlInclude(typeof(AdminAccount))]
[XmlInclude(typeof(CustomerAccount))]
[XmlInclude(typeof(Reservation))]


public abstract class Account
{
    // todo make these fields into properties (fix readonly issue)
    public int ID;
    public string Email;
    public string Name;
    public string Password;

    protected Account(int id, string name, string email, string password)
    {
        //string firstLetter = name[0].ToString();
        ID = id;
        //Name = string.Concat(firstLetter.ToUpper(), name.AsSpan(1));
        Name = name;
        Email = email;
        Password = password;
        //if (LoginSystem.Accounts != null)
        //{
        //    LoginSystem.Accounts.Add(this);
        //}
        //else
        //{
        //    LoginSystem.Accounts = new List<Account> { this };
        //}
    }

    public Account() { }

    public abstract List<Reservation> GetReservations();

    public override string ToString()
    {
        return $"ID: {ID}\nName: {Name}\nEmail: {Email}\nPassword: {Utensils.HashPassword(Password)}";   //Password hashed for privacy of customers
    }
}