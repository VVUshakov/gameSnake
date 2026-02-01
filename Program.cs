using gameSnake.Core;

namespace SnakeGame
{
    // ==================== КЛАССЫ ИГРОВЫХ ОБЪЕКТОВ ====================





    // ==================== СИСТЕМЫ И СЕРВИСЫ ====================



    /// <summary>
    /// Фабрика для создания игровых объектов
    /// </summary>
    public class GameObjectFactory : IGameObjectFactory
    {
        /// <summary>
        /// Создает новый объект еды
        /// </summary>
        /// <param name="width">Ширина игрового поля</param>
        /// <param name="height">Высота игрового поля</param>
        /// <returns>Новый объект еды</returns>
        public Food CreateFood(int width, int height)
        {
            return new Food(width, height);
        }

        /// <summary>
        /// Создает новый объект змейки
        /// </summary>
        /// <param name="initialLength">Начальная длина змейки</param>
        /// <returns>Новый объект змейки</returns>
        public Snake CreateSnake(int initialLength)
        {
            var settings = new GameSettings();
            return new Snake(settings.InitialSnakePosition, initialLength);
        }
    }




    /// <summary>
    /// Основной игровой движок, управляющий игровым циклом
    /// </summary>
    public class GameEngine
    {
        private readonly GameSettings _settings;
        private readonly IRenderer _renderer;
        private readonly IInputHandler _inputHandler;
        private readonly ICollisionDetector _collisionDetector;
        private readonly IGameObjectFactory _gameObjectFactory;

        private Snake _snake;
        private Food _food;
        private int _score;
        private bool _isGameOver;

        /// <summary>
        /// Создает новый игровой движок с указанными зависимостями
        /// </summary>
        /// <param name="settings">Настройки игры</param>
        /// <param name="renderer">Рендерер для отрисовки</param>
        /// <param name="inputHandler">Обработчик ввода</param>
        /// <param name="collisionDetector">Детектор столкновений</param>
        /// <param name="gameObjectFactory">Фабрика игровых объектов</param>
        public GameEngine(
            GameSettings settings,
            IRenderer renderer,
            IInputHandler inputHandler,
            ICollisionDetector collisionDetector,
            IGameObjectFactory gameObjectFactory)
        {
            _settings = settings;
            _renderer = renderer;
            _inputHandler = inputHandler;
            _collisionDetector = collisionDetector;
            _gameObjectFactory = gameObjectFactory;
        }

        /// <summary>
        /// Инициализирует игровое состояние
        /// </summary>
        public void Initialize()
        {
            _snake = _gameObjectFactory.CreateSnake(_settings.InitialSnakeLength);
            _food = _gameObjectFactory.CreateFood(_settings.Width, _settings.Height);
            _score = 0;
            _isGameOver = false;

            // Гарантия, что еда не появится внутри змейки
            while(_collisionDetector.CheckSnakeCollision(_food.Position, _snake))
            {
                _food.Respawn(_settings.Width, _settings.Height);
            }
        }

        /// <summary>
        /// Запускает основной игровой цикл
        /// </summary>
        public void Run()
        {
            Initialize();

            // Основной игровой цикл
            while(!_isGameOver)
            {
                // Обработка ввода
                _inputHandler.ProcessInput();

                // Проверка выхода
                if(_inputHandler.ShouldExit())
                {
                    _isGameOver = true;
                    break;
                }

                // Получение направления движения
                var direction = _inputHandler.GetDirection();
                if(direction.HasValue)
                {
                    _snake.SetNextDirection(direction.Value);
                }

                // Обновление игрового состояния
                Update();

                // Проверка условий завершения игры
                CheckGameOver();

                // Отрисовка текущего состояния
                _renderer.DrawGame(_snake, _food, _score);

                // Задержка для управления скоростью игры
                System.Threading.Thread.Sleep(_settings.GameSpeed);
            }

            // Отображение экрана завершения игры
            _renderer.DrawGameOver(_score);
        }

        /// <summary>
        /// Обновляет игровое состояние
        /// </summary>
        private void Update()
        {
            // Движение змейки
            _snake.Move();

            // Проверка съедения еды
            if(_snake.Position.Equals(_food.Position))
            {
                _score += _settings.FoodScoreValue;

                // Рост змейки после съедения еды
                _snake.Move(grow: true);

                // Создание новой еды
                _food.Respawn(_settings.Width, _settings.Height);

                // Гарантия, что новая еда не появится внутри змейки
                while(_collisionDetector.CheckSnakeCollision(_food.Position, _snake))
                {
                    _food.Respawn(_settings.Width, _settings.Height);
                }
            }
        }

        /// <summary>
        /// Проверяет условия завершения игры
        /// </summary>
        private void CheckGameOver()
        {
            // Проверка столкновения со стенами
            if(_collisionDetector.CheckWallCollision(_snake.Position, _settings.Width, _settings.Height))
            {
                _isGameOver = true;
                return;
            }

            // Проверка столкновения с собой
            if(_snake.CheckSelfCollision())
            {
                _isGameOver = true;
            }
        }
    }

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