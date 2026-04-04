namespace gameSnake.Utils
{
    /// <summary>
    /// Настраивает размер окна консоли под игровое поле.
    /// </summary>
    public static class ConsoleWindowConfigurator
    {
        private const int WidthPadding = 4;
        private const int HeightPadding = 6;

        /// <summary>
        /// Устанавливает размер окна консоли с учётом отступов.
        /// </summary>
        /// <param name="fieldWidth">Ширина игрового поля</param>
        /// <param name="fieldHeight">Высота игрового поля</param>
        public static void Configure(int fieldWidth, int fieldHeight)
        {
            Console.SetWindowSize(
                fieldWidth + WidthPadding,
                fieldHeight + HeightPadding);
        }
    }
}
