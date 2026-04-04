using gameSnake.Core;
using gameSnake.Models;

namespace gameSnake.UI.ConsoleUI.InputHandlers
{
    /// <summary>
    /// Обрабатывает игровые команды: рестарт, пауза, выход.
    /// </summary>
    public static class GameCommandHandler
    {
        /// <summary>
        /// Обрабатывает игровую команду.
        /// </summary>
        /// <param name="key">Нажатая клавиша</param>
        /// <param name="state">Текущее состояние игры</param>
        public static void Handle(ConsoleKey key, GameState state)
        {
            switch (key)
            {
                case ConsoleKey.Enter:
                    state.Flags.IsRestartRequested = true;
                    break;
                case ConsoleKey.P:
                case ConsoleKey.Spacebar:
                    state.Flags.IsPaused = !state.Flags.IsPaused;
                    state.ActiveMessage = state.Flags.IsPaused ? GameMessage.Pause : null;
                    break;
                case ConsoleKey.Escape:
                    state.Flags.IsExit = true;
                    break;
            }
        }
    }
}
