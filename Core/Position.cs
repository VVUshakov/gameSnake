namespace SnakeGame.Core
{
    /// <summary>
    /// Представляет координаты на игровом поле
    /// </summary>
    public class Position
    {
        /// <summary>
        /// Координата по оси X
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Координата по оси Y
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Создает новый объект позиции
        /// </summary>
        /// <param name="x">Координата по оси X</param>
        /// <param name="y">Координата по оси Y</param>
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Сравнивает текущую позицию с другой позицией
        /// </summary>
        /// <param name="other">Позиция для сравнения</param>
        /// <returns>true, если позиции совпадают</returns>
        public bool Equals(Position other)
        {
            return X == other.X && Y == other.Y;
        }
    }
}