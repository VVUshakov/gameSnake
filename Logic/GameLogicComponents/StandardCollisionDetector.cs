using gameSnake.Models;

namespace gameSnake.Logic.GameLogicComponents
{
    /// <summary>
    /// Стандартный детектор столкновений: проверка стен и тела змейки.
    /// </summary>
    public static class StandardCollisionDetector
    {
        /// <summary>
        /// Проверяет наличие столкновения со стеной или собственным телом.
        /// </summary>
        /// <param name="snake">Змейка</param>
        /// <param name="field">Игровое поле</param>
        /// <returns>True, если есть столкновение</returns>
        public static bool HasCollision(Snake snake, PlayingField field)
        {
            Point head = snake.Head;

            // Столкновение со стеной
            if (head.X <= 0 || head.X >= field.Width - 1 ||
                head.Y <= 0 || head.Y >= field.Height - 1)
                return true;

            // Столкновение с собственным телом
            for (int i = 0; i < snake.Body.Count - 1; i++)
                if (snake.Body[i].Equals(head)) return true;

            return false;
        }
    }
}
