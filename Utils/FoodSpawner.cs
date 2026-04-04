using gameSnake.Models;

namespace gameSnake.Utils
{
    /// <summary>
    /// Отвечает за размещение еды на игровом поле.
    /// </summary>
    public static class FoodSpawner
    {
        /// <summary>
        /// Создаёт еду в случайном свободном месте на поле.
        /// </summary>
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
            // Ограничиваем количество попыток
            int maxAttempts = 1000;

            for(int attempt = 0; attempt < maxAttempts; attempt++)
            {
                // Создаем координаты для новой еды
                int x = random.Next(field.Left + 1, field.Right);
                int y = random.Next(field.Top + 1, field.Bottom);
                Point candidate = new Point(x, y);

                // Если координаты не пересекаются со змейкой, возвращаем результат,
                // иначе повторяем пока есть попытки
                if(!snake.Contains(candidate))
                    return candidate;
            }

            return null;
        }
    }
}
