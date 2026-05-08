namespace Snake.Core;

public static class DirectionExtensions
{
    public static bool IsOppositeOf(this Direction direction, Direction other)
    {
        return (direction, other) is
            (Direction.Up, Direction.Down) or
            (Direction.Down, Direction.Up) or
            (Direction.Left, Direction.Right) or
            (Direction.Right, Direction.Left);
    }

    public static Direction Opposite(this Direction direction)
    {
        return direction switch
        {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}
