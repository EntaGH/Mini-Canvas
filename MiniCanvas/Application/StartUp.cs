namespace App.Application;

public class StartUp
{
    public void Run()
    {
        PrintMenu();

        CommandHandler commandHandler = new();

        while (true)
        {
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                continue;

            commandHandler.Handle(input);
        }
    }

    private void PrintMenu()
    {
        Console.WriteLine("===== Mini Canvas Program ============");
        Console.WriteLine("C w h         - Create a new canvas");
        Console.WriteLine("L x1 y1 x2 y2 - Draw a line");
        Console.WriteLine("R x1 y1 x2 y2 - Draw a rectangle");
        Console.WriteLine("B x y c       - Bucket fill");
        Console.WriteLine("Q             - Quit");
        Console.WriteLine("======================================");
    }
}