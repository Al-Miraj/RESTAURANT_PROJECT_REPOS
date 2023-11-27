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


public class Reservation
{
    public Account? CustomerAccount { get; set; } = null;
    public int? ReservationNumber { get; set; }
    public int? NumberOfPeople { get; set; }
    public DateOnly? Date { get; set; }
    public string? TimeSlot { get; set; }
    public Table? SelectedTable { get; set; }
    public double? Discount;

    public Reservation(int? reservationNumber, int? numberOfPeople, DateOnly? date, string? timeSlot, Table? selectedTable)
    {
        ReservationNumber = reservationNumber;
        NumberOfPeople = numberOfPeople;
        Date = date;
        TimeSlot = timeSlot;
        SelectedTable = selectedTable;
        if (CustomerAccount != null)
        {
            CustomerAccount.Reservations.Add(this);
        }
    }

    public Reservation() : this(null, null, null, null, null) { }

    public override string ToString()
    {
        return $"Reservation Number: {ReservationNumber}\nAmount of People: {NumberOfPeople}\nDate: {Date}\nTimeslot: {TimeSlot}\nTable number: {SelectedTable.TableNumber}";
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
