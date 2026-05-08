namespace Snake.ConsoleApp;

internal sealed record ConsoleGameSettings
{
    public static ConsoleGameSettings Default { get; } = new();

    public int BoardWidth { get; init; } = 32;

    public int BoardHeight { get; init; } = 16;

    public TimeSpan TickDelay { get; init; } = TimeSpan.FromMilliseconds(500);

    public TimeSpan InputPollingDelay { get; init; } = TimeSpan.FromMilliseconds(10);
}
