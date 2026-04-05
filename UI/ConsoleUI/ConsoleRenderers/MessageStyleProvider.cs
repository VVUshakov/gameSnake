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
        public static string[] GetContent(GameMessageType message)
        {
            switch (message)
            {
                case GameMessageType.Pause:
                    return GameMessages.GetPauseMessage();
                case GameMessageType.GameOver:
                    return GameMessages.GetGameOverMessage();
                case GameMessageType.Win:
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
        public static ConsoleColor GetColor(GameMessageType message)
        {
            switch (message)
            {
                case GameMessageType.Pause:
                    return RenderConstants.PauseColor;
                case GameMessageType.GameOver:
                    return RenderConstants.GameOverColor;
                case GameMessageType.Win:
                    return RenderConstants.GameWinColor;
                default:
                    return RenderConstants.DefaultMessageColor;
            }
        }
    }
}
