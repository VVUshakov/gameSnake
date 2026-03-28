namespace Snake
{
    /// <summary>
    /// Простейшее представление координат на игровом поле.
    /// Используется для позиционирования змейки, еды и других объектов.
    /// </summary>
    public class Point
    {
        /// <summary>
        /// Координата X (горизонтальная позиция)
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Координата Y (вертикальная позиция)
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Инициализирует новую точку с указанными координатами
        /// </summary>
        /// <param name="x">Координата X</param>
        /// <param name="y">Координата Y</param>
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}