//public class Reservation
//{
//    public int? ReservationNumber;
//    public int? NumberOfPeople;
//    public DateOnly? Date;
//    public string? TimeSlot;
//    public List<Deal>? DealsApplied;
//    public Table? SelectedTable;
//    public Reservation()
//    {
//        ReservationNumber = null;
//        NumberOfPeople = null;
//        Date = null;
//        TimeSlot = null;
//        SelectedTable = null;
//        DealsApplied = null;
//    }
//}

using System;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

[XmlInclude(typeof(Account))]
[XmlInclude(typeof(Table))]
[XmlInclude(typeof(Deal))]
[XmlInclude(typeof(DateOnly))]


public class Reservation
{
    public int CustomerID;
    public int ReservationNumber;
    public int NumberOfPeople;
    [XmlIgnore]
    public DateOnly Date;
    [XmlElement("Date")]
    public string DateString // used only by xml (de)serializer.
    {
        get { return Date.ToString(); }
        set { Date = DateOnly.Parse(value); }
    }
    public string TimeSlot;
    public Table SelectedTable;
    public List<Deal> DealsApplied = new List<Deal>(); //
    public double NonDiscountedPrice;
    public double DiscountFactor = 0; // 

    public Reservation(int customerID, int reservationNumber, int numberOfPeople, DateOnly date, string timeSlot, Table selectedTable, double nonDiscountedPrice)
    {
        CustomerID = customerID;
        ReservationNumber = reservationNumber;
        NumberOfPeople = numberOfPeople;
        Date = date;
        TimeSlot = timeSlot;
        SelectedTable = selectedTable;
        NonDiscountedPrice = nonDiscountedPrice;
    }

    public Reservation() { }



    public override string ToString()
    {
        return 
            $"Customer ID: {CustomerID}" +
            $"\nReservation Number: {ReservationNumber}" +
            $"\nAmount of People: {NumberOfPeople}" +
            $"\nDate: {Date}\nTimeslot: {TimeSlot}" +
            $"\nTable number: {SelectedTable.TableNumber}" +
            $"\nPrice: {GetTotalPrice()}";
    }

    public double GetTotalPrice()
    {
        return NonDiscountedPrice * (1 - DiscountFactor);
    }

    //public void AddDeal(Deal deal)
    //{
    //    if (deal != null)
    //    {
    //        if (deal is GuestAmountDiscountDeal)
    //        { DealsApplied.Add(deal);}
    //    }
    //}

    // todo: get the total price of the reservation and store it
    //public double GetTotalPrice()
    //{
    //    if (DealsApplied.Count > 0)
    //    {
    //        foreach (Deal deal in DealsApplied)
    //        {

    //        }
    //    }
    //    return TotalPrice;
    //}
}