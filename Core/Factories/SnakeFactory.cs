using gameSnake.Models;

namespace gameSnake.Core.Factories
{
    /// <summary>
    /// Отвечает за построение сегментов тела змейки на основе позиции головы, направления и длины.
    /// </summary>
    public static class SnakeFactory
    {
        /// <summary>
        /// Создаёт змейку с телом, выстроенным от головы в заданном направлении.
        /// </summary>
        /// <param name="headPosition">Позиция головы</param>
        /// <param name="direction">Направление движения</param>
        /// <param name="snakeLength">Длина змейки (минимум 1)</param>
        /// <returns>Готовая змейка</returns>
        public static Snake Create(Point headPosition, Direction direction, int snakeLength = 3)
        {
            if (snakeLength < 1) snakeLength = 1;

            var segments = new List<Point>(snakeLength);
            for (int i = snakeLength - 1; i >= 0; i--)
            {
                segments.Add(direction switch
                {
                    Direction.Right => new Point(headPosition.X - i, headPosition.Y),
                    Direction.Left  => new Point(headPosition.X + i, headPosition.Y),
                    Direction.Up    => new Point(headPosition.X, headPosition.Y + i),
                    Direction.Down  => new Point(headPosition.X, headPosition.Y - i),
                    _ => throw new ArgumentException($"Invalid direction: {direction}")
                });
            }

            return new Snake(segments);
        }
    }
}
