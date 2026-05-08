namespace Snake.Core;

public sealed class SnakeGame
{
    private readonly IFoodGenerator _foodGenerator;
    private readonly HashSet<Cell> _occupiedCells = [];
    private readonly LinkedList<Cell> _snake = [];

    private Direction _currentDirection;
    private bool _directionChangeQueued;
    private Direction _nextDirection;

    public SnakeGame(SnakeGameOptions options, IFoodGenerator? foodGenerator = null)
    {
        ArgumentNullException.ThrowIfNull(options);

        Board = new GameBoard(options.BoardWidth, options.BoardHeight);
        _currentDirection = options.InitialDirection;
        _nextDirection = options.InitialDirection;
        _foodGenerator = foodGenerator ?? new RandomFoodGenerator();

        InitializeSnake(options.InitialLength, options.InitialDirection);
        Food = _foodGenerator.PlaceFood(Board, _occupiedCells);
        Status = Food is null ? GameStatus.Won : GameStatus.Running;
    }

    public GameBoard Board { get; }

    public Cell? Food { get; private set; }

    public int Score => _snake.Count;

    public GameStatus Status { get; private set; }

    public GameSnapshot CreateSnapshot()
    {
        return new GameSnapshot(
            Board,
            _snake.ToArray(),
            Food,
            _nextDirection,
            Score,
            Status);
    }

    public bool TryChangeDirection(Direction requestedDirection)
    {
        if (Status != GameStatus.Running)
        {
            return false;
        }

        if (requestedDirection == _nextDirection)
        {
            return true;
        }

        if (_directionChangeQueued || requestedDirection.IsOppositeOf(_currentDirection))
        {
            return false;
        }

        _nextDirection = requestedDirection;
        _directionChangeQueued = true;
        return true;
    }

    public GameStatus Tick()
    {
        if (Status != GameStatus.Running)
        {
            return Status;
        }

        _currentDirection = _nextDirection;
        _directionChangeQueued = false;

        var nextHead = Head.Move(_currentDirection);
        var eatsFood = Food == nextHead;

        if (Board.IsWall(nextHead) || HitsBody(nextHead, eatsFood))
        {
            Status = GameStatus.GameOver;
            return Status;
        }

        if (!eatsFood)
        {
            RemoveTail();
        }

        AddHead(nextHead);

        if (eatsFood)
        {
            PlaceNextFood();
        }

        return Status;
    }

    private Cell Head => _snake.First?.Value ?? throw new InvalidOperationException("Snake has no head.");

    private Cell Tail => _snake.Last?.Value ?? throw new InvalidOperationException("Snake has no tail.");

    private void InitializeSnake(int initialLength, Direction direction)
    {
        if (initialLength < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(initialLength), "Snake length must be positive.");
        }

        var segments = CreateInitialSegments(initialLength, direction);
        foreach (var segment in segments)
        {
            _snake.AddLast(segment);
            _occupiedCells.Add(segment);
        }
    }

    private IReadOnlyList<Cell> CreateInitialSegments(int initialLength, Direction direction)
    {
        var segments = new List<Cell>(initialLength) { new(Board.Width / 2, Board.Height / 2) };
        var growthDirection = direction.Opposite();

        while (segments.Count < initialLength)
        {
            segments.Add(segments[^1].Move(growthDirection));
        }

        if (segments.Any(Board.IsWall))
        {
            throw new ArgumentException("Initial snake does not fit on the board.", nameof(initialLength));
        }

        return segments;
    }

    private bool HitsBody(Cell nextHead, bool eatsFood)
    {
        if (!_occupiedCells.Contains(nextHead))
        {
            return false;
        }

        return eatsFood || nextHead != Tail;
    }

    private void RemoveTail()
    {
        var tail = Tail;
        _snake.RemoveLast();
        _occupiedCells.Remove(tail);
    }

    private void AddHead(Cell head)
    {
        _snake.AddFirst(head);
        _occupiedCells.Add(head);
    }

    private void PlaceNextFood()
    {
        Food = _foodGenerator.PlaceFood(Board, _occupiedCells);
        Status = Food is null ? GameStatus.Won : GameStatus.Running;
    }
}
