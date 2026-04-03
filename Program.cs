using Snake.UI.ConsoleUI;
using Snake.Core;
using Snake.Interfaces;
using Snake.Logic;

namespace Snake
{
    /// <summary>
    /// Точка входа в приложение
    /// </summary>
    public class Program
    {
        static void Main()
        {
            IGameRenderer renderer = new ConsoleRenderer();
            IInputHandler input = new ConsoleInputHandler();
            IGameLogic logic = new SnakeGameLogic();

            GameLoop gameLoop = new GameLoop(renderer, input, logic, () => GameFactory.CreateGameState());
            gameLoop.Run();
        }
    }
}
