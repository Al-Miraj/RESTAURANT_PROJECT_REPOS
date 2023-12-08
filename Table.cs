using System.Text.Json.Serialization;

public class Table
{
    [JsonPropertyName("TableNumber")]
    public int TableNumber;

    [JsonPropertyName("Coordinate")]
    public (int, int) Coordinate;

    [JsonPropertyName("Capacity")]
    public int Capacity;

    [JsonPropertyName("IsReservated")]
    public bool IsReservated;

    [JsonPropertyName("TablePrice")]
    public double TablePrice;

    public Table(int tableNumber, (int, int) coordinate, int capacity, double tablePrice, bool isReservated)
    {
        TableNumber = tableNumber;
        Coordinate = coordinate;
        Capacity = capacity;
        TablePrice = tablePrice;
        IsReservated = isReservated;
    }

    public Table() { }
}