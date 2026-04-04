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

        private static string[] GetContent(GameMessage message) => message switch
        {
            GameMessage.Pause    => ServiseMessange.GetPauseMessange(),
            GameMessage.GameOver => ServiseMessange.GetGameOverMessange(),
            GameMessage.Win      => ServiseMessange.GetGameWinMessange(),
            _                    => Array.Empty<string>()
        };

        private static ConsoleColor GetColor(GameMessage message) => message switch
        {
            GameMessage.Pause    => RenderConstants.PauseColor,
            GameMessage.GameOver => RenderConstants.GameOverColor,
            GameMessage.Win      => RenderConstants.GameWinColor,
            _                    => RenderConstants.DefaultMessageColor
        };

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
