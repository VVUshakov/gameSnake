namespace SnakeGame
{
    // ==================== ИНТЕРФЕЙСЫ ====================

    /// <summary>
    /// Интерфейс для отрисовки игрового состояния
    /// </summary>
    public interface IRenderer
    {
        /// <summary>
        /// Отрисовывает текущее состояние игры
        /// </summary>
        /// <param name="snake">Змейка для отрисовки</param>
        /// <param name="food">Еда для отрисовки</param>
        /// <param name="score">Текущий счет</param>
        void DrawGame(Snake snake, Food food, int score);

        /// <summary>
        /// Отрисовывает экран завершения игры
        /// </summary>
        /// <param name="score">Финальный счет</param>
        void DrawGameOver(int score);

        /// <summary>
        /// Очищает экран игры
        /// </summary>
        void Clear();
    }

    /// <summary>
    /// Интерфейс для обработки пользовательского ввода
    /// </summary>
    public interface IInputHandler
    {
        /// <summary>
        /// Обрабатывает все накопленные события ввода
        /// </summary>
        void ProcessInput();

        /// <summary>
        /// Получает текущее направление движения из обработанного ввода
        /// </summary>
        /// <returns>Направление движения или null, если направление не изменилось</returns>
        Direction? GetDirection();

        /// <summary>
        /// Проверяет, запрошен ли выход из игры
        /// </summary>
        /// <returns>true, если игрок нажал клавишу выхода</returns>
        bool ShouldExit();
    }

    /// <summary>
    /// Базовый интерфейс для всех игровых объектов
    /// </summary>
    public interface IGameObject
    {
        /// <summary>
        /// Текущая позиция игрового объекта на поле
        /// </summary>
        Position Position { get; }

        /// <summary>
        /// Обновляет состояние игрового объекта
        /// </summary>
        void Update();
    }

    /// <summary>
    /// Интерфейс для обнаружения столкновений между игровыми объектами
    /// </summary>
    public interface ICollisionDetector
    {
        /// <summary>
        /// Проверяет столкновение позиции с границами игрового поля
        /// </summary>
        /// <param name="position">Позиция для проверки</param>
        /// <param name="width">Ширина игрового поля</param>
        /// <param name="height">Высота игрового поля</param>
        /// <returns>true, если позиция выходит за границы поля</returns>
        bool CheckWallCollision(Position position, int width, int height);

        /// <summary>
        /// Проверяет столкновение позиции с телом змейки
        /// </summary>
        /// <param name="position">Позиция для проверки</param>
        /// <param name="snake">Змейка для проверки столкновения</param>
        /// <returns>true, если позиция пересекается с телом змейки</returns>
        bool CheckSnakeCollision(Position position, Snake snake);
    }

    /// <summary>
    /// Интерфейс для создания игровых объектов
    /// </summary>
    public interface IGameObjectFactory
    {
        /// <summary>
        /// Создает новый объект еды
        /// </summary>
        /// <param name="width">Ширина игрового поля</param>
        /// <param name="height">Высота игрового поля</param>
        /// <returns>Новый объект еды</returns>
        Food CreateFood(int width, int height);

        /// <summary>
        /// Создает новый объект змейки
        /// </summary>
        /// <param name="initialLength">Начальная длина змейки</param>
        /// <returns>Новый объект змейки</returns>
        Snake CreateSnake(int initialLength);
    }

    // ==================== МОДЕЛИ ДАННЫХ ====================

    /// <summary>
    /// Перечисление возможных направлений движения
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// Движение вверх
        /// </summary>
        Up,

        /// <summary>
        /// Движение вниз
        /// </summary>
        Down,

        /// <summary>
        /// Движение влево
        /// </summary>
        Left,

        /// <summary>
        /// Движение вправо
        /// </summary>
        Right,

        /// <summary>
        /// Отсутствие движения
        /// </summary>
        None
    }

    /// <summary>
    /// Представляет координаты на игровом поле
    /// </summary>
    public class Position
    {
        /// <summary>
        /// Координата по оси X
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Координата по оси Y
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Создает новый объект позиции
        /// </summary>
        /// <param name="x">Координата по оси X</param>
        /// <param name="y">Координата по оси Y</param>
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Сравнивает текущую позицию с другой позицией
        /// </summary>
        /// <param name="other">Позиция для сравнения</param>
        /// <returns>true, если позиции совпадают</returns>
        public bool Equals(Position other)
        {
            return X == other.X && Y == other.Y;
        }
    }

    /// <summary>
    /// Класс, содержащий настройки игры
    /// </summary>
    public class GameSettings
    {
        /// <summary>
        /// Ширина игрового поля
        /// </summary>
        public int Width { get; } = 20;

        /// <summary>
        /// Высота игрового поля
        /// </summary>
        public int Height { get; } = 15;

        /// <summary>
        /// Начальная длина змейки
        /// </summary>
        public int InitialSnakeLength { get; } = 3;

        /// <summary>
        /// Количество очков за съеденную еду
        /// </summary>
        public int FoodScoreValue { get; } = 10;

        /// <summary>
        /// Скорость игры в миллисекундах
        /// </summary>
        public int GameSpeed { get; } = 100;

        /// <summary>
        /// Начальная позиция змейки
        /// </summary>
        public Position InitialSnakePosition { get; } = new Position(5, 5);

        /// <summary>
        /// Создает настройки игры со значениями по умолчанию
        /// </summary>
        public GameSettings() { }

        /// <summary>
        /// Создает настройки игры с пользовательскими значениями
        /// </summary>
        /// <param name="width">Ширина игрового поля</param>
        /// <param name="height">Высота игрового поля</param>
        /// <param name="initialSnakeLength">Начальная длина змейки</param>
        /// <param name="foodScoreValue">Количество очков за еду</param>
        /// <param name="gameSpeed">Скорость игры</param>
        /// <param name="initialPosition">Начальная позиция змейки</param>
        public GameSettings(int width, int height, int initialSnakeLength, int foodScoreValue,
                           int gameSpeed, Position initialPosition)
        {
            Width = width;
            Height = height;
            InitialSnakeLength = initialSnakeLength;
            FoodScoreValue = foodScoreValue;
            GameSpeed = gameSpeed;
            InitialSnakePosition = initialPosition;
        }
    }

    // ==================== КЛАССЫ ИГРОВЫХ ОБЪЕКТОВ ====================

    /// <summary>
    /// Класс, представляющий змейку - основной игровой объект
    /// </summary>
    public class Snake : IGameObject
    {
        private readonly List<Position> _body;
        private Direction _currentDirection;
        private Direction? _nextDirection;

        /// <summary>
        /// Тело змейки в виде списка позиций
        /// </summary>
        public IReadOnlyList<Position> Body => _body;

        /// <summary>
        /// Текущая позиция головы змейки
        /// </summary>
        public Position Position => _body[0];

        /// <summary>
        /// Текущее направление движения змейки
        /// </summary>
        public Direction CurrentDirection => _currentDirection;

        /// <summary>
        /// Создает новую змейку с указанными параметрами
        /// </summary>
        /// <param name="initialPosition">Начальная позиция головы</param>
        /// <param name="initialLength">Начальная длина змейки</param>
        public Snake(Position initialPosition, int initialLength)
        {
            _body = new List<Position>();
            _currentDirection = Direction.Right;
            _nextDirection = null;

            // Создание начального тела змейки
            for(int i = 0; i < initialLength; i++)
            {
                _body.Add(new Position(initialPosition.X - i, initialPosition.Y));
            }
        }

        /// <summary>
        /// Двигает змейку в текущем направлении
        /// </summary>
        /// <param name="grow">true, если змейка должна вырасти (после съедения еды)</param>
        public void Move(bool grow = false)
        {
            // Применение отложенного изменения направления
            if(_nextDirection.HasValue)
            {
                ChangeDirection(_nextDirection.Value);
                _nextDirection = null;
            }

            Position oldHead = _body[0];
            Position newHead = CalculateNewHeadPosition(oldHead, _currentDirection);

            // Добавление новой головы
            _body.Insert(0, newHead);

            // Удаление хвоста, если змейка не растет
            if(!grow && _body.Count > 1)
            {
                _body.RemoveAt(_body.Count - 1);
            }
        }

        /// <summary>
        /// Устанавливает направление для следующего шага змейки
        /// </summary>
        /// <param name="newDirection">Новое направление движения</param>
        public void SetNextDirection(Direction newDirection)
        {
            // Проверка, что направление не противоположно текущему
            if(!IsOppositeDirection(newDirection, _currentDirection))
            {
                _nextDirection = newDirection;
            }
        }

        /// <summary>
        /// Изменяет текущее направление движения змейки
        /// </summary>
        /// <param name="newDirection">Новое направление движения</param>
        private void ChangeDirection(Direction newDirection)
        {
            _currentDirection = newDirection;
        }

        /// <summary>
        /// Проверяет столкновение головы змейки с ее телом
        /// </summary>
        /// <returns>true, если произошло столкновение с собой</returns>
        public bool CheckSelfCollision()
        {
            Position head = _body[0];

            // Проверка всех сегментов тела, кроме головы
            for(int i = 1; i < _body.Count; i++)
            {
                if(head.Equals(_body[i]))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Вычисляет новую позицию головы на основе текущего направления
        /// </summary>
        /// <param name="currentHead">Текущая позиция головы</param>
        /// <param name="direction">Направление движения</param>
        /// <returns>Новая позиция головы</returns>
        private Position CalculateNewHeadPosition(Position currentHead, Direction direction)
        {
            return direction switch
            {
                Direction.Up => new Position(currentHead.X, currentHead.Y - 1),
                Direction.Down => new Position(currentHead.X, currentHead.Y + 1),
                Direction.Left => new Position(currentHead.X - 1, currentHead.Y),
                Direction.Right => new Position(currentHead.X + 1, currentHead.Y),
                _ => currentHead
            };
        }

        /// <summary>
        /// Проверяет, являются ли два направления противоположными
        /// </summary>
        /// <param name="dir1">Первое направление</param>
        /// <param name="dir2">Второе направление</param>
        /// <returns>true, если направления противоположны</returns>
        private bool IsOppositeDirection(Direction dir1, Direction dir2)
        {
            return (dir1 == Direction.Up && dir2 == Direction.Down) ||
                   (dir1 == Direction.Down && dir2 == Direction.Up) ||
                   (dir1 == Direction.Left && dir2 == Direction.Right) ||
                   (dir1 == Direction.Right && dir2 == Direction.Left);
        }

        /// <summary>
        /// Обновляет состояние змейки (реализация IGameObject)
        /// </summary>
        public void Update()
        {
            Move();
        }
    }

    /// <summary>
    /// Класс, представляющий еду на игровом поле
    /// </summary>
    public class Food : IGameObject
    {
        private static readonly Random _random = new Random();

        /// <summary>
        /// Текущая позиция еды на поле
        /// </summary>
        public Position Position { get; private set; }

        /// <summary>
        /// Создает новую еду в случайной позиции
        /// </summary>
        /// <param name="width">Ширина игрового поля</param>
        /// <param name="height">Высота игрового поля</param>
        public Food(int width, int height)
        {
            Respawn(width, height);
        }

        /// <summary>
        /// Перемещает еду в новую случайную позицию
        /// </summary>
        /// <param name="width">Ширина игрового поля</param>
        /// <param name="height">Высота игрового поля</param>
        public void Respawn(int width, int height)
        {
            Position = new Position(
                _random.Next(0, width),
                _random.Next(0, height)
            );
        }

        /// <summary>
        /// Обновляет состояние еды (реализация IGameObject)
        /// </summary>
        public void Update()
        {
            // Еда не требует обновления, так как статична
        }
    }

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