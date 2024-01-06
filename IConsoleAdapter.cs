public interface IConsoleAdapter
{
    string ReadLine();
    void WriteLine(string value);
    // Add other Console methods as needed
}

public class ConsoleAdapter : IConsoleAdapter
{
    public string ReadLine()
    {
        return Console.ReadLine();
    }

    public void WriteLine(string value)
    {
        Console.WriteLine(value);
    }
    // Implement other Console methods
}
