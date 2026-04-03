using gameSnake.Servises;
using gameSnake.Utils;

namespace gameSnake.Models
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
        /// <param name="width">Ширина поля в клетках (по умолчанию 30)</param>
        /// <param name="height">Высота поля в клетках (по умолчанию 30)</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Выбрасывается, если размер поля меньше минимального
        /// </exception>
        public PlayingField(int width = 30, int height = 15)
        {
            // Получаем массив всех сервисных сообщений игры
            List<string[]> allMessages = ServiseMessange.GetAllMessages();

            // Получаем минимальные ширину и высоту игрового поля (с учётом рамки и места для сервисных сообщений).
            // Вычисляется динамически на основе максимального габарита сервисного сообщения + рамка(2) + запас(2)
            int minWidth = MessageSizer.GetMaxWidth(allMessages) + 4;
            int minHeight = MessageSizer.GetMaxHeight(allMessages) + 4;

            // Проверяем входящие параметры на предмет соответсвия минимальным размерам
            if (width < minWidth) { width = minWidth; }
            if (height < minHeight) { height = minHeight; }

            Width = width;
            Height = height;
        }
    }    
}