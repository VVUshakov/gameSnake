using gameSnake.Interfaces;
using gameSnake.Models;

namespace gameSnake.UI.ConsoleUI.InputHandlers
{
    /// <summary>
    /// Обрабатывает клавиши направления (стрелки).
    /// Запрещает разворот на 180 градусов при длине змейки > 1.
    /// </summary>
    public static class DirectionHandler
    {
        /// <summary>
        /// Обрабатывает клавишу направления.
        /// </summary>
        /// <param name="key">Нажатая клавиша</param>
        /// <param name="inputState">Часть состояния, реагирующая на ввод</param>
        /// <param name="snakeLength">Длина змейки (для проверки разворота на 180)</param>
        public static void Handle(ConsoleKey key, IInputState inputState, int snakeLength)
        {
            if (inputState.IsPaused) return;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    ChangeDirection(inputState, Direction.Up, Direction.Down, snakeLength);
                    break;
                case ConsoleKey.DownArrow:
                    ChangeDirection(inputState, Direction.Down, Direction.Up, snakeLength);
                    break;
                case ConsoleKey.LeftArrow:
                    ChangeDirection(inputState, Direction.Left, Direction.Right, snakeLength);
                    break;
                case ConsoleKey.RightArrow:
                    ChangeDirection(inputState, Direction.Right, Direction.Left, snakeLength);
                    break;
            }
        }

        /// <summary>
        /// Меняет направление змейки, запрещая разворот на 180 градусов.
        /// Разворот запрещён, если длина змейки больше 1 и текущее направление
        /// не противоположно новому.
        /// </summary>
        /// <param name="inputState">Часть состояния, реагирующая на ввод</param>
        /// <param name="newDir">Новое направление движения</param>
        /// <param name="oppositeDir">Противоположное направление (запрещённое)</param>
        /// <param name="snakeLength">Длина змейки</param>
        private static void ChangeDirection(IInputState inputState, Direction newDir, Direction oppositeDir, int snakeLength)
        {
            if (snakeLength > 1 && inputState.CurrentDirection != oppositeDir)
            {
                inputState.CurrentDirection = newDir;
            }
        }
    }
}
