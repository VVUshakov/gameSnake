namespace Snake
{
    /// <summary>
    /// Этот класс занимается только математическими расчётами.
    /// Он проверяет, помещается ли змейка в поле, и вычисляет,
    /// где должна быть голова, чтобы вся змейка была ровно по центру.
    /// Мы сделали его статическим, потому что ему не нужно хранить данные —
    /// только вычислять
    /// </summary>
    public static class PositionCalculator
    {
        /// <summary>
        /// Рассчитывает позицию головы так, чтобы вся змейка была отцентрирована на поле
        /// </summary>
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

            // Половина длины змейки (для центрирования)
            int halfLength = snakeLength / 2;

            Point headPosition; // координаты головы

            // Рассчитываем позицию головы в зависимости от направления
            switch(direction)
            {
                case Direction.Right:
                    // При движении вправо голова должна быть правее центра на половину длины
                    headPosition = new Point(
                        x: centerX + halfLength,
                        y: centerY
                    );
                    break;

                case Direction.Left:
                    // При движении влево голова должна быть левее центра на половину длины
                    headPosition = new Point(
                        x: centerX - halfLength,
                        y: centerY
                    );
                    break;

                case Direction.Down:
                    // При движении вниз голова должна быть ниже центра на половину длины
                    headPosition = new Point(
                        x: centerX,
                        y: centerY + halfLength
                    );
                    break;

                case Direction.Up:
                    // При движении вверх голова должна быть выше центра на половину длины
                    headPosition = new Point(
                        x: centerX,
                        y: centerY - halfLength
                    );
                    break;

                default:
                    throw new ArgumentException($"Неизвестное направление: {direction}");
            }

            return headPosition;
        }

        /// <summary>
        /// Проверяет, помещается ли змейка в поле при заданном направлении
        /// </summary>
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
    }
}