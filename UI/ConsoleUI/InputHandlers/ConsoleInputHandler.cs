using gameSnake.Interfaces;
using gameSnake.UI.ConsoleUI.InputHandlers;

namespace gameSnake.UI.ConsoleUI.InputHandlers
{
    /// <summary>
    /// Обрабатывает ввод с клавиатуры в консоли.
    /// Координирует InputReader, GameCommandHandler и DirectionHandler.
    /// </summary>
    public class ConsoleInputHandler : IInputHandler
    {
        /// <summary>
        /// Считывает и обрабатывает нажатия клавиш.
        /// </summary>
        /// <param name="inputState">Часть состояния, реагирующая на ввод</param>
        /// <param name="snakeLength">Длина змейки (для проверки разворота на 180)</param>
        public void ProcessInput(IInputState inputState, int snakeLength)
        {
            ConsoleKey? key = InputReader.ReadKey();
            if (!key.HasValue) return;

            GameCommandHandler.Handle(key.Value, inputState);
            DirectionHandler.Handle(key.Value, inputState, snakeLength);
        }
    }
}
