namespace Snake.Core;

public sealed record SnakeGameOptions
{
    public int BoardWidth { get; init; } = 32;

    public int BoardHeight { get; init; } = 16;

    public int InitialLength { get; init; } = 5;

    public Direction InitialDirection { get; init; } = Direction.Right;
}
