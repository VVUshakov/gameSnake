using gameSnake.Models;
using gameSnake.Servises;
using gameSnake.Utils;

namespace gameSnake.UI.ConsoleUI.ConsoleRenderers
{
    /// <summary>
    /// Отвечает за отрисовку сервисных сообщений поверх игрового поля.
    /// Поддерживает сообщения о паузе, победе и проигрыше с разными цветами.
    /// </summary>
    public static class MessageRenderer
    {
        /// <summary>
        /// Отрисовывает сообщение о проигрыше.
        /// </summary>
        /// <param name="field">Игровое поле для центрирования</param>
        /// <param name="headerHeight">Высота заголовка для смещения</param>
        public static void DrawGameOver(PlayingField field, int headerHeight)
        {
            string[] message = ServiseMessange.GetGameOverMessange();
            DrawCenteredMessage(field, message, headerHeight, RenderConstants.GameOverColor);
        }

        /// <summary>
        /// Отрисовывает сообщение о победе.
        /// </summary>
        /// <param name="field">Игровое поле для центрирования</param>
        /// <param name="headerHeight">Высота заголовка для смещения</param>
        public static void DrawGameWin(PlayingField field, int headerHeight)
        {
            string[] message = ServiseMessange.GetGameWinMessange();
            DrawCenteredMessage(field, message, headerHeight, RenderConstants.GameWinColor);
        }

        /// <summary>
        /// Отрисовывает сообщение о паузе.
        /// </summary>
        /// <param name="field">Игровое поле для центрирования</param>
        /// <param name="headerHeight">Высота заголовка для смещения</param>
        public static void DrawPause(PlayingField field, int headerHeight)
        {
            string[] message = ServiseMessange.GetPauseMessange();
            DrawCenteredMessage(field, message, headerHeight, RenderConstants.PauseColor);
        }

        /// <summary>
        /// Отрисовывает центрированное сообщение на игровом поле с указанным цветом.
        /// </summary>
        /// <param name="field">Игровое поле для расчёта позиции</param>
        /// <param name="lines">Строки сообщения</param>
        /// <param name="headerHeight">Высота заголовка для смещения</param>
        /// <param name="color">Цвет текста сообщения</param>
        private static void DrawCenteredMessage(PlayingField field, string[] lines, int headerHeight, ConsoleColor color)
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
