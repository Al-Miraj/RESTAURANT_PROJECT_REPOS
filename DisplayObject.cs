using System;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

public class DisplayObject
{
    [XmlIgnore]
    [JsonIgnore]
    public char[,] Display { get; protected set; } = new char[0, 0];

    [JsonIgnore]
    public int Height => Display.GetLength(0);
    [JsonIgnore]
    public int Width => Display.GetLength(1);

    /// <summary>
    ///  Prints <see cref="DisplayObject.Display"/> at the specified <paramref name="position"/>.
    /// </summary>
    /// <param name="position">The <see cref="Console"/> Cursor coordinates to print <see cref="DisplayObject.Display"/> at. </param>
    public virtual void PrintAt((int X, int Y) position)
    {
        for (int y = 0; y <= Height - 1; y++)
        {
            Console.SetCursorPosition(position.X, position.Y + y);

            for (int x = 0; x <= Width - 1; x++)
            {
                Console.Write(Display[y, x]);
            }
        }
    }
}