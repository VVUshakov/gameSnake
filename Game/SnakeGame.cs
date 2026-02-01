using SnakeGame.Core;
using SnakeGame.Engine;
using SnakeGame.Systems.Collision;
using SnakeGame.Systems.Factories;
using SnakeGame.Systems.Input;
using SnakeGame.Systems.Rendering;

namespace SnakeGame.Game
{
    /// <summary>
    /// Фасад для запуска игры, скрывающий сложность инициализации
    /// </summary>
    public class SnakeGame
    {
        /// <summary>
        /// Запускает игру "Змейка"
        /// </summary>
        public static void Start()
        {
            Console.Title = "Змейка";
            Console.CursorVisible = false;

            try
            {
                // Создание зависимостей
                var settings = new GameSettings();
                var renderer = new ConsoleRenderer(settings);
                var inputHandler = new ConsoleInputHandler();
                var collisionDetector = new CollisionDetector();
                var gameObjectFactory = new GameObjectFactory();

                // Создание и запуск игрового движка
                var gameEngine = new GameEngine(
                    settings,
                    renderer,
                    inputHandler,
                    collisionDetector,
                    gameObjectFactory
                );

                gameEngine.Run();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            finally
            {
                Console.CursorVisible = true;
            }

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}