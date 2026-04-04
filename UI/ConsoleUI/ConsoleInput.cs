using gameSnake.Interfaces;

namespace gameSnake.UI.ConsoleUI
{
    /// <summary>
    /// Реализация ввода через System.Console.
    /// </summary>
    public class ConsoleInput : IConsoleInput
    {
        public bool KeyAvailable => Console.KeyAvailable;
        public ConsoleKey ReadKey(bool showKeyOnScreen)
        {
            return Console.ReadKey(!showKeyOnScreen).Key;
        }
    }
}
