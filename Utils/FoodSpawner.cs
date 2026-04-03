using Snake.Models;
using SnakeType = Snake.Models.Snake;

namespace Snake.Utils
{
    /// <summary>
    /// Отвечает за размещение еды на игровом поле.
    /// </summary>
    public static class FoodSpawner
    {
        private static readonly Random _random = new Random();

        /// <summary>
        /// Создаёт еду в случайном свободном месте на поле.
        /// </summary>
        public static Food CreateFood(PlayingField field, SnakeType snake)
        {
            Point? position = FindFreePosition(field, snake);
            return new Food(position, position != null);
        }

        /// <summary>
        /// Ищет случайную свободную позицию между рамками поля.
        /// </summary>
        private static Point? FindFreePosition(PlayingField field, SnakeType snake)
        {
            int maxAttempts = 1000;

            for(int attempt = 0; attempt < maxAttempts; attempt++)
            {
                int x = _random.Next(field.Left + 1, field.Right);
                int y = _random.Next(field.Top + 1, field.Bottom);
                Point candidate = new Point(x, y);

                if(!snake.Contains(candidate))
                    return candidate;
            }

            return null;
        }
    }
}
