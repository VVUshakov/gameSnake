using State = gameSnake.Core.State.GameState;
using ITimer = gameSnake.Interfaces.ITimer;
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
            ITimer timer = new SystemTimer();
            GameLoop gameLoop = new GameLoop(renderer, input, logic, timer);

            while (true)
            {
                // Вычисляем размеры сообщений
                var messages = MessageRegistry.GetAll();
                var (maxMsgWidth, maxMsgHeight) = MessageSizer.GetMaxSize(messages);

                // Вычисляем размеры поля
                (int fieldWidth, int fieldHeight) = FieldSizeCalculator.Calculate(maxMsgWidth, maxMsgHeight);

                // Подстраиваем консоль под поле
                ConsoleWindowConfigurator.Configure(fieldWidth, fieldHeight);

                // Создаём и запускаем игру
                State state = GameStateFactory.Create(fieldWidth, fieldHeight);
                gameLoop.Run(state);

                if (state.Flags.IsExit || !state.Flags.IsRestartRequested) break;
            }
        }
    }
}
