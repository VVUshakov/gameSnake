using gameSnake.Interfaces;
using gameSnake.Models;

namespace gameSnake.UI.ConsoleUI.InputHandlers
{
    /// <summary>
    /// Предоставляет действия для клавиш ввода.
    /// Хранит словарь маппинга (установления соответствия) <see cref="ConsoleKey"/> к делегатам действий.
    /// Регистрация новой клавиши = одна строка в словаре — без изменения методов.
    /// </summary>
    public static class KeyActionProvider
    {
        /// <summary>
        /// Словарь соответствия клавиш действиям.
        /// Ключ — клавиша, значение — делегат, принимающий <see cref="IInputState"/> и длину змейки.
        /// </summary>
        private static readonly Dictionary<ConsoleKey, Action<IInputState, int>> _actions = new()
        {
            // Команды
            [ConsoleKey.Enter]    = (state, _) => state.IsRestartRequested = true,
            [ConsoleKey.Escape]   = (state, _) => state.IsExit = true,
            [ConsoleKey.P]        = TogglePause,
            [ConsoleKey.Spacebar] = TogglePause,

            // Направления
            [ConsoleKey.UpArrow]    = (state, snakeLength) => TrySetDirection(state, Direction.Up, Direction.Down, snakeLength),
            [ConsoleKey.DownArrow]  = (state, snakeLength) => TrySetDirection(state, Direction.Down, Direction.Up, snakeLength),
            [ConsoleKey.LeftArrow]  = (state, snakeLength) => TrySetDirection(state, Direction.Left, Direction.Right, snakeLength),
            [ConsoleKey.RightArrow] = (state, snakeLength) => TrySetDirection(state, Direction.Right, Direction.Left, snakeLength),
        };

        /// <summary>
        /// Находит и применяет действие, привязанное к указанной клавише.
        /// Если клавиша не зарегистрирована — ничего не делает.
        /// </summary>
        /// <param name="key">Нажатая клавиша</param>
        /// <param name="state">Состояние ввода, которое может быть изменено действием</param>
        /// <param name="snakeLength">Длина змейки (используется для проверки разворота на 180°)</param>
        public static void Handle(ConsoleKey key, IInputState state, int snakeLength)
        {
            if (_actions.TryGetValue(key, out var action))
                action(state, snakeLength);
        }

        /// <summary>
        /// Переключает режим паузы: включает/выключает и устанавливает сервисное сообщение.
        /// </summary>
        private static void TogglePause(IInputState state, int _)
        {
            state.IsPaused = !state.IsPaused;
            state.ActiveMessage = state.IsPaused ? GameMessageType.Pause : null;
        }

        /// <summary>
        /// Устанавливает новое направление змейки, запрещая разворот на 180°.
        /// Пропускает ввод, если игра на паузе.
        /// </summary>
        /// <param name="state">Состояние ввода</param>
        /// <param name="newDir">Новое направление движения</param>
        /// <param name="oppositeDir">Противоположное направление (разворот запрещён)</param>
        /// <param name="snakeLength">Длина змейки (разворот разрешён при длине 1)</param>
        private static void TrySetDirection(IInputState state, Direction newDir, Direction oppositeDir, int snakeLength)
        {
            if (state.IsPaused) return;
            if (snakeLength <= 1 || state.CurrentDirection == oppositeDir) return;
            state.CurrentDirection = newDir;
        }
    }
}
