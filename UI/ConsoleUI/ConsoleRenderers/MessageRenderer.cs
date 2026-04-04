using gameSnake.Models;
using gameSnake.Utils;

namespace gameSnake.UI.ConsoleUI.ConsoleRenderers
{
    /// <summary>
    /// Отвечает за отрисовку сервисных сообщений поверх игрового поля.
    /// Не зависит от источника текста — получает строки как параметр.
    /// </summary>
    public static class MessageRenderer
    {
        /// <summary>
        /// Отрисовывает центрированное сообщение на игровом поле.
        /// </summary>
        /// <param name="field">Игровое поле для расчёта позиции</param>
        /// <param name="headerHeight">Высота заголовка для смещения</param>
        /// <param name="lines">Строки сообщения</param>
        /// <param name="color">Цвет текста</param>
        public static void Draw(PlayingField field, int headerHeight, string[] lines, ConsoleColor color)
        {
            int messageWidth = MessageSizer.GetWidth(lines);
            int messageHeight = MessageSizer.GetHeight(lines);

            Point startPosition = PositionCalculator.CalculateCenteredMessagePosition(
                field.Width, field.Height + headerHeight, messageWidth, messageHeight);

            if (startPosition.X < 0 || startPosition.Y < headerHeight) return;

            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;

            for (int i = 0; i < lines.Length; i++)
            {
                int y = startPosition.Y + i;
                if (y >= headerHeight && y < headerHeight + field.Height)
                {
                    Console.SetCursorPosition(startPosition.X, y);
                    Console.Write(lines[i]);
                }
            }
            Console.ForegroundColor = originalColor;
        }
    }
}
