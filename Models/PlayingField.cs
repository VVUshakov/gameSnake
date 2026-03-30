using gameSnake;
using Snake.Utils;

namespace Snake.Models
{
    /// <summary>
    /// Представляет игровое поле.
    /// Определяет размеры поля и границы доступной области.
    /// </summary>
    public class PlayingField
    {
        /// <summary>
        /// Минимальная ширина поля (с учётом рамки и места для сообщений).
        /// Сообщение о паузе имеет ширину 26 символов, поэтому минимум 28 (с рамкой и запасом)
        /// </summary>
        public const int MinWidth = 28;

        /// <summary>
        /// Минимальная высота поля (с учётом рамки и места для сообщений).
        /// Сообщение о паузе имеет высоту 6 строк, поэтому минимум 10 (с рамкой и запасом)
        /// </summary>
        public const int MinHeight = 12;

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
        /// <param name="width">Ширина поля в клетках (по умолчанию 30)</param>
        /// <param name="height">Высота поля в клетках (по умолчанию 30)</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Выбрасывается, если размер поля меньше минимального
        /// </exception>
        public PlayingField(int width = 30, int height = 15)
        {
            if (width < MinWidth)
                throw new ArgumentOutOfRangeException(nameof(width), $"Минимальная ширина поля: {MinWidth}");

            if (height < MinHeight)
                throw new ArgumentOutOfRangeException(nameof(height), $"Минимальная высота поля: {MinHeight}");

            Width = width;
            Height = height;
        }
    }    
}