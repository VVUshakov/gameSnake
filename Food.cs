namespace Snake
{
    /// <summary>
    /// Представляет еду на игровом поле.
    /// При поедании змейкой увеличивает счёт и растёт змейка.
    /// </summary>
    public class Food
    {
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
    }
}