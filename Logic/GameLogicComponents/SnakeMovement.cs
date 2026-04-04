using gameSnake.Models;

namespace gameSnake.Logic.GameLogicComponents
{
    /// <summary>
    /// Стратегия движения змейки: смещение на 1 клетку по направлению.
    /// </summary>
    public static class SnakeMovement
    {
        /// <summary>
        /// Вычисляет новую позицию головы на основе текущего направления.
        /// </summary>
        /// <param name="currentHead">Текущая позиция головы</param>
        /// <param name="direction">Направление движения</param>
        /// <returns>Новая позиция головы</returns>
        public static Point CalculateNewHead(Point currentHead, Direction direction) => direction switch
        {
            Direction.Up    => new Point(currentHead.X, currentHead.Y - 1),
            Direction.Down  => new Point(currentHead.X, currentHead.Y + 1),
            Direction.Left  => new Point(currentHead.X - 1, currentHead.Y),
            Direction.Right => new Point(currentHead.X + 1, currentHead.Y),
            _ => currentHead
        };
    }
}
