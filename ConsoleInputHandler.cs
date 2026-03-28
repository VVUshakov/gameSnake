namespace Snake
{
    /// <summary>
    /// Обрабатывает ввод с клавиатуры в консоли.
    /// Управляет движением змейки, паузой и выходом из игры.
    /// </summary>
    public class ConsoleInputHandler : IInputHandler
    {
        /// <summary>
        /// Считывает и обрабатывает нажатия клавиш клавиатуры.
        /// </summary>
        /// <param name="state">Текущее состояние игры</param>
        public void ProcessInput(GameState state)
        {
            // Если нет нажатых клавиш - выходим
            if(!Console.KeyAvailable) return;

            // Читаем клавишу (true - не отображать её на экране)
            ConsoleKey key = Console.ReadKey(true).Key;

            // Обработка паузы (клавиша P)
            if(key == ConsoleKey.P)
            {
                state.IsPaused = !state.IsPaused;
                return;
            }

            // Если игра на паузе - не обрабатываем другие клавиши, кроме выход (Escape)
            if (state.IsPaused)
            {
                if(key == ConsoleKey.Escape)
                    state.IsExit = true;
                return;
            }

            // Обрабатываем стрелки
            switch(key)
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

                case ConsoleKey.Escape:
                    state.IsExit = true;
                    break;
            }
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