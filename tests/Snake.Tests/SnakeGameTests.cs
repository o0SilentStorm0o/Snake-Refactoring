using Snake.Core;

namespace Snake.Tests;

public sealed class SnakeGameTests
{
    [Fact]
    public void NewGameStartsWithConfiguredSnake()
    {
        var game = CreateGame(initialLength: 5, firstFood: new Cell(6, 3));

        var snapshot = game.CreateSnapshot();

        Assert.Equal(GameStatus.Running, snapshot.Status);
        Assert.Equal(5, snapshot.Score);
        Assert.Equal(new Cell(5, 4), snapshot.Head);
        Assert.Equal(new Cell(6, 3), snapshot.Food);
    }

    [Fact]
    public void SnakeCannotReverseIntoItself()
    {
        var game = CreateGame(initialLength: 3, firstFood: new Cell(1, 1));

        var accepted = game.TryChangeDirection(Direction.Left);
        game.Tick();

        Assert.False(accepted);
        Assert.Equal(GameStatus.Running, game.Status);
        Assert.Equal(new Cell(6, 4), game.CreateSnapshot().Head);
    }

    [Fact]
    public void SnakeGrowsAfterEatingFood()
    {
        var game = CreateGame(
            initialLength: 3,
            firstFood: new Cell(6, 4),
            nextFood: new Cell(7, 4));

        game.Tick();

        var snapshot = game.CreateSnapshot();
        Assert.Equal(4, snapshot.Score);
        Assert.Equal(new Cell(6, 4), snapshot.Head);
        Assert.Equal(new Cell(7, 4), snapshot.Food);
    }

    [Fact]
    public void SnakeDiesWhenItHitsWall()
    {
        var game = CreateGame(boardWidth: 7, boardHeight: 6, initialLength: 3, firstFood: new Cell(1, 1));

        game.Tick();
        game.Tick();
        game.Tick();

        Assert.Equal(GameStatus.GameOver, game.Status);
    }

    [Fact]
    public void SnakeCanMoveIntoTailCellWhenTailMovesAway()
    {
        var game = CreateGame(initialLength: 4, firstFood: new Cell(1, 1));

        game.TryChangeDirection(Direction.Up);
        game.Tick();
        game.TryChangeDirection(Direction.Left);
        game.Tick();
        game.TryChangeDirection(Direction.Down);
        game.Tick();

        var tailBeforeMove = game.CreateSnapshot().Snake[^1];
        game.TryChangeDirection(Direction.Right);
        game.Tick();

        Assert.Equal(GameStatus.Running, game.Status);
        Assert.Equal(tailBeforeMove, game.CreateSnapshot().Head);
    }

    private static SnakeGame CreateGame(
        int boardWidth = 10,
        int boardHeight = 8,
        int initialLength = 5,
        Cell? firstFood = null,
        Cell? nextFood = null)
    {
        return new SnakeGame(
            new SnakeGameOptions
            {
                BoardWidth = boardWidth,
                BoardHeight = boardHeight,
                InitialLength = initialLength,
                InitialDirection = Direction.Right
            },
            new SequenceFoodGenerator(firstFood, nextFood));
    }

    private sealed class SequenceFoodGenerator : IFoodGenerator
    {
        private readonly Queue<Cell?> _foods;

        public SequenceFoodGenerator(params Cell?[] foods)
        {
            _foods = new Queue<Cell?>(foods);
        }

        public Cell? PlaceFood(GameBoard board, IReadOnlySet<Cell> occupiedCells)
        {
            while (_foods.Count > 0)
            {
                var food = _foods.Dequeue();

                if (food is null)
                {
                    continue;
                }

                if (!board.IsWall(food.Value) && !occupiedCells.Contains(food.Value))
                {
                    return food;
                }
            }

            foreach (var cell in board.InteriorCells())
            {
                if (!occupiedCells.Contains(cell))
                {
                    return cell;
                }
            }

            return null;
        }
    }
}
