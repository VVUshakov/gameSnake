using gameSnake.Models;

namespace gameSnake.Logic.GameLogicComponents
{
    /// <summary>
    /// Детектор столкновений змейки со стенами и собственным телом.
    /// </summary>
    public interface ICollisionDetector
    {
        /// <summary>
        /// Проверяет наличие столкновения.
        /// </summary>
        /// <param name="snake">Змейка</param>
        /// <param name="field">Игровое поле</param>
        /// <returns>True, если есть столкновение</returns>
        bool HasCollision(Snake snake, PlayingField field);
    }
}
