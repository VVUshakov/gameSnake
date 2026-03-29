namespace Snake
{
    /// <summary>
    /// Предоставляет методы для расчёта позиций объектов на игровом поле.
    /// Содержит математические функции для центрирования змейки и сообщений.
    /// </summary>
    public static class PositionCalculator
    {
        /// <summary>
        /// Рассчитывает позицию головы так, чтобы вся змейка была отцентрирована на поле.
        /// </summary>
        /// <param name="fieldWidth">Ширина игрового поля в клетках</param>
        /// <param name="fieldHeight">Высота игрового поля в клетках</param>
        /// <param name="snakeLength">Длина змейки в клетках</param>
        /// <param name="direction">Направление движения змейки</param>
        /// <returns>Позиция головы змейки для центрирования</returns>
        /// <exception cref="ArgumentException">
        /// Выбрасывается, если змейка не помещается в поле при заданном направлении
        /// </exception>
        public static Point CalculateCenteredHeadPosition(
            int fieldWidth,         // ширина игрового поля
            int fieldHeight,        // высота игрового поля
            int snakeLength,        // длина змейки
            Direction direction     // направление движения
        )
        {
            // Проверка: помещается ли змейка в поле по ширине
            if((direction == Direction.Right || direction == Direction.Left) && snakeLength > fieldWidth)
                throw new ArgumentException($"Змейка длиной {snakeLength} не помещается в поле шириной {fieldWidth}");

            // Проверка: помещается ли змейка в поле по высоте
            if((direction == Direction.Up || direction == Direction.Down) && snakeLength > fieldHeight)
                throw new ArgumentException($"Змейка длиной {snakeLength} не помещается в поле высотой {fieldHeight}");

            // Центр поля
            int centerX = fieldWidth / 2;
            int centerY = fieldHeight / 2;

            Point headPosition; // координаты головы

            // Рассчитываем позицию головы в зависимости от направления
            // Важно: голова должна быть так, чтобы всё тело поместилось в поле
            switch(direction)
            {
                case Direction.Right:
                    // При движении вправо: голова в центре, хвост слева
                    // Голова не должна быть ближе к левому краю, чем длина змейки - 1
                    int headXRight = Math.Max(snakeLength - 1, centerX);
                    // Но и не должна выходить за правую границу
                    headXRight = Math.Min(headXRight, fieldWidth - 1);
                    headPosition = new Point(x: headXRight, y: centerY);
                    break;

                case Direction.Left:
                    // При движении влево: голова в центре, хвост справа
                    int headXLeft = Math.Min(fieldWidth - snakeLength, centerX);
                    headXLeft = Math.Max(headXLeft, 0);
                    headPosition = new Point(x: headXLeft, y: centerY);
                    break;

                case Direction.Down:
                    // При движении вниз: голова в центре, хвост сверху
                    int headYDown = Math.Max(snakeLength - 1, centerY);
                    headYDown = Math.Min(headYDown, fieldHeight - 1);
                    headPosition = new Point(x: centerX, y: headYDown);
                    break;

                case Direction.Up:
                    // При движении вверх: голова в центре, хвост снизу
                    int headYUp = Math.Min(fieldHeight - snakeLength, centerY);
                    headYUp = Math.Max(headYUp, 0);
                    headPosition = new Point(x: centerX, y: headYUp);
                    break;

                default:
                    throw new ArgumentException($"Неизвестное направление: {direction}");
            }

            return headPosition;
        }

        /// <summary>
        /// Проверяет, помещается ли змейка в поле при заданном направлении.
        /// </summary>
        /// <param name="fieldWidth">Ширина игрового поля в клетках</param>
        /// <param name="fieldHeight">Высота игрового поля в клетках</param>
        /// <param name="snakeLength">Длина змейки в клетках</param>
        /// <param name="direction">Направление движения змейки</param>
        /// <returns>true, если змейка помещается; false в противном случае</returns>
        public static bool CanPlaceSnake(
            int fieldWidth,         // ширина игрового поля
            int fieldHeight,        // высота игрового поля
            int snakeLength,        // длина змейки
            Direction direction     // направление движения
        )
        {
            try
            {
                CalculateCenteredHeadPosition(
                    fieldWidth,   // ширина игрового поля
                    fieldHeight,  // высота игрового поля
                    snakeLength,  // длина змейки
                    direction     // направление движения
                );
                return true;
            }
            catch(ArgumentException)
            {
                return false;
            }
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
        /// Вычисляет максимальную ширину сообщения (длину самой длинной строки).
        /// </summary>
        /// <param name="lines">Строки сообщения</param>
        /// <returns>Максимальная длина строки</returns>
        public static int GetMessageWidth(string[] lines)
        {
            int maxWidth = 0;
            foreach(string line in lines)
            {
                if(line.Length > maxWidth)
                {
                    maxWidth = line.Length;
                }
            }
            return maxWidth;
        }

        /// <summary>
        /// Вычисляет высоту сообщения (количество строк).
        /// </summary>
        /// <param name="lines">Строки сообщения</param>
        /// <returns>Количество строк</returns>
        public static int GetMessageHeight(string[] lines)
        {
            return lines.Length;
        }
    }
}