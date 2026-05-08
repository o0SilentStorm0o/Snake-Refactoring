using Snake.ConsoleApp;
using Snake.Core;

var settings = ConsoleGameSettings.Default;
var game = new SnakeGame(new SnakeGameOptions
{
    BoardWidth = settings.BoardWidth,
    BoardHeight = settings.BoardHeight,
    InitialLength = 5,
    InitialDirection = Direction.Right
});

var runner = new ConsoleGameRunner(
    game,
    new ConsoleSnakeRenderer(),
    new ConsoleInputReader(),
    settings);

runner.Run();
