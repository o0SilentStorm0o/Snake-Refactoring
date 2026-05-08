using Snake.Core;

namespace Snake.ConsoleApp;

internal sealed class ConsoleInputReader
{
    public IEnumerable<Direction> ReadDirections()
    {
        while (Console.KeyAvailable)
        {
            if (TryReadDirection(Console.ReadKey(intercept: true).Key, out var direction))
            {
                yield return direction;
            }
        }
    }

    private static bool TryReadDirection(ConsoleKey key, out Direction direction)
    {
        direction = key switch
        {
            ConsoleKey.UpArrow => Direction.Up,
            ConsoleKey.DownArrow => Direction.Down,
            ConsoleKey.LeftArrow => Direction.Left,
            ConsoleKey.RightArrow => Direction.Right,
            _ => default
        };

        return key is ConsoleKey.UpArrow or
            ConsoleKey.DownArrow or
            ConsoleKey.LeftArrow or
            ConsoleKey.RightArrow;
    }
}
