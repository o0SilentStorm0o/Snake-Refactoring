namespace Snake.Core;

public sealed class GameBoard
{
    public GameBoard(int width, int height)
    {
        if (width < 5)
        {
            throw new ArgumentOutOfRangeException(nameof(width), "Board width must be at least 5.");
        }

        if (height < 5)
        {
            throw new ArgumentOutOfRangeException(nameof(height), "Board height must be at least 5.");
        }

        Width = width;
        Height = height;
    }

    public int Width { get; }

    public int Height { get; }

    public bool Contains(Cell cell)
    {
        return cell.X >= 0 &&
            cell.X < Width &&
            cell.Y >= 0 &&
            cell.Y < Height;
    }

    public bool IsWall(Cell cell)
    {
        return !Contains(cell) ||
            cell.X == 0 ||
            cell.X == Width - 1 ||
            cell.Y == 0 ||
            cell.Y == Height - 1;
    }

    public IEnumerable<Cell> InteriorCells()
    {
        for (var y = 1; y < Height - 1; y++)
        {
            for (var x = 1; x < Width - 1; x++)
            {
                yield return new Cell(x, y);
            }
        }
    }
}
