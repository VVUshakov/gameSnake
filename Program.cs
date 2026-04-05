using State = gameSnake.Core.State.GameState;
using gameSnake.UI.ConsoleUI.InputHandlers;
using gameSnake.UI.ConsoleUI.ConsoleRenderers;
using gameSnake.Core;
using gameSnake.Interfaces;
using gameSnake.Logic.SnakeLogic;
using gameSnake.Servises;
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
                // Вычисляем максимальные размеры сервисных сообщений
                var messages = MessageRegistry.GetAll();
                var (maxMessageWidth, maxMessageHeight) = MessageSizer.GetMaxSize(messages);

                // Вычисляем размеры игрового поля (с учетом габаритов сервисных сообщений)
                (int fieldWidth, int fieldHeight) = FieldSizeCalculator.Calculate(maxMessageWidth, maxMessageHeight);

                // Подстраиваем консоль под размер игрового поля
                ConsoleWindowConfigurator.Configure(fieldWidth, fieldHeight);

                // Создаём и запускаем игру
                State state = GameStateFactory.Create(fieldWidth, fieldHeight);
                gameLoop.Run(state);

                if (state.Flags.IsExit || !state.Flags.IsRestartRequested) break;
            }
        }
    }
}
