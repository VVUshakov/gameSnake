using gameSnake.Core;

namespace gameSnake.UI.ConsoleUI.InputHandlers
{
    /// <summary>
    /// Обрабатывает игровые команды: рестарт, пауза, выход и т.п.
    /// </summary>
    public static class GameCommandHandler
    {
        /// <summary>
        /// Обрабатывает игровую команду
        /// </summary>
        /// <param name="key">Нажатая клавиша</param>
        /// <param name="state">Текущее состояние игры</param>
        public static void Handle(ConsoleKey key, GameState state)
        {
            switch (key)
            {
                case ConsoleKey.Enter:
                    state.IsRestartRequested = true;
                    break;
                case ConsoleKey.P:
                case ConsoleKey.Spacebar:
                    state.IsPaused = !state.IsPaused;
                    break;
                case ConsoleKey.Escape:
                    state.IsExit = true;
                    break;
            }
        }
    }
}
