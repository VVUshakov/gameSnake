using gameSnake.Interfaces;

namespace gameSnake.UI.ConsoleUI
{
    /// <summary>
    /// Реализация ввода через System.Console.
    /// </summary>
    public class ConsoleInput : IConsoleInput
    {
        public bool KeyAvailable => System.Console.KeyAvailable;
        public System.ConsoleKey ReadKey(bool showKeyOnScreen) => System.Console.ReadKey(!showKeyOnScreen).Key;
    }
}
