namespace App.Application;

public class Canvas
{
    private readonly char drawCharacter = 'x';
    private readonly char defaultCharacter = ' ';
    private readonly int width;
    private readonly int height;
    private readonly char[,] pixels;

    public Canvas(int width, int height)
    {
        if (width <= 0 || height <= 0)
            throw new ArgumentOutOfRangeException("Canvas size must be greater than zero.");

        this.width = width;
        this.height = height;

        pixels = new char[height + 2, width + 2];

        Clear();
        DrawBorder();
    }

    public void DrawLine(int x1, int y1, int x2, int y2)
    {
        if (x1 == x2)
        {
            int start = Math.Min(y1, y2);
            int end = Math.Max(y1, y2);

            for (int y = start; y <= end; y++)
            {
                SetPixel(x1, y, drawCharacter);
            }
        }
        else if (y1 == y2)
        {
            int start = Math.Min(x1, x2);
            int end = Math.Max(x1, x2);

            for (int x = start; x <= end; x++)
            {
                SetPixel(x, y1, drawCharacter);
            }
        }
        else
        {
            throw new Exception("Line must be horizontal or vertical.");
        }    
    }

    public void DrawRectangle(
        int x1,
        int y1,
        int x2,
        int y2)
    {
        DrawLine(x1, y1, x2, y1);
        DrawLine(x1, y2, x2, y2);
        DrawLine(x1, y1, x1, y2);
        DrawLine(x2, y1, x2, y2);
    }

    public void BucketFill(
        int startX,
        int startY,
        char color)
    {
        if (!IsInside(startX, startY))
            return;

        char oldColor = GetPixel(startX, startY);

        if (oldColor == color)
            return;

        Queue<(int x, int y)> queue = new();

        queue.Enqueue((startX, startY));

        while (queue.Count > 0)
        {
            var (x, y) = queue.Dequeue();

            if (!IsInside(x, y))
                continue;

            if (GetPixel(x, y) != oldColor)
                continue;

            SetPixel(x, y, color);

            queue.Enqueue((x + 1, y));
            queue.Enqueue((x - 1, y));
            queue.Enqueue((x, y + 1));
            queue.Enqueue((x, y - 1));
        }
    }

    public void Print()
    {
        for (int y = 0; y < height + 2; y++)
        {
            for (int x = 0; x < width + 2; x++)
            {
                Console.Write(pixels[y, x]);
            }

            Console.WriteLine();
        }
    }

    private char GetPixel(int x, int y)
    {
        return pixels[y, x];
    }

    private void SetPixel(int x, int y, char value)
    {
        if (!IsInside(x, y))
            return;

        pixels[y, x] = value;
    }

    private bool IsInside(int x, int y)
    {
        return x >= 1 &&
               x <= width &&
               y >= 1 &&
               y <= height;
    }

    private void Clear()
    {
        for (int y = 0; y < height + 2; y++)
        {
            for (int x = 0; x < width + 2; x++)
            {
                pixels[y, x] = defaultCharacter;
            }
        }
    }

    private void DrawBorder()
    {
        for (int x = 1; x <= width; x++)
        {
            pixels[0, x] = '-';
            pixels[height + 1, x] = '-';
        }

        for (int y = 1; y <= height; y++)
        {
            pixels[y, 0] = '|';
            pixels[y, width + 1] = '|';
        }

        pixels[0, 0] = '+';
        pixels[0, width + 1] = '+';
        pixels[height + 1, 0] = '+';
        pixels[height + 1, width + 1] = '+';
    }
}