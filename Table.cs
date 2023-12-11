using System.Text.Json.Serialization;

public class Table : DisplayObject
{
    private int _tableNumber;
    [JsonPropertyName("TableNumber")]
    public int TableNumber
    {
        get { return _tableNumber; }
        set
        {
            _tableNumber = value;
            Display = new char[5, 8];
            CreateTableDisplay();
        }
    }

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

    private void CreateTableDisplay()
    {
        // get right- and downMost indices of the grid
        // i.e. height = 5, then downMost index is 4.
        int rightMost = Width - 1;
        int downMost = Height - 1;

        // Get the corner symbols of the table in the correct indices
        Display[0, 1] = '╔';
        Display[downMost, 1] = '╚';
        Display[0, rightMost - 1] = '╗';
        Display[downMost, rightMost - 1] = '╝';

        // Connect the corners with the table side characters
        for (int x = 2; x < rightMost - 1; x++)
            Display[0, x] = '═';
        for (int x = 2; x < rightMost - 1; x++)
            Display[downMost, x] = '═';
        for (int y = 1; y < downMost; y++)
            Display[y, 1] = '║';
        for (int y = 1; y < downMost; y++)
            Display[y, rightMost - 1] = '║';

        // place the seat characters according to the table capacity
        char leftSeat = '[';
        char rightSeat = ']';
        if (Capacity == 2)
        {
            Display[2, 0] = leftSeat; Display[2, 7] = rightSeat;
        }
        else if (Capacity == 4)
        {
            Display[1, 0] = leftSeat; Display[1, 7] = rightSeat;
            Display[3, 0] = leftSeat; Display[3, 7] = rightSeat;
        }
        else if (Capacity == 6)
        {
            Display[1, 0] = leftSeat; Display[1, 7] = rightSeat;
            Display[2, 0] = leftSeat; Display[2, 7] = rightSeat;
            Display[3, 0] = leftSeat; Display[3, 7] = rightSeat;
        }

        // Place the TableNumber in the middle of the table display
        Display[2, Width / 2 - 1] = TableNumber.ToString("00")[0];
        Display[2, Width / 2] = TableNumber.ToString("00")[1];

        // fill the remaining indices on the grid with ' ' character
        for (int x = 0; x <= rightMost; x++)
            for (int y = 0; y <= downMost; y++)
                if (Display[y, x] == '\0')
                { Display[y, x] = ' '; }
    }
}