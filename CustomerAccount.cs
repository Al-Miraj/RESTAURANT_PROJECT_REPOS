using System.Xml.Serialization;

public sealed class CustomerAccount : Account
{
    [XmlIgnore]
    public List<Reservation> Reservations = new List<Reservation>();
    public CustomerAccount(int id, string name, string email, string password) 
        : base(id, name, email, password) { }

    public CustomerAccount() : base(){ }

    public override List<Reservation> GetReservations() => Reservations;
}