namespace Snake.Models
{
    /// <summary>
    /// Представляет игровое поле.
    /// Определяет размеры поля и границы доступной области.
    /// </summary>
    public class PlayingField
    {
        /// <summary>
        /// Ширина игрового поля в клетках
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Высота игрового поля в клетках
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Левая граница поля (координата X = 0)
        /// </summary>
        public int Left => 0;

        /// <summary>
        /// Правая граница поля (координата X = Width - 1)
        /// </summary>
        public int Right => Width - 1;

        /// <summary>
        /// Верхняя граница поля (координата Y = 0)
        /// </summary>
        public int Top => 0;

        /// <summary>
        /// Нижняя граница поля (координата Y = Height - 1)
        /// </summary>
        public int Bottom => Height - 1;

        /// <summary>
        /// Проверяет, находится ли точка в рамке поля
        /// </summary>
        /// <param name="point">Точка для проверки</param>
        /// <returns>true, если точка является рамкой; false в противном случае</returns>
        public bool IsBorder(Point point)
        {
            return point.X == Left || point.X == Right ||
                   point.Y == Top || point.Y == Bottom;
        }

        /// <summary>
        /// Проверяет, находится ли точка в доступной области (между рамками)
        /// </summary>
        /// <param name="point">Точка для проверки</param>
        /// <returns>true, если точка находится между рамками; false в противном случае</returns>
        public bool IsInside(Point point)
        {
            return point.X > Left && point.X < Right &&
                   point.Y > Top && point.Y < Bottom;
        }

        /// <summary>
        /// Проверяет, находится ли точка в пределах поля (включая рамку)
        /// </summary>
        /// <param name="point">Точка для проверки</param>
        /// <returns>true, если точка в пределах поля; false в противном случае</returns>
        public bool IsWithinBounds(Point point)
        {
            return point.X >= Left && point.X <= Right &&
                   point.Y >= Top && point.Y <= Bottom;
        }

        /// <summary>
        /// Инициализирует новое игровое поле с указанными размерами
        /// </summary>
        /// <param name="width">Ширина поля в клетках (по умолчанию 20)</param>
        /// <param name="height">Высота поля в клетках (по умолчанию 10)</param>
        public PlayingField(int width = 20, int height = 10)
        {
            Width = width;
            Height = height;
        }
    }
}