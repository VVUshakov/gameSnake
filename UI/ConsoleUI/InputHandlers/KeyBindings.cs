using gameSnake.Interfaces;
using gameSnake.Models;

namespace gameSnake.UI.ConsoleUI.InputHandlers
{
    /// <summary>
    /// Маппинг клавиш на игровые действия.
    /// Одна точка регистрации — добавление клавиши = одна строка в словаре.
    /// </summary>
    public static class KeyBindings
    {
        private static readonly Dictionary<ConsoleKey, Action<IInputState, int>> _bindings = new()
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
        /// Применяет действие, привязанное к клавише.
        /// </summary>
        public static void Handle(ConsoleKey key, IInputState state, int snakeLength)
        {
            if (_bindings.TryGetValue(key, out var action))
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
