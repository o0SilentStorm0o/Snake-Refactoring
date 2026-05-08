namespace Snake.Core;

public sealed class RandomFoodGenerator : IFoodGenerator
{
    private readonly Random _random;

    public RandomFoodGenerator()
        : this(Random.Shared)
    {
    }

    public RandomFoodGenerator(Random random)
    {
        _random = random ?? throw new ArgumentNullException(nameof(random));
    }

    public Cell? PlaceFood(GameBoard board, IReadOnlySet<Cell> occupiedCells)
    {
        ArgumentNullException.ThrowIfNull(board);
        ArgumentNullException.ThrowIfNull(occupiedCells);

        var availableCells = board
            .InteriorCells()
            .Where(cell => !occupiedCells.Contains(cell))
            .ToArray();

        if (availableCells.Length == 0)
        {
            return null;
        }

        return availableCells[_random.Next(availableCells.Length)];
    }
}
