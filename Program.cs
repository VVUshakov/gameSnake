using Snake.UI.ConsoleUI;
using Snake.Core;
using Snake.Interfaces;
using Snake.Logic;
using Snake.Models;

namespace Snake
{
    /// <summary>
    /// Точка входа в приложение
    /// </summary>
    public class Program
    {
        static void Main()
        {
            // Создаём зависимости
            IGameRenderer renderer = new ConsoleRenderer();
            IInputHandler input = new ConsoleInputHandler();
            IGameLogic logic = new SnakeGameLogic();

            // Создаём и запускаем игровой цикл
            GameLoop gameLoop = new GameLoop(renderer, input, logic);
            gameLoop.RunWithRestart();
        }
    }
}