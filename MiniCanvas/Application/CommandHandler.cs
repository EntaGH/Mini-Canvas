using App.Application;

public class CommandHandler
{
    private Canvas? canvas;

    private readonly Dictionary<string, CommandDefinition> handlers;

    public CommandHandler()
    {
        handlers = new Dictionary<string, CommandDefinition>
        {
            ["C"] = new(['i', 'i'], CreateCanvas),
            ["L"] = new(['i', 'i', 'i', 'i'], DrawLine),
            ["R"] = new(['i', 'i', 'i', 'i'], DrawRectangle),
            ["B"] = new(['i', 'i', 'c'], BucketFill),
            ["Q"] = new([], Quit)
        };
    }

    public void Handle(string input)
    {
        try
        {
            var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 0)
                throw new ArgumentException("Command cannot be empty.");

            var command = parts[0];

            if (!handlers.TryGetValue(command, out var definition))
            {
                throw new ArgumentException($"Invalid command: '{command}'.");
            }

            var args = parts.Skip(1).ToArray();

            ValidateArguments(
                command,
                args,
                definition.Arguments.ToList());

            definition.Handler(args);
        }
        catch (Exception e)
        {
            HandleException(e);
        }
        finally
        {
            canvas?.Print();
        }
    }

    private static void ValidateArguments(
        string command,
        string[] args,
        IReadOnlyList<char> expectedArguments)
    {
        if (args.Length != expectedArguments.Count)
        {
            throw new ArgumentException(
                $"Command '{command}' expects " +
                $"{expectedArguments.Count} arguments, " +
                $"but received {args.Length}.");
        }

        for (int i = 0; i < args.Length; i++)
        {
            switch (expectedArguments[i])
            {
                case 'i':
                    if (!int.TryParse(args[i], out _))
                    {
                        throw new FormatException($"Argument {i + 1} of command '{command}' " + $"must be an integer.");
                    }

                    break;

                case 'c':
                    if (args[i].Length != 1)
                    {
                        throw new ArgumentException($"Argument {i + 1} of command '{command}' " + $"must be a single character.");
                    }

                    break;

                default:
                    throw new InvalidOperationException($"Unknown argument type '{expectedArguments[i]}'.");
            }
        }
    }

    private static void HandleException(Exception e)
    {
        switch (e)
        {
            case ArgumentOutOfRangeException:
                Console.WriteLine($"Invalid value: {e.Message}");
                break;

            case ArgumentException:
                Console.WriteLine($"Invalid argument: {e.Message}");
                break;

            case FormatException:
                Console.WriteLine($"Invalid format: {e.Message}");
                break;

            case InvalidOperationException:
                Console.WriteLine($"Invalid operation: {e.Message}");
                break;

            default:
                Console.WriteLine($"Unexpected error: {e.Message}");
                break;
        }
    }

    private void CreateCanvas(string[] args)
    {
        int width = int.Parse(args[0]);
        int height = int.Parse(args[1]);

        canvas = new Canvas(width, height);
    }

    private void DrawLine(string[] args)
    {
        if (canvas == null)
            return;

        int x1 = int.Parse(args[0]);
        int y1 = int.Parse(args[1]);
        int x2 = int.Parse(args[2]);
        int y2 = int.Parse(args[3]);

        canvas.DrawLine(x1, y1, x2, y2);
    }

    private void DrawRectangle(string[] args)
    {
        if (canvas == null)
            return;

        int x1 = int.Parse(args[0]);
        int y1 = int.Parse(args[1]);
        int x2 = int.Parse(args[2]);
        int y2 = int.Parse(args[3]);

        canvas.DrawRectangle(x1, y1, x2, y2);
    }

    private void BucketFill(string[] args)
    {
        if (canvas == null)
            return;

        int x = int.Parse(args[0]);
        int y = int.Parse(args[1]);
        char color = args[2][0];

        canvas.BucketFill(x, y, color);
    }

    private void Quit(string[] args)
    {
        Environment.Exit(0);
    }

    public Canvas? GetCanvas()
    {
        return canvas;
    }
}