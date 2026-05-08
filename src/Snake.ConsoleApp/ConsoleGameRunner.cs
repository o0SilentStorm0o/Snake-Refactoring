using System.Diagnostics;
using Snake.Core;

namespace Snake.ConsoleApp;

internal sealed class ConsoleGameRunner
{
    private readonly SnakeGame _game;
    private readonly ConsoleInputReader _inputReader;
    private readonly ConsoleSnakeRenderer _renderer;
    private readonly ConsoleGameSettings _settings;

    public ConsoleGameRunner(
        SnakeGame game,
        ConsoleSnakeRenderer renderer,
        ConsoleInputReader inputReader,
        ConsoleGameSettings settings)
    {
        _game = game;
        _renderer = renderer;
        _inputReader = inputReader;
        _settings = settings;
    }

    public void Run()
    {
        PrepareConsole();

        try
        {
            while (_game.Status == GameStatus.Running)
            {
                _renderer.Render(_game.CreateSnapshot());
                ReadInputUntilNextTick();
                _game.Tick();
            }

            var finalSnapshot = _game.CreateSnapshot();
            _renderer.Render(finalSnapshot);
            _renderer.RenderFinishedGame(finalSnapshot);
        }
        finally
        {
            Console.ResetColor();
            Console.CursorVisible = true;
        }
    }

    private void PrepareConsole()
    {
        Console.CursorVisible = false;
        Console.Clear();

        if (Console.IsOutputRedirected || !OperatingSystem.IsWindows())
        {
            return;
        }

        try
        {
            var width = Math.Max(_settings.BoardWidth, Console.WindowWidth);
            var height = Math.Max(_settings.BoardHeight + 1, Console.WindowHeight);
            Console.SetWindowSize(width, height);
        }
        catch (IOException)
        {
        }
        catch (PlatformNotSupportedException)
        {
        }
        catch (ArgumentOutOfRangeException)
        {
        }
    }

    private void ReadInputUntilNextTick()
    {
        var elapsedTime = Stopwatch.StartNew();

        while (elapsedTime.Elapsed < _settings.TickDelay)
        {
            foreach (var direction in _inputReader.ReadDirections())
            {
                _game.TryChangeDirection(direction);
            }

            Thread.Sleep(_settings.InputPollingDelay);
        }
    }
}
