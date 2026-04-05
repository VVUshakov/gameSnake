using gameSnake.Interfaces;

namespace gameSnake.UI.ConsoleUI
{
    /// <summary>
    /// Настраивает окно консоли: размер, видимость курсора.
    /// </summary>
    public class ConsoleWindowConfigurator : IWindowConfigurator
    {
        private const int WidthPadding = 4;
        private const int HeightPadding = 6;

        /// <summary>
        /// Устанавливает размер окна консоли и скрывает курсор.
        /// </summary>
        /// <param name="fieldWidth">Ширина игрового поля</param>
        /// <param name="fieldHeight">Высота игрового поля</param>
        public void Configure(int fieldWidth, int fieldHeight)
        {
            Console.SetWindowSize(
                fieldWidth + WidthPadding,
                fieldHeight + HeightPadding);
            Console.CursorVisible = false;
        }
    }
}
