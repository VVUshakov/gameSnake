using gameSnake.Models;

namespace gameSnake.Logic.GameLogicComponents
{
    /// <summary>
    /// Стратегия вычисления новой позиции головы змейки.
    /// </summary>
    public interface IMovementStrategy
    {
        /// <summary>
        /// Вычисляет новую позицию головы на основе текущего направления.
        /// </summary>
        /// <param name="currentHead">Текущая позиция головы</param>
        /// <param name="direction">Направление движения</param>
        /// <returns>Новая позиция головы</returns>
        Point CalculateNewHead(Point currentHead, Direction direction);
    }
}
