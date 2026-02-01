using gameSnake.Core;

namespace SnakeGame
{
    // ==================== КЛАССЫ ИГРОВЫХ ОБЪЕКТОВ ====================





    // ==================== СИСТЕМЫ И СЕРВИСЫ ====================










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
                var settings = new GameSettings();
                var renderer = new ConsoleRenderer(settings);
                var inputHandler = new ConsoleInputHandler();
                var collisionDetector = new CollisionDetector();
                var gameObjectFactory = new GameObjectFactory();

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

    /// <summary>
    /// Главный класс приложения
    /// </summary>
    class Program
    {
        /// <summary>
        /// Точка входа в приложение
        /// </summary>
        static void Main()
        {
            SnakeGame.Start();
        }
    }
}