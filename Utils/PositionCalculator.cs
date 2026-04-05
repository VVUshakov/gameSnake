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
        /// Рассчитывает позицию головы так, чтобы вся змейка была отцентрирована на игровом поле.
        /// Если змейка не помещается, длина автоматически уменьшается до допустимой.
        /// </summary>
        /// <param name="fieldWidth">Ширина игрового поля</param>
        /// <param name="fieldHeight">Высота игрового поля</param>
        /// <param name="snakeLength">Желаемая длина змейки</param>
        /// <param name="direction">Направление движения змейки</param>
        /// <returns>Кортеж: позиция головы и скорректированная длина змейки</returns>
        public static (Point headPosition, int finalSnakeLength) CalculateCenteredHeadPosition(
            int fieldWidth,
            int fieldHeight,
            int snakeLength,
            Direction direction)
        {
            // Границы рамки
            int frameLeft = 0;
            int frameRight = fieldWidth - 1;
            int frameTop = 0;
            int frameBottom = fieldHeight - 1;

            // Границы доступной области (между рамками)
            int playableLeft = frameLeft + 1;
            int playableRight = frameRight - 1;
            int playableTop = frameTop + 1;
            int playableBottom = frameBottom - 1;

            // Размеры доступной области (между рамками)
            int availableWidth = playableRight - playableLeft;
            int availableHeight = playableBottom - playableTop;

            // Ограничиваем длину змейки доступным пространством
            int finalSnakeLength = direction == Direction.Left || direction == Direction.Right
                ? Min(snakeLength, availableWidth)
                : Min(snakeLength, availableHeight);

            // Центр игрового поля
            int centerX = fieldWidth / 2;
            int centerY = fieldHeight / 2;

            // Рассчитывает позицию головы, чтобы змейка была отцентрирована на игровом поле
            Point headPosition;
            switch (direction)
            {
                case Direction.Right:
                    int maxX = Max(finalSnakeLength, centerX);
                    int rightX = Min(maxX, playableRight);
                    headPosition = new Point(rightX, centerY);
                    break;

                case Direction.Left:
                    int minX = Min(playableRight - finalSnakeLength, centerX);
                    int leftX = Max(minX, playableLeft);
                    headPosition = new Point(leftX, centerY);
                    break;

                case Direction.Down:
                    int maxY = Max(finalSnakeLength, centerY);
                    int downY = Min(maxY, playableBottom);
                    headPosition = new Point(centerX, downY);
                    break;

                case Direction.Up:
                    int minY = Min(playableBottom - finalSnakeLength, centerY);
                    int upY = Max(minY, playableTop);
                    headPosition = new Point(centerX, upY);
                    break;

                default:
                    headPosition = new Point(centerX, centerY);
                    break;
            }

            return (headPosition, finalSnakeLength);
        }

        /// <summary>
        /// Проверяет, помещается ли змейка в игровое поле при заданном направлении.
        /// </summary>
        /// <param name="fieldWidth">Ширина игрового поля</param>
        /// <param name="fieldHeight">Высота игрового поля</param>
        /// <param name="snakeLength">Длина змейки</param>
        /// <param name="direction">Направление движения змейки</param>
        /// <returns>True, если змейка помещается</returns>
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

        /// <summary>
        /// Возвращает меньшее из двух чисел.
        /// </summary>
        private static int Min(int a, int b) => a < b ? a : b;

        /// <summary>
        /// Возвращает большее из двух чисел.
        /// </summary>
        private static int Max(int a, int b) => a > b ? a : b;
    }
}
