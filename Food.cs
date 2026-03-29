namespace Snake
{
    /// <summary>
    /// Представляет еду на игровом поле.
    /// При поедании змейкой увеличивает счёт и растёт змейка.
    /// </summary>
    public class Food
    {
        private static readonly Random _random = new Random();

        /// <summary>
        /// Позиция еды на игровом поле.
        /// Может быть null, если нет свободного места для размещения.
        /// </summary>
        public Point? Position { get; set; }

        /// <summary>
        /// Количество очков, которое даёт эта еда при поедании
        /// </summary>
        public int PointsValue { get; set; } = 10;

        /// <summary>
        /// Флаг успешности создания еды (true — еда размещена, false — нет свободного места)
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Инициализирует новый экземпляр еды с указанной позицией
        /// </summary>
        /// <param name="position">Позиция еды на игровом поле</param>
        /// <param name="isSuccess">Флаг успешности создания (по умолчанию true)</param>
        public Food(Point? position, bool isSuccess = true)
        {
            Position = position;
            IsSuccess = isSuccess;
        }

        /// <summary>
        /// Создаёт начальную еду в случайном свободном месте на поле
        /// </summary>
        /// <param name="field">Игровое поле</param>
        /// <param name="snake">Змейка для проверки занятых клеток</param>
        /// <returns>Объект еды с позицией и флагом успешности</returns>
        public static Food CreateInitialFood(PlayingField field, Snake snake)
        {
            Point? position = GenerateRandomFoodPosition(field, snake);

            return new Food(
                position: position,
                isSuccess: position != null
            );
        }

        /// <summary>
        /// Генерирует случайное положение еды, не занятое змейкой.
        /// Еда появляется только в пределах игрового поля (между рамками).
        /// </summary>
        /// <param name="field">Игровое поле</param>
        /// <param name="snake">Змейка для проверки занятых клеток</param>
        /// <returns>Позиция еды или null, если нет свободного места</returns>
        private static Point? GenerateRandomFoodPosition(PlayingField field, Snake snake)
        {
            int maxAttempts = 1000; // ограничиваем максимальное количество попыток

            // Еда должна появляться между рамками: от 1 до Width-2 и от 1 до Height-2
            int minX = 1;
            int maxX = field.Width - 2;
            int minY = 1;
            int maxY = field.Height - 2;

            // Проверяем, есть ли место для еды
            if(maxX < minX || maxY < minY)
                return null;

            for(int attempt = 0; attempt < maxAttempts; attempt++)
            {
                int x = _random.Next(minX, maxX + 1);   // случайная координата X (между рамками)
                int y = _random.Next(minY, maxY + 1);   // случайная координата Y (между рамками)
                Point candidateFood = new Point(x, y);  // создаём координату

                // Проверяем, не занята ли эта клетка змейкой
                if(!snake.Contains(candidateFood))
                {
                    return candidateFood; // нашли свободное место!
                }
            }

            // Если не нашли свободное место после всех попыток
            return null;
        }
    }
}