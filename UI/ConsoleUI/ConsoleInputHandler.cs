using gameSnake.Core;
using gameSnake.Interfaces;
using gameSnake.Models;

namespace gameSnake.UI.ConsoleUI
{
    /// <summary>
    /// Обрабатывает ввод с клавиатуры в консоли.
    /// </summary>
    public class ConsoleInputHandler : IInputHandler
    {
        private bool _waitingForRestart = false;

        /// <summary>Считывает и обрабатывает нажатия клавиш клавиатуры.</summary>
        public void ProcessInput(GameState state)
        {
            if(!Console.KeyAvailable) return;
            ConsoleKey key = Console.ReadKey(true).Key;

            switch(key)
            {
                case ConsoleKey.Enter:
                    _waitingForRestart = true;
                    break;
                case ConsoleKey.P:
                case ConsoleKey.Spacebar:
                    state.IsPaused = !state.IsPaused;
                    break;
                case ConsoleKey.Escape:
                    state.IsExit = true;
                    break;
                case ConsoleKey.UpArrow:
                    if(!state.IsPaused) ChangeDirection(state, Direction.Up, Direction.Down);
                    break;
                case ConsoleKey.DownArrow:
                    if(!state.IsPaused) ChangeDirection(state, Direction.Down, Direction.Up);
                    break;
                case ConsoleKey.LeftArrow:
                    if(!state.IsPaused) ChangeDirection(state, Direction.Left, Direction.Right);
                    break;
                case ConsoleKey.RightArrow:
                    if(!state.IsPaused) ChangeDirection(state, Direction.Right, Direction.Left);
                    break;
            }

            if(_waitingForRestart)
            {
                if(key == ConsoleKey.Enter) state.IsRestartRequested = true;
                _waitingForRestart = false;
            }
        }

        /// <summary>Включает режим ожидания перезапуска (после проигрыша)</summary>
        public void WaitForRestart() => _waitingForRestart = true;

        /// <summary>Запрашивает у пользователя повторную игру после окончания</summary>
        public bool AskPlayAgain()
        {
            while(Console.KeyAvailable) Console.ReadKey(true);
            WaitForRestart();
            return false;
        }

        /// <summary>Меняет направление, запрещая разворот на 180 при длине > 1</summary>
        private static void ChangeDirection(GameState state, Direction newDir, Direction oppositeDir)
        {
            if(state.Snake.Body.Count > 1 && state.CurrentDirection != oppositeDir)
                state.CurrentDirection = newDir;
        }
    }
}
