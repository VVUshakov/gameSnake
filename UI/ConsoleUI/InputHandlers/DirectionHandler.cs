using gameSnake.Core;
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
        /// <param name="state">Текущее состояние игры</param>
        public static void Handle(ConsoleKey key, GameState state)
        {
            if (state.Flags.IsPaused) return;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    ChangeDirection(state, Direction.Up, Direction.Down);
                    break;
                case ConsoleKey.DownArrow:
                    ChangeDirection(state, Direction.Down, Direction.Up);
                    break;
                case ConsoleKey.LeftArrow:
                    ChangeDirection(state, Direction.Left, Direction.Right);
                    break;
                case ConsoleKey.RightArrow:
                    ChangeDirection(state, Direction.Right, Direction.Left);
                    break;
            }
        }

        private static void ChangeDirection(GameState state, Direction newDir, Direction oppositeDir)
        {
            if (state.Snake.Body.Count > 1 && state.CurrentDirection != oppositeDir)
            {
                state.CurrentDirection = newDir;
            }
        }
    }
}
