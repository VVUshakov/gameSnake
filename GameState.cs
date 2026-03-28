namespace Snake
{
    /// <summary>
    /// Состояние игры. Содержит все данные, необходимые для работы игры.
    /// </summary>
    public class GameState
    {
        // Управляющие флаги
        public bool IsExit { get; set; } = false;       // флаг выхода из игры
        public bool IsGameOver { get; set; } = false;   // флаг проигрыша
        public bool IsWin { get; set; } = false;        // флаг победы
        public bool IsPaused { get; set; } = false;     // флаг паузы

        // Настройки
        public int Fps { get; set; } = 100;     // задержка между кадрами (мс)

        // Игровые данные
        public Header Header { get; } = new Header();   // служебная информация (счёт, уровень и т.п.)
        public Direction CurrentDirection { get; set; } = Direction.Right; // текущее направление

        // Компоненты игры
        public PlayingField Field { get; }  // объект игрового поля
        public Snake Snake { get; }         // объект змейки
        public Food Food { get; }           // объект еды

        public GameState()
        {
            // Создаём поле
            Field = new PlayingField();

            // Рассчитываем необходимую координату головы,
            // чтобы тело змейки была центровано на игровом поле
            Point headPosition = PositionCalculator.CalculateCenteredHeadPosition(
                fieldWidth: Field.Width,    // ширина игрового поля
                fieldHeight: Field.Height,  // высота игрового поля
                snakeLength: 3,             // начальная длина змейки
                direction: Direction.Right  // направление змейки
            );

            // Создаём змейку с центрированным телом на игровом поле
            Snake = new Snake(
                headPosition: headPosition,
                direction: Direction.Right,
                snakeLength: 3
            );

            // Создание еды с проверкой свободного места
            Food = CreateInitialFood(Field, Snake);

            // Проверяем, удалось ли создать еду
            if(!Food.IsSuccess)
            {
                // Если нет свободного места - игру нельзя начать
                throw new InvalidOperationException(
                    "Нет свободного места для еды! Невозможно начать игру."
                );
            }
        }

        /// <summary>
        /// Создаёт начальную еду в случайном свободном месте
        /// </summary>
        private Food CreateInitialFood(PlayingField field, Snake snake)
        {
            // Сгенерировать случайную точку (координату) положения еды
            Point? position = GenerateRandomFoodPosition(field, snake);

            bool isSuccess; // флаг успешности операции

            if(position == null)
            {
                isSuccess = false; // нет свободного места
            }
            else
            {
                isSuccess = true; // еда успешно создана
            }

            // Возвращаем объект еды
            return new Food(
                position: position,
                isSuccess: isSuccess
            );
        }

        /// <summary>
        /// Генерирует случайное положение еды, не занятое змейкой
        /// </summary>
        private Point? GenerateRandomFoodPosition(PlayingField field, Snake snake)
        {
            int maxAttempts = 1000; // ограничиваем максимальное количество попыток

            for(int attempt = 0; attempt < maxAttempts; attempt++)
            {
                Random _random = new Random();
                int x = _random.Next(0, field.Width);   // случайная координата X
                int y = _random.Next(0, field.Height);  // случайная координата Y
                Point candidateFood = new Point(x, y);  // создаём координату

                // Проверяем, не занята ли эта клетка змейкой
                if(!snake.Contains(candidateFood))
                {
                    return candidateFood; // нашли свободное место!
                }
            }

            // Если не нашли свободное место после всех попыток
            return null;
        }
    }
}