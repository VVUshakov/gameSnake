using gameSnake.UI.ConsoleUI;
using gameSnake.UI.ConsoleUI.ConsoleRenderers;
using gameSnake.Core;
using gameSnake.Interfaces;
using gameSnake.Logic;
using static System.Console;

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

            while(true)
            {
                GameState state = GameFactory.CreateGameState();
                gameLoop.Run(state);

                if(state.IsExit || !state.IsRestartRequested) break;
            }
        }
    }
}
