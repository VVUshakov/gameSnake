using gameSnake.Models;
using gameSnake.Servises;
using gameSnake.Utils;

namespace gameSnake.UI.ConsoleUI.ConsoleRenderers
{
    /// <summary>
    /// Отвечает за отрисовку сервисных сообщений поверх игрового поля.
    /// Определяет контент и цвет по типу сообщения.
    /// </summary>
    public static class MessageRenderer
    {
        /// <summary>
        /// Отрисовывает сервисное сообщение, центрированное на игровом поле.
        /// </summary>
        /// <param name="field">Игровое поле для расчёта позиции</param>
        /// <param name="headerHeight">Высота заголовка для смещения</param>
        /// <param name="message">Тип сервисного сообщения</param>
        public static void Draw(PlayingField field, int headerHeight, GameMessage message)
        {
            string[] lines = GetContent(message);
            ConsoleColor color = GetColor(message);
            DrawCenteredMessage(field, lines, headerHeight, color);
        }

        /// <summary>
        /// Возвращает текст сообщения по его типу.
        /// </summary>
        /// <param name="message">Тип сервисного сообщения</param>
        /// <returns>Массив строк с текстом сообщения</returns>
        private static string[] GetContent(GameMessage message) => message switch
        {
            GameMessage.Pause    => GameMessages.GetPauseMessage(),
            GameMessage.GameOver => GameMessages.GetGameOverMessage(),
            GameMessage.Win      => GameMessages.GetWinMessage(),
            _                    => Array.Empty<string>()
        };

        /// <summary>
        /// Возвращает цвет сообщения по его типу.
        /// </summary>
        /// <param name="message">Тип сервисного сообщения</param>
        /// <returns>Цвет для отрисовки сообщения</returns>
        private static ConsoleColor GetColor(GameMessage message) => message switch
        {
            GameMessage.Pause    => RenderConstants.PauseColor,
            GameMessage.GameOver => RenderConstants.GameOverColor,
            GameMessage.Win      => RenderConstants.GameWinColor,
            _                    => RenderConstants.DefaultMessageColor
        };

        /// <summary>
        /// Отрисовывает центрированное сообщение на игровом поле с указанным цветом.
        /// </summary>
        /// <param name="field">Игровое поле для расчёта позиции</param>
        /// <param name="lines">Строки сообщения</param>
        /// <param name="headerHeight">Высота заголовка для смещения</param>
        /// <param name="color">Цвет текста сообщения</param>
        private static void DrawCenteredMessage(PlayingField field, string[] lines, int headerHeight, ConsoleColor color)
        {
            (int messageWidth, int messageHeight) = MessageSizer.GetSize(lines);

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
