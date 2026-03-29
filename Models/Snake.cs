namespace Snake.Models
{
    /// <summary>
    /// Представляет змейку на игровом поле.
    /// Содержит список сегментов тела и методы для работы со змейкой.
    /// </summary>
    public class Snake
    {
        /// <summary>
        /// Список всех сегментов змейки.
        /// Body[0] — хвост, Body[Body.Count - 1] — голова
        /// </summary>
        public List<Point> Body { get; set; }

        /// <summary>
        /// Координаты головы змейки (всегда последний элемент списка)
        /// </summary>
        public Point Head => Body[Body.Count - 1];

        /// <summary>
        /// Координаты хвоста змейки (всегда первый элемент списка)
        /// </summary>
        public Point Tail => Body[0];

        /// <summary>
        /// Инициализирует новую змейку с указанной позицией головы и направлением.
        /// </summary>
        /// <param name="headPosition">Позиция головы змейки</param>
        /// <param name="direction">Начальное направление движения</param>
        /// <param name="snakeLength">Длина змейки (по умолчанию 3)</param>
        /// <exception cref="ArgumentException">Выбрасывается при неизвестном направлении</exception>
        public Snake(
            Point headPosition,     // позиция головы
            Direction direction,    // начальное направление движения
            int snakeLength = 3     // длина змейки
        )
        {
            // Защита: длина змейки должна быть хотя бы 1
            if(snakeLength < 1) snakeLength = 1;

            // Создаём список для тела змейки
            Body = new List<Point>();

            // Строим змейку от хвоста к голове
            // i = snakeLength-1 (хвост) ... 0 (голова)
            for(int i = snakeLength - 1; i >= 0; i--)
            {
                switch(direction)
                {
                    case Direction.Right:
                        // При движении вправо: хвост слева, голова справа
                        Body.Add(new Point(x: headPosition.X - i, y: headPosition.Y));
                        break;

                    case Direction.Left:
                        // При движении влево: хвост справа, голова слева
                        Body.Add(new Point(x: headPosition.X + i, y: headPosition.Y));
                        break;

                    case Direction.Up:
                        // При движении вверх: хвост снизу, голова сверху
                        Body.Add(new Point(x: headPosition.X, y: headPosition.Y + i));
                        break;

                    case Direction.Down:
                        // При движении вниз: хвост сверху, голова снизу
                        Body.Add(new Point(x: headPosition.X, y: headPosition.Y - i));
                        break;

                    default:
                        throw new ArgumentException($"Неизвестное направление: {direction}");
                }
            }
        }

        /// <summary>
        /// Проверяет, содержит ли змейка указанную точку (сегмент тела).
        /// </summary>
        /// <param name="point">Точка для проверки</param>
        /// <returns>true, если точка является частью змейки; false в противном случае</returns>
        public bool Contains(Point point)
        {
            foreach(Point segment in Body)
            {
                if(segment.X == point.X && segment.Y == point.Y)
                    return true;
            }
            return false;
        }
    }
}