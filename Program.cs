using gameSnake.Core;

namespace SnakeGame
{
    // ==================== КЛАССЫ ИГРОВЫХ ОБЪЕКТОВ ====================





    // ==================== СИСТЕМЫ И СЕРВИСЫ ====================

    /// <summary>
    /// Класс для обнаружения столкновений между игровыми объектами
    /// </summary>
    public class CollisionDetector : ICollisionDetector
    {
        /// <summary>
        /// Проверяет, находится ли позиция за пределами игрового поля
        /// </summary>
        /// <param name="position">Позиция для проверки</param>
        /// <param name="width">Ширина игрового поля</param>
        /// <param name="height">Высота игрового поля</param>
        /// <returns>true, если позиция выходит за границы поля</returns>
        public bool CheckWallCollision(Position position, int width, int height)
        {
            return position.X < 0 || position.X >= width ||
                   position.Y < 0 || position.Y >= height;
        }

        /// <summary>
        /// Проверяет, пересекается ли позиция с телом змейки
        /// </summary>
        /// <param name="position">Позиция для проверки</param>
        /// <param name="snake">Змейка для проверки столкновения</param>
        /// <returns>true, если позиция пересекается с телом змейки</returns>
        public bool CheckSnakeCollision(Position position, Snake snake)
        {
            foreach(var segment in snake.Body)
            {
                if(position.Equals(segment))
                {
                    return true;
                }
            }
            return false;
        }
    }

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
    /// Класс для отрисовки игры в консоли
    /// </summary>
    public class ConsoleRenderer : IRenderer
    {
        private const char BorderSymbol = '#';
        private const char SnakeBodySymbol = 'O';
        private const char FoodSymbol = '@';

        private readonly GameSettings _settings;

        /// <summary>
        /// Создает новый рендерер для консоли
        /// </summary>
        /// <param name="settings">Настройки игры</param>
        public ConsoleRenderer(GameSettings settings)
        {
            _settings = settings;
        }

        /// <summary>
        /// Отрисовывает текущее состояние игры
        /// </summary>
        /// <param name="snake">Змейка для отрисовки</param>
        /// <param name="food">Еда для отрисовки</param>
        /// <param name="score">Текущий счет</param>
        public void DrawGame(Snake snake, Food food, int score)
        {
            Clear();
            DrawBorder();
            DrawSnake(snake);
            DrawFood(food);
            DrawScore(score);
            DrawControls();
        }

        /// <summary>
        /// Отрисовывает экран завершения игры
        /// </summary>
        /// <param name="score">Финальный счет</param>
        public void DrawGameOver(int score)
        {
            Console.SetCursorPosition(0, _settings.Height + 4);
            Console.WriteLine("Игра окончена! Счет: " + score);
        }

        /// <summary>
        /// Очищает экран игры
        /// </summary>
        public void Clear()
        {
            Console.Clear();
        }

        /// <summary>
        /// Рисует границы игрового поля
        /// </summary>
        private void DrawBorder()
        {
            int borderLength = _settings.Width + 2;
            Console.WriteLine(new string(BorderSymbol, borderLength));

            for(int y = 0; y < _settings.Height; y++)
            {
                Console.Write(BorderSymbol);
                for(int x = 0; x < _settings.Width; x++)
                {
                    Console.Write(' ');
                }
                Console.WriteLine(BorderSymbol);
            }

            Console.WriteLine(new string(BorderSymbol, borderLength));
        }

        /// <summary>
        /// Рисует змейку на игровом поле
        /// </summary>
        /// <param name="snake">Змейка для отрисовки</param>
        private void DrawSnake(Snake snake)
        {
            foreach(var segment in snake.Body)
            {
                Console.SetCursorPosition(segment.X + 1, segment.Y + 1);
                Console.Write(SnakeBodySymbol);
            }
        }

        /// <summary>
        /// Рисует еду на игровом поле
        /// </summary>
        /// <param name="food">Еда для отрисовки</param>
        private void DrawFood(Food food)
        {
            Console.SetCursorPosition(food.Position.X + 1, food.Position.Y + 1);
            Console.Write(FoodSymbol);
        }

        /// <summary>
        /// Отображает текущий счет
        /// </summary>
        /// <param name="score">Текущий счет</param>
        private void DrawScore(int score)
        {
            Console.SetCursorPosition(0, _settings.Height + 2);
            Console.WriteLine($"Счет: {score}");
        }

        /// <summary>
        /// Отображает подсказку по управлению
        /// </summary>
        private void DrawControls()
        {
            Console.SetCursorPosition(0, _settings.Height + 3);
            Console.WriteLine("Управление: WASD/Стрелки, Выход: Esc");
        }
    }

    /// <summary>
    /// Класс для обработки ввода с клавиатуры в консоли
    /// </summary>
    public class ConsoleInputHandler : IInputHandler
    {
        private Direction? _currentDirection;
        private bool _shouldExit;
        private readonly Queue<ConsoleKeyInfo> _keyQueue;

        /// <summary>
        /// Создает новый обработчик ввода для консоли
        /// </summary>
        public ConsoleInputHandler()
        {
            _currentDirection = null;
            _shouldExit = false;
            _keyQueue = new Queue<ConsoleKeyInfo>();
        }

        /// <summary>
        /// Обрабатывает все накопленные события ввода
        /// </summary>
        public void ProcessInput()
        {
            // Сбор всех нажатых клавиш в очередь
            while(Console.KeyAvailable)
            {
                _keyQueue.Enqueue(Console.ReadKey(true));
            }

            // Обработка всех накопленных клавиш
            while(_keyQueue.Count > 0)
            {
                var key = _keyQueue.Dequeue();

                // Проверка клавиши выхода
                if(key.Key == ConsoleKey.Escape)
                {
                    _shouldExit = true;
                }
                else
                {
                    // Преобразование клавиш в направления
                    var direction = key.Key switch
                    {
                        ConsoleKey.W or ConsoleKey.UpArrow => Direction.Up,
                        ConsoleKey.S or ConsoleKey.DownArrow => Direction.Down,
                        ConsoleKey.A or ConsoleKey.LeftArrow => Direction.Left,
                        ConsoleKey.D or ConsoleKey.RightArrow => Direction.Right,
                        _ => (Direction?)null
                    };

                    if(direction.HasValue)
                    {
                        _currentDirection = direction.Value;
                    }
                }
            }
        }

        /// <summary>
        /// Получает текущее направление движения из обработанного ввода
        /// </summary>
        /// <returns>Направление движения или null, если направление не изменилось</returns>
        public Direction? GetDirection()
        {
            var direction = _currentDirection;
            _currentDirection = null; // Сброс после получения
            return direction;
        }

        /// <summary>
        /// Проверяет, запрошен ли выход из игры
        /// </summary>
        /// <returns>true, если игрок нажал клавишу выхода</returns>
        public bool ShouldExit()
        {
            return _shouldExit;
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