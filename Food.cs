namespace Snake
{
    /// <summary>
    /// Представляет еду на игровом поле
    /// </summary>
    public class Food
    {
        // Позиция еды на поле (может быть null, если нет свободного места)
        public Point? Position { get; set; }

        // Сколько очков даёт эта еда (ценность)
        public int PointsValue { get; set; } = 10;

        // Флаг успешности создания еды
        public bool IsSuccess { get; set; }

        public Food(Point? position, bool isSuccess = true)
        {
            Position = position;
            IsSuccess = isSuccess;
        }
    }
}