namespace gameSnake.Interfaces
{
    /// <summary>
    /// Абстракция над консольным вводом для тестируемости.
    /// </summary>
    public interface IConsoleInput
    {
        bool KeyAvailable { get; }
        ConsoleKey ReadKey(bool showKeyOnScreen);
    }
}
