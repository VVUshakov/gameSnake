using gameSnake.Core;
using gameSnake.Interfaces;
using gameSnake.Models;

namespace gameSnake.UI.ConsoleUI
{
    /// <summary>
    /// Обрабатывает ввод с клавиатуры в консоли.
    /// Управляет движением змейки, паузой и выходом из игры.
    /// </summary>
    public class ConsoleInputHandler : IInputHandler
    {
        private bool _waitingForRestart = false;

        /// <summary>
        /// Считывает и обрабатывает нажатия клавиш клавиатуры.
        /// </summary>
        /// <param name="state">Текущее состояние игры</param>
        public void ProcessInput(GameState state)
        {
            // Если нет нажатых клавиш - выходим
            if (!Console.KeyAvailable) return;

            // Читаем клавишу (true - не отображать её на экране)
            ConsoleKey key = Console.ReadKey(true).Key;

            // Обрабатываем все клавиши в едином switch
            switch (key)
            {
                case ConsoleKey.Enter:
                    // Подтверждение перезапуска игры
                    _waitingForRestart = true;
                    break;

                case ConsoleKey.P:
                case ConsoleKey.Spacebar:
                    // Пауза
                    state.IsPaused = !state.IsPaused;
                    break;

                case ConsoleKey.Escape:
                    // Выход (в том числе из паузы)
                    state.IsExit = true;
                    break;

                case ConsoleKey.UpArrow:
                    // Движение вверх (если не на паузе)
                    if (!state.IsPaused)
                        ChangeDirection(state, Direction.Up, Direction.Down);
                    break;

                case ConsoleKey.DownArrow:
                    // Движение вниз (если не на паузе)
                    if (!state.IsPaused)
                        ChangeDirection(state, Direction.Down, Direction.Up);
                    break;

                case ConsoleKey.LeftArrow:
                    // Движение влево (если не на паузе)
                    if (!state.IsPaused)
                        ChangeDirection(state, Direction.Left, Direction.Right);
                    break;

                case ConsoleKey.RightArrow:
                    // Движение вправо (если не на паузе)
                    if (!state.IsPaused)
                        ChangeDirection(state, Direction.Right, Direction.Left);
                    break;
            }

            // Обработка перезапуска после проигрыша (клавиша Enter)
            if (_waitingForRestart)
            {
                // Если нажат Enter, изменяем глобальный флаг перезапуска игры
                if (key == ConsoleKey.Enter) { state.IsRestartRequested = true; }

                // Изменяем локальный флаг перезапуска игры
                _waitingForRestart = false;
            }
        }

        /// <summary>
        /// Включает режим ожидания перезапуска (после проигрыша)
        /// </summary>
        public void WaitForRestart()
        {
            _waitingForRestart = true;
        }

        /// <summary>
        /// Запрашивает у пользователя повторную игру после окончания
        /// </summary>
        /// <returns>true, если пользователь хочет сыграть ещё, false в противном случае</returns>
        public bool AskPlayAgain()
        {
            // Ждём, пока пользователь отпустит предыдущие клавиши
            while(Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }

            WaitForRestart();
            return false; // Возвращаем false, реальное решение будет в ProcessInput
        }

        /// <summary>
        /// Меняет направление движения змейки, если это не противоречит правилам.
        /// Разворот на 180° запрещён, если длина змейки больше 1 сегмента.
        /// </summary>
        /// <param name="state">Состояние игры</param>
        /// <param name="newDirection">Новое направление движения</param>
        /// <param name="oppositeDirection">Противоположное направление (запрещено для разворота)</param>
        private static void ChangeDirection(GameState state, Direction newDirection, Direction oppositeDirection)
        {
            // Разворот на 180° запрещён, если длина змейки больше 1
            if(state.Snake.Body.Count > 1 && state.CurrentDirection != oppositeDirection)
            {
                state.CurrentDirection = newDirection;
            }
        }
    }
}