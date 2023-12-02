using System.Xml.Serialization;

class Program
{
    public static void Main()
    {
        //CustomerAccount john = new CustomerAccount(2, "john", "john@mail.com", "John-12345");
        //int ppl = ReservationSystem.GetNumberOfPeople(false);
        //DateOnly date = ReservationSystem.GetReservationDate();
        //string timeslot  = ReservationSystem.GetTimeslot();
        //Table selectedTable = ReservationSystem.GetChosenTable(ppl);
        //if (selectedTable != null)
        //{
        //    int reservationNumber = ReservationSystem.GenerateReservationNumber();
        //    Reservation reservation = new Reservation(john.Id, reservationNumber, ppl, date, timeslot, selectedTable);
        //    reservation.NonDiscountedPrice += selectedTable.TablePrice;

        //    ReservationSystem.DisplayReservationDetails(reservation);
        //    // UpdateJson(); // Reservation added to json
        //    // todo: reservations must be stored somehow
        //}
        //else
        //{
        //    Console.WriteLine("You weren't able to finish the reservation.");
        //}
        //Console.ReadKey();

        //Console.WriteLine(Restaurant.Accounts.Count == 1);
        //Console.WriteLine(Restaurant.AdminAccounts.Count == 1);
        //Console.WriteLine(Restaurant.Deals.Count == 1);
        //Console.WriteLine(Restaurant.Tables.Count == 15);
        //Console.WriteLine(Restaurant.Reservations.Count == 0);

        //CustomerAccount john = new CustomerAccount(2, "john", "john@mail.com", "John-12345");
        //Restaurant.Accounts.Add(john);
        //int ppl = ReservationSystem.GetNumberOfPeople(false);
        //DateOnly date = ReservationSystem.GetReservationDate();
        //string timeslot = ReservationSystem.GetTimeslot();
        //Table selectedTable = ReservationSystem.GetChosenTable(ppl);
        //if (selectedTable != null)
        //{
        //    int reservationNumber = ReservationSystem.GenerateReservationNumber();
        //    Reservation reservation = new Reservation(john.Id, reservationNumber, ppl, date, timeslot, selectedTable);
        //    reservation.NonDiscountedPrice += selectedTable.TablePrice;

        //    ReservationSystem.DisplayReservationDetails(reservation);
        //    john.Reservations.Add(reservation);
        //    Restaurant.AddReservation(reservation);
        //    XmlFileHandler.WriteToFile(Restaurant.Accounts, Restaurant.AccountsXmlFileName);
        //}
        //Console.WriteLine(Restaurant.Accounts.Count == 1);
        //Console.WriteLine(Restaurant.AdminAccounts.Count == 1);
        //Console.WriteLine(Restaurant.Deals.Count == 1);
        //Console.WriteLine(Restaurant.Tables.Count == 15);
        //Console.WriteLine(Restaurant.Reservations.Count == 0);

        //List<Reservation> rs = XmlFileHandler.ReadFromFile<Reservation>(Restaurant.ReservationsXmlFileName);
        //Reservation reservationx = rs[0];
        //Console.WriteLine(reservationx.Date);

        OptionMenu.RunMenu();







        //Menu.RunMenu();
        //foreach (AdminAccount adminAcc in LoginSystem.AdminAccounts)
        //{
        //    Console.WriteLine(adminAcc.Name);
        //    Console.WriteLine(adminAcc.Id);
        //    Console.WriteLine(adminAcc.Email);
        //    Console.WriteLine(adminAcc.Password);
        //    Console.WriteLine(adminAcc.Reservations.Count);
        //}
        //foreach (Account Account in LoginSystem.Accounts)
        //{
        //    Console.WriteLine(Account.Name);
        //    Console.WriteLine(Account.Id);
        //    Console.WriteLine(Account.Email);
        //    Console.WriteLine(Account.Password);
        //    Console.WriteLine(Account.Reservations.Count);
        //}
    }
}

//[XmlInclude(typeof(Reservation))]
//public class Customer
//{
//    public int Id;
//    public List<Reservation> Reservations = new List<Reservation>();
//    public Customer(int id)
//    {
//        Id = id;
//    }
//}

//public class Reservation
//{
//    public int CustomerId;
//    public int ReservationID;
//    public Reservation(int customerId, int reservationId)
//    {
//        CustomerId = customerId;
//        ReservationID = reservationId;
//    }
//}

//public class Programm
//{
//    public static void Main()
//    {
//        List<Customer> customers = new List<Customer>();
//        List<Reservation> reservations = new List<Reservation>();
//        Customer cJohn = new Customer(1);
//        Reservation johnReservation = new Reservation(cJohn.Id, 9263);
//        cJohn.Reservations.Add(johnReservation);

//        XmlFileHandler.WriteToFile(customers, "Customers.xml");
//        XmlFileHandler.WriteToFile(reservations, "Reservations.xml");
//    }
//}