using gameSnake.Models;
using gameSnake.Servises.MessageServise;

namespace gameSnake.UI.ConsoleUI.ConsoleRenderers
{
    /// <summary>
    /// Определяет контент и цвет сервисного сообщения по его типу.
    /// Регистрация стилей через словарь — добавление нового сообщения = одна строка.
    /// </summary>
    public static class MessageStyleProvider
    {
        private static readonly Dictionary<GameMessageType, (ConsoleColor Color, Func<string[]> Content)> _styles = new()
        {
            [GameMessageType.Pause]    = (RenderConstants.PauseColor,    GameMessages.GetPauseMessage),
            [GameMessageType.GameOver] = (RenderConstants.GameOverColor, GameMessages.GetGameOverMessage),
            [GameMessageType.Win]      = (RenderConstants.GameWinColor,  GameMessages.GetWinMessage),
        };

        /// <summary>
        /// Возвращает текст сообщения по его типу.
        /// </summary>
        public static string[] GetContent(GameMessageType message)
            => _styles.TryGetValue(message, out var s) ? s.Content() : Array.Empty<string>();

        /// <summary>
        /// Возвращает цвет сообщения по его типу.
        /// </summary>
        public static ConsoleColor GetColor(GameMessageType message)
            => _styles.TryGetValue(message, out var s) ? s.Color : RenderConstants.DefaultMessageColor;
    }
}
