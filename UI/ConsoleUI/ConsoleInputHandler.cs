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
        private readonly IConsoleInput _input;

        public ConsoleInputHandler(IConsoleInput? input = null)
        {
            _input = input ?? new ConsoleInput();
        }

        /// <summary>Считывает и обрабатывает нажатия клавиш.</summary>
        public void ProcessInput(GameState state)
        {
            if(!_input.KeyAvailable) return;
            ConsoleKey key = _input.ReadKey(showKeyOnScreen: false);

            switch(key)
            {
                case ConsoleKey.Enter:
                    state.IsRestartRequested = true;
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
        }

        /// <summary>Меняет направление, запрещая разворот на 180 при длине > 1</summary>
        private static void ChangeDirection(GameState state, Direction newDir, Direction oppositeDir)
        {
            if(state.Snake.Body.Count > 1 && state.CurrentDirection != oppositeDir)
                state.CurrentDirection = newDir;
        }
    }
}
