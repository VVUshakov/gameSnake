using gameSnake.Models;

namespace gameSnake.Logic.GameLogicComponents
{
    /// <summary>
    /// Стандартная стратегия движения: смещение на 1 клетку по направлению.
    /// </summary>
    public class StandardMovement : IMovementStrategy
    {
        /// <summary>
        /// Вычисляет новую позицию головы на основе текущего направления.
        /// </summary>
        public Point CalculateNewHead(Point currentHead, Direction direction) => direction switch
        {
            Direction.Up    => new Point(currentHead.X, currentHead.Y - 1),
            Direction.Down  => new Point(currentHead.X, currentHead.Y + 1),
            Direction.Left  => new Point(currentHead.X - 1, currentHead.Y),
            Direction.Right => new Point(currentHead.X + 1, currentHead.Y),
            _ => currentHead
        };
    }
}
