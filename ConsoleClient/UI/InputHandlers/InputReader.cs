namespace ConsoleClient.UI.InputHandlers
{
    /// <summary>
    /// Читает нажатую клавишу из консоли.
    /// </summary>
    public static class InputReader
    {
        /// <summary>
        /// Возвращает нажатую клавишу или null, если ввода нет.
        /// </summary>
        public static ConsoleKey? ReadKey()
        {
            if (!Console.KeyAvailable) return null;
            return Console.ReadKey(intercept: true).Key;
        }
    }
}
