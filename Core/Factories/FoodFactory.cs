using gameSnake.Models;

namespace gameSnake.Core.Factories
{
    /// <summary>
    /// Отвечает за размещение еды на игровом поле.
    /// </summary>
    public static class FoodFactory
    {
        /// <summary>
        /// Создаёт еду в случайном свободном месте на поле.
        /// </summary>
        /// <param name="field">Игровое поле</param>
        /// <param name="snake">Змейка (для исключения её позиций)</param>
        /// <param name="random">Генератор случайных чисел (по умолчанию — новый Random)</param>
        /// <returns>Новый объект еды</returns>
        public static Food CreateFood(PlayingField field, Snake snake, Random? random = null)
        {
            random ??= new Random();
            Point? position = FindFreePosition(field, snake, random);
            return new Food(position, position != null);
        }

        /// <summary>
        /// Ищет случайную свободную позицию между рамками поля.
        /// </summary>
        private static Point? FindFreePosition(PlayingField field, Snake snake, Random random)
        {
            int maxAttempts = 1000;

            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                int x = random.Next(field.Left + 1, field.Right);
                int y = random.Next(field.Top + 1, field.Bottom);
                Point candidate = new Point(x, y);

                if (!snake.Contains(candidate))
                    return candidate;
            }

            return null;
        }
    }
}
