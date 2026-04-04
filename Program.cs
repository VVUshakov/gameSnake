using gameSnake.UI.ConsoleUI.InputHandlers;
using gameSnake.UI.ConsoleUI.ConsoleRenderers;
using gameSnake.Core;
using gameSnake.Interfaces;
using gameSnake.Logic.SnakeLogic;
using gameSnake.Utils;

namespace gameSnake
{
    public class Program
    {
        static void Main()
        {
            IGameRenderer renderer = new ConsoleRenderer();
            IInputHandler input = new ConsoleInputHandler();
            IGameLogic logic = new SnakeGameLogic();
            GameLoop gameLoop = new GameLoop(renderer, input, logic);

            while (true)
            {
                // Вычисляем размеры игрового поля
                (int fieldWidth, int fieldHeight) = FieldSizeCalculator.Calculate();

                // Подстраиваем консоль под игровое поле
                ConsoleWindowConfigurator.Configure(fieldWidth, fieldHeight);

                // Создаём и запускаем игру
                GameState state = GameStateFactory.Create(fieldWidth, fieldHeight);
                gameLoop.Run(state);

                if (state.Flags.IsExit || !state.Flags.IsRestartRequested) break;
            }
        }
    }
}
