using ITimer = gameSnake.Interfaces.ITimer;
using gameSnake.UI.ConsoleUI;
using gameSnake.UI.ConsoleUI.InputHandlers;
using gameSnake.UI.ConsoleUI.ConsoleRenderers;
using gameSnake.Core.Engine;
using gameSnake.Core;
using gameSnake.Interfaces;
using gameSnake.Logic.SnakeLogic;

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
            IWindowConfigurator windowConfigurator = new ConsoleWindowConfigurator();
            
            var game = new Game(renderer, input, logic, timer, windowConfigurator);
            game.Run();
        }
    }
}
