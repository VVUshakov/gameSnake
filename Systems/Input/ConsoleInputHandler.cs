using SnakeGame.Core;
using SnakeGame.Interfaces;

namespace SnakeGame.Systems.Input
{
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
}