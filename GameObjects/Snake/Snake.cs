using SnakeGame.Core;
using SnakeGame.Interfaces;

namespace SnakeGame.GameObjects
{
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
}