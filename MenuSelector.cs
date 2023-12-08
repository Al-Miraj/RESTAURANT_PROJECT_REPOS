public static class MenuSelector
{
    private static string Cursor = " >";

    public static int RunMenuNavigator(List<string> options)
    {
        int selectedOption = 0;
        int x = Console.CursorLeft;
        int y = Console.CursorTop;
        string option;
        Console.CursorVisible = false;
        while (true)
        {
            Console.SetCursorPosition(x, y);
            for (int i = 0; i < options.Count; i++)
            {
                if (i == selectedOption)
                {
                    Console.WriteLine($"{Cursor} {options[i]}");
                }
                else
                {
                    Console.WriteLine($"   {options[i]}");
                }
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey();

            if (keyInfo.Key == ConsoleKey.UpArrow && selectedOption > 0)
            {
                selectedOption--;
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow && selectedOption < options.Count - 1)
            {
                selectedOption++;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                Console.CursorVisible = true;
                return selectedOption;
            }
            else
            {
                continue;
            }
        }
    }
}