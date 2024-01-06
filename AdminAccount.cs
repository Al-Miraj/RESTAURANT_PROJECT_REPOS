public class AdminAccount : Account
    {
    public AdminAccount() : base() { }

    public AdminAccount(int id, string name, string email, string password)
        : base(id, name, email, password) { }
    public override List<Reservation> GetReservations()
        {
            return ReservationSystem.Reservations;
        }
    public List<Account> GetAccounts()
    {
        return LoginSystem.Accounts;
    }


}