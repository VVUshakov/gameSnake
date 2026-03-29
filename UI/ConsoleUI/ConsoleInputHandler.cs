using Snake.Core;
using Snake.Interfaces;
using Snake.Models;

namespace Snake.UI.ConsoleUI
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
            if(!Console.KeyAvailable) return;

            // Читаем клавишу (true - не отображать её на экране)
            ConsoleKey key = Console.ReadKey(true).Key;

            // Обработка перезапуска после проигрыша (клавиши Y/N)
            if(_waitingForRestart)
            {
                // Получаем символ клавиши для поддержки русской раскладки
                char keyChar = Console.ReadKey(true).KeyChar;
                
                if(keyChar == 'y' || keyChar == 'Y' || keyChar == 'н' || keyChar == 'Н')
                {
                    _waitingForRestart = false;
                    state.IsRestartRequested = true;
                }
                else if(keyChar == 'n' || keyChar == 'N' || keyChar == 'т' || keyChar == 'Т' || keyChar == 27) // 27 = Escape
                {
                    _waitingForRestart = false;
                    state.IsExit = true;
                }
                return;
            }

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