namespace Snake.Core;

public sealed record GameSnapshot(
    GameBoard Board,
    IReadOnlyList<Cell> Snake,
    Cell? Food,
    Direction Direction,
    int Score,
    GameStatus Status)
{
    public Cell Head => Snake[0];
}
