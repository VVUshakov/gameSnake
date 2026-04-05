using gameSnake.Models;
using gameSnake.Servises;

namespace gameSnake.UI.ConsoleUI.ConsoleRenderers
{
    /// <summary>
    /// Определяет контент и цвет сервисного сообщения по его типу.
    /// </summary>
    public static class MessageStyleProvider
    {
        /// <summary>
        /// Возвращает текст сообщения по его типу.
        /// </summary>
        /// <param name="message">Тип сервисного сообщения</param>
        /// <returns>Массив строк с текстом сообщения</returns>
        public static string[] GetContent(GameMessage message)
        {
            switch (message)
            {
                case GameMessage.Pause:
                    return GameMessages.GetPauseMessage();
                case GameMessage.GameOver:
                    return GameMessages.GetGameOverMessage();
                case GameMessage.Win:
                    return GameMessages.GetWinMessage();
                default:
                    return Array.Empty<string>();
            }
        }

        /// <summary>
        /// Возвращает цвет сообщения по его типу.
        /// </summary>
        /// <param name="message">Тип сервисного сообщения</param>
        /// <returns>Цвет для отрисовки сообщения</returns>
        public static ConsoleColor GetColor(GameMessage message)
        {
            switch (message)
            {
                case GameMessage.Pause:
                    return RenderConstants.PauseColor;
                case GameMessage.GameOver:
                    return RenderConstants.GameOverColor;
                case GameMessage.Win:
                    return RenderConstants.GameWinColor;
                default:
                    return RenderConstants.DefaultMessageColor;
            }
        }
    }
}
