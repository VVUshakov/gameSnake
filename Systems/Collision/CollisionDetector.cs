using SnakeGame.Core;
using SnakeGame.GameObjects;
using SnakeGame.Interfaces;

namespace SnakeGame.Systems.Collision
{
    /// <summary>
    /// Класс для обнаружения столкновений между игровыми объектами
    /// </summary>
    public class CollisionDetector : ICollisionDetector
    {
        /// <summary>
        /// Проверяет, находится ли позиция за пределами игрового поля
        /// </summary>
        /// <param name="position">Позиция для проверки</param>
        /// <param name="width">Ширина игрового поля</param>
        /// <param name="height">Высота игрового поля</param>
        /// <returns>true, если позиция выходит за границы поля</returns>
        public bool CheckWallCollision(Position position, int width, int height)
        {
            return position.X < 0 || position.X >= width ||
                   position.Y < 0 || position.Y >= height;
        }

        /// <summary>
        /// Проверяет, пересекается ли позиция с телом змейки
        /// </summary>
        /// <param name="position">Позиция для проверки</param>
        /// <param name="snake">Змейка для проверки столкновения</param>
        /// <returns>true, если позиция пересекается с телом змейки</returns>
        public bool CheckSnakeCollision(Position position, Snake snake)
        {
            foreach(var segment in snake.Body)
            {
                if(position.Equals(segment))
                {
                    return true;
                }
            }
            return false;
        }
    }
}