using gameSnake.Models;

namespace gameSnake.Utils
{
    /// <summary>
    /// Предоставляет методы для расчёта позиций объектов на игровом поле.
    /// Содержит математические функции для центрирования змейки и сообщений.
    /// </summary>
    public static class PositionCalculator
    {
        /// <summary>
        /// Рассчитывает позицию головы так, чтобы вся змейка была отцентрирована на поле.
        /// Если змейка не помещается, длина автоматически уменьшается до допустимой.
        /// </summary>
        /// <returns>Кортеж: позиция головы и скорректированная длина змейки</returns>
        public static (Point headPosition, int finalSnakeLength) CalculateCenteredHeadPosition(
            int fieldWidth,
            int fieldHeight,
            int snakeLength,
            Direction direction)
        {
            int availableWidth = fieldWidth - 2;
            int availableHeight = fieldHeight - 2;

            // Ограничиваем длину змейки доступным пространством
            int finalSnakeLength = direction == Direction.Left || direction == Direction.Right
                ? Math.Min(snakeLength, availableWidth)
                : Math.Min(snakeLength, availableHeight);

            int centerX = fieldWidth / 2;
            int centerY = fieldHeight / 2;

            Point headPosition = direction switch
            {
                Direction.Right => new Point(Math.Min(Math.Max(finalSnakeLength, centerX), fieldWidth - 2), centerY),
                Direction.Left  => new Point(Math.Max(Math.Min(fieldWidth - 1 - finalSnakeLength, centerX), 1), centerY),
                Direction.Down  => new Point(centerX, Math.Min(Math.Max(finalSnakeLength, centerY), fieldHeight - 2)),
                Direction.Up    => new Point(centerX, Math.Max(Math.Min(fieldHeight - 1 - finalSnakeLength, centerY), 1)),
                _               => new Point(centerX, centerY)
            };

            return (headPosition, finalSnakeLength);
        }

        /// <summary>
        /// Проверяет, помещается ли змейка в поле при заданном направлении.
        /// </summary>
        public static bool CanPlaceSnake(int fieldWidth, int fieldHeight, int snakeLength, Direction direction)
        {
            int available = direction == Direction.Left || direction == Direction.Right
                ? fieldWidth - 2
                : fieldHeight - 2;
            return snakeLength <= available;
        }

        /// <summary>
        /// Рассчитывает позицию для центрирования сообщения на игровом поле.
        /// </summary>
        /// <param name="fieldWidth">Ширина игрового поля</param>
        /// <param name="fieldHeight">Высота игрового поля (можно передать с учётом заголовка: fieldHeight + headerHeight)</param>
        /// <param name="messageWidth">Ширина сообщения (максимальная длина строки)</param>
        /// <param name="messageHeight">Высота сообщения (количество строк)</param>
        /// <returns>Координаты верхнего левого угла для отрисовки сообщения</returns>
        public static Point CalculateCenteredMessagePosition(
            int fieldWidth,
            int fieldHeight,
            int messageWidth,
            int messageHeight)
        {
            int startX = (fieldWidth - messageWidth) / 2;
            int startY = (fieldHeight - messageHeight) / 2;

            return new Point(startX, startY);
        }
    }
}