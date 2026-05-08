using Snake.Core;

namespace Snake.ConsoleApp;

internal sealed class ConsoleSnakeRenderer
{
    private const char Food = '*';
    private const char SnakeBody = 'o';
    private const char SnakeHead = 'O';
    private const char Wall = '#';

    public void Render(GameSnapshot snapshot)
    {
        Console.Clear();
        DrawBorder(snapshot.Board);
        DrawFood(snapshot.Food);
        DrawSnake(snapshot.Snake);
        Console.ResetColor();
    }

    public void RenderFinishedGame(GameSnapshot snapshot)
    {
        var message = snapshot.Status == GameStatus.Won
            ? $"You win, Score: {snapshot.Score}"
            : $"Game over, Score: {snapshot.Score}";

        var text = FitInsideBoard(message, snapshot.Board.Width);
        var x = Math.Max(1, (snapshot.Board.Width - text.Length) / 2);
        var y = snapshot.Board.Height / 2;

        Console.ForegroundColor = ConsoleColor.White;
        Console.SetCursorPosition(x, y);
        Console.Write(text);
        Console.SetCursorPosition(0, snapshot.Board.Height);
        Console.ResetColor();
    }

    private static void DrawBorder(GameBoard board)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;

        for (var x = 0; x < board.Width; x++)
        {
            WriteAt(new Cell(x, 0), Wall);
            WriteAt(new Cell(x, board.Height - 1), Wall);
        }

        for (var y = 0; y < board.Height; y++)
        {
            WriteAt(new Cell(0, y), Wall);
            WriteAt(new Cell(board.Width - 1, y), Wall);
        }
    }

    private static void DrawFood(Cell? food)
    {
        if (food is null)
        {
            return;
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
        WriteAt(food.Value, Food);
    }

    private static void DrawSnake(IReadOnlyList<Cell> snake)
    {
        Console.ForegroundColor = ConsoleColor.Green;

        foreach (var segment in snake.Skip(1))
        {
            WriteAt(segment, SnakeBody);
        }

        Console.ForegroundColor = ConsoleColor.Red;
        WriteAt(snake[0], SnakeHead);
    }

    private static string FitInsideBoard(string message, int boardWidth)
    {
        var maxLength = Math.Max(0, boardWidth - 2);
        return message.Length <= maxLength ? message : message[..maxLength];
    }

    private static void WriteAt(Cell cell, char value)
    {
        Console.SetCursorPosition(cell.X, cell.Y);
        Console.Write(value);
    }
}
