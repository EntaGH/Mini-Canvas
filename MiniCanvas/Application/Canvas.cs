namespace App.Application;

public class Canvas
{
    private readonly char _drawCharacter = 'x';
    private readonly char _defaultCharacter = ' ';
    private readonly char _borderHorizontalCharacter = '-';
    private readonly char _borderVerticalCharacter = '|';
    private readonly char _borderCornerCharacter = '+';
    public int Width { get; }
    public int Height { get; }
    private readonly char[,] _pixels;

    public Canvas(int width, int height)
    {
        if (width <= 0 || height <= 0)
        {
            throw new ArgumentOutOfRangeException("Canvas size must be greater than zero.");
        }

        this.Width = width;
        this.Height = height;

        _pixels = new char[height + 2, width + 2];

        Clear();
        DrawBorder();
    }

    public void DrawLine(int x1, int y1, int x2, int y2)
    {
        if (x1 != x2 && y1 != y2)
        {
            throw new ArgumentException("Line must be horizontal or vertical.");
        }

        if (x1 == x2)
        {
            int start = Math.Min(y1, y2);
            int end = Math.Max(y1, y2);

            for (int y = start; y <= end; y++)
            {
                SetPixel(x1, y, _drawCharacter);
            }
        }
        else
        {
            int start = Math.Min(x1, x2);
            int end = Math.Max(x1, x2);

            for (int x = start; x <= end; x++)
            {
                SetPixel(x, y1, _drawCharacter);
            }
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
        {
            return;
        }

        char oldColor = GetPixel(startX, startY);

        if (oldColor == color)
        {
            return;
        }

        Queue<(int x, int y)> queue = new();

        queue.Enqueue((startX, startY));

        while (queue.Count > 0)
        {
            (int x, int y) = queue.Dequeue();

            if (!IsInside(x, y))
            {
                continue;
            }

            if (GetPixel(x, y) != oldColor)
            {
                continue;
            }

            SetPixel(x, y, color);

            queue.Enqueue((x + 1, y));
            queue.Enqueue((x - 1, y));
            queue.Enqueue((x, y + 1));
            queue.Enqueue((x, y - 1));
        }
    }

    public void Print()
    {
        for (int y = 0; y < Height + 2; y++)
        {
            for (int x = 0; x < Width + 2; x++)
            {
                Console.Write(_pixels[y, x]);
            }

            Console.WriteLine();
        }
    }

    private char GetPixel(int x, int y)
    {
        return _pixels[y, x];
    }

    private void SetPixel(int x, int y, char value)
    {
        if (!IsInside(x, y))
        {
            return;
        }

        _pixels[y, x] = value;
    }

    private bool IsInside(int x, int y)
    {
        return x >= 1 &&
               x <= Width &&
               y >= 1 &&
               y <= Height;
    }

    private void Clear()
    {
        for (int y = 0; y < Height + 2; y++)
        {
            for (int x = 0; x < Width + 2; x++)
            {
                _pixels[y, x] = _defaultCharacter;
            }
        }
    }

    private void DrawBorder()
    {
        for (int x = 1; x <= Width; x++)
        {
            _pixels[0, x] = _borderHorizontalCharacter;
            _pixels[Height + 1, x] = _borderHorizontalCharacter;
        }

        for (int y = 1; y <= Height; y++)
        {
            _pixels[y, 0] = _borderVerticalCharacter;
            _pixels[y, Width + 1] = _borderVerticalCharacter;
        }

        _pixels[0, 0] = _borderCornerCharacter;
        _pixels[0, Width + 1] = _borderCornerCharacter;
        _pixels[Height + 1, 0] = _borderCornerCharacter;
        _pixels[Height + 1, Width + 1] = _borderCornerCharacter;
    }
}