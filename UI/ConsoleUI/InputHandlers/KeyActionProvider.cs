using gameSnake.Interfaces;
using gameSnake.Models;

namespace gameSnake.UI.ConsoleUI.InputHandlers
{
    /// <summary>
    /// Предоставляет действия для клавиш ввода.
    /// Регистрация клавиш через словарь — добавление = одна строка.
    /// </summary>
    public static class KeyActionProvider
    {
        private static readonly Dictionary<ConsoleKey, Action<IInputState, int>> _actions = new()
        {
            [ConsoleKey.Enter]      = (s, _) => s.IsRestartRequested = true,
            [ConsoleKey.Escape]     = (s, _) => s.IsExit = true,
            [ConsoleKey.P]          = TogglePause,
            [ConsoleKey.Spacebar]   = TogglePause,
            [ConsoleKey.UpArrow]    = (s, l) => TrySetDirection(s, Direction.Up, Direction.Down, l),
            [ConsoleKey.DownArrow]  = (s, l) => TrySetDirection(s, Direction.Down, Direction.Up, l),
            [ConsoleKey.LeftArrow]  = (s, l) => TrySetDirection(s, Direction.Left, Direction.Right, l),
            [ConsoleKey.RightArrow] = (s, l) => TrySetDirection(s, Direction.Right, Direction.Left, l),
        };

        /// <summary>
        /// Возвращает действие для клавиши и применяет его.
        /// </summary>
        public static void Handle(ConsoleKey key, IInputState state, int snakeLength)
        {
            if (_actions.TryGetValue(key, out var action))
                action(state, snakeLength);
        }

        private static void TogglePause(IInputState state, int _)
        {
            state.IsPaused = !state.IsPaused;
            state.ActiveMessage = state.IsPaused ? GameMessageType.Pause : null;
        }

        private static void TrySetDirection(IInputState state, Direction newDir, Direction oppositeDir, int snakeLength)
        {
            if (state.IsPaused) return;
            if (snakeLength <= 1 || state.CurrentDirection == oppositeDir) return;
            state.CurrentDirection = newDir;
        }
    }
}
