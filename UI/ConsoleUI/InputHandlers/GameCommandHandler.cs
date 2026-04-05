using gameSnake.Core.State;
using gameSnake.Interfaces;
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
        /// <param name="inputState">Часть состояния, реагирующая на ввод</param>
        public static void Handle(ConsoleKey key, IInputState inputState)
        {
            switch (key)
            {
                case ConsoleKey.Enter:
                    inputState.IsRestartRequested = true;
                    break;
                case ConsoleKey.P:
                case ConsoleKey.Spacebar:
                    inputState.IsPaused = !inputState.IsPaused;
                    inputState.ActiveMessage = inputState.IsPaused ? GameMessage.Pause : null;
                    break;
                case ConsoleKey.Escape:
                    inputState.IsExit = true;
                    break;
            }
        }
    }
}
