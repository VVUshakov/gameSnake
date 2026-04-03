using gameSnake.UI.ConsoleUI;
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
                try
                {
                    GameState state = GameFactory.CreateGameState();
                    gameLoop.Run(state);

                    if(state.IsExit || !state.IsRestartRequested) break;
                }
                catch(InvalidOperationException ex)
                {
                    Clear();
                    WriteLine("ОШИБКА: " + ex.Message);
                    WriteLine("Нажмите любую клавишу для выхода...");
                    ReadKey();
                    break;
                }
            }
        }
    }
}
