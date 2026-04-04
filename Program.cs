using gameSnake.UI.ConsoleUI.InputHandlers;
using gameSnake.UI.ConsoleUI.ConsoleRenderers;
using gameSnake.Core;
using gameSnake.Interfaces;
using gameSnake.Logic;
using gameSnake.Servises;
using gameSnake.Utils;

namespace gameSnake
{
    public class Program
    {
        // Запас консоли: рамка + заголовок + отступы
        private const int ConsoleWidthPadding = 4;
        private const int ConsoleHeightPadding = 6;

        static void Main()
        {
            IGameRenderer renderer = new ConsoleRenderer();
            IInputHandler input = new ConsoleInputHandler();
            IGameLogic logic = new SnakeGameLogic();
            GameLoop gameLoop = new GameLoop(renderer, input, logic);

            while (true)
            {
                // Вычисляем размеры поля на основе сообщений
                var messages = ServiseMessange.GetAllMessages();
                int fieldWidth = MessageSizer.GetMaxWidth(messages) + 4;
                int fieldHeight = MessageSizer.GetMaxHeight(messages) + 4;

                // Подстраиваем консоль под поле + запас
                Console.SetWindowSize(
                    fieldWidth + ConsoleWidthPadding,
                    fieldHeight + ConsoleHeightPadding);

                GameState state = GameStateFactory.Create(fieldWidth, fieldHeight);

                gameLoop.Run(state);

                if (state.IsExit || !state.IsRestartRequested) break;
            }
        }
    }
}
