using gameSnake.GameObjects.Snake;
using SnakeGame.Core;

namespace SnakeGame.Interfaces
{
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
}