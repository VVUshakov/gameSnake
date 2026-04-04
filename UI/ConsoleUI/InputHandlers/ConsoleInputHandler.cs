using gameSnake.Core;
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
        /// Считывает и обрабатывает нажатия клавиш
        /// </summary>
        /// <param name="state">Текущее состояние игры</param>
        public void ProcessInput(GameState state)
        {
            ConsoleKey? key = InputReader.ReadKey();
            if (!key.HasValue) return;

            GameCommandHandler.Handle(key.Value, state);
            DirectionHandler.Handle(key.Value, state);
        }
    }
}
