namespace Snake
{
    /// <summary>
    /// Реализует игровую логику змейки.
    /// Управляет движением змейки, столкновениями и поеданием еды.
    /// </summary>
    public class SnakeGameLogic : IGameLogic
    {
        private readonly Random _random = new Random();

        /// <summary>
        /// Обновляет состояние игры: перемещает змейку, проверяет столкновения,
        /// обрабатывает поедание еды.
        /// </summary>
        /// <param name="state">Текущее состояние игры</param>
        public void Update(GameState state)
        {
            // Если игра уже окончена, ничего не делаем
            if(state.IsGameOver || state.IsWin) { return; }

            // Вычисляем новую позицию головы
            Point newHead = CalculateNewHeadPosition(state.Snake.Head, state.CurrentDirection);

            // Добавляем новую голову в конец списка
            state.Snake.Body.Add(newHead);

            // Проверяем, съедена ли еда
            bool foodEaten = IsFoodEaten(newHead, state.Food);

            if(foodEaten)
            {
                // Если съели еду - увеличиваем счёт и создаём новую еду
                state.Header.Score += state.Food.PointsValue;
                GenerateNewFood(state);
                // Хвост НЕ удаляем - змейка растёт
            }
            else
            {
                // Если не съели - удаляем хвост
                state.Snake.Body.Remove(state.Snake.Tail);
            }

            // Проверяем столкновения
            CheckCollisions(state);

            // Проверяем условие победы
            CheckWinCondition(state);
        }

        /// <summary>
        /// Вычисляет новую позицию головы на основе текущего направления
        /// </summary>
        /// <param name="head">Текущая позиция головы</param>
        /// <param name="direction">Направление движения</param>
        /// <returns>Новая позиция головы</returns>
        private static Point CalculateNewHeadPosition(Point head, Direction direction)
        {
            Point newHead = head;

            switch(direction)
            {
                case Direction.Up:
                    newHead = new Point(head.X, head.Y - 1);
                    break;

                case Direction.Down:
                    newHead = new Point(head.X, head.Y + 1);
                    break;

                case Direction.Left:
                    newHead = new Point(head.X - 1, head.Y);
                    break;

                case Direction.Right:
                    newHead = new Point(head.X + 1, head.Y);
                    break;
            }

            return newHead;
        }

        /// <summary>
        /// Проверяет, съела ли змейка еду
        /// </summary>
        /// <param name="head">Позиция головы змейки</param>
        /// <param name="food">Объект еды</param>
        /// <returns>true, если еда съедена, false в противном случае</returns>
        private static bool IsFoodEaten(Point head, Food food)
        {
            if(food.Position == null) return false;

            return head.X == food.Position.X && head.Y == food.Position.Y;
        }

        /// <summary>
        /// Генерирует новую еду в случайном свободном месте
        /// </summary>
        /// <param name="state">Состояние игры</param>
        private void GenerateNewFood(GameState state)
        {
            // Пытаемся найти свободную клетку
            int maxAttempts = 1000;
            int attempts = 0;

            Point? newPosition = null;

            do
            {
                int x = _random.Next(0, state.Field.Width);
                int y = _random.Next(0, state.Field.Height);
                newPosition = new Point(x, y);
                attempts++;

                if(attempts > maxAttempts)
                {
                    // Если не нашли место — свободных клеток нет
                    return;
                }
            }
            while(IsPositionOccupiedBySnake(state, newPosition));

            // Создаём новую еду на свободном месте
            state.Food.Position = newPosition;
            state.Food.IsSuccess = true;
        }

        /// <summary>
        /// Проверяет условие победы — змейка заняла всё поле
        /// </summary>
        /// <param name="state">Состояние игры</param>
        private static void CheckWinCondition(GameState state)
        {
            int totalCells = state.Field.Width * state.Field.Height;

            if(state.Snake.Body.Count >= totalCells)
            {
                state.IsWin = true;
            }
        }

        /// <summary>
        /// Проверяет, занята ли позиция змейкой
        /// </summary>
        /// <param name="state">Состояние игры</param>
        /// <param name="position">Позиция для проверки</param>
        /// <returns>true, если позиция занята, false в противном случае</returns>
        private static bool IsPositionOccupiedBySnake(GameState state, Point position)
        {
            foreach(Point segment in state.Snake.Body)
            {
                if(segment.X == position.X && segment.Y == position.Y)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Проверяет столкновения змейки со стенами и собственным телом
        /// </summary>
        /// <param name="state">Состояние игры</param>
        private void CheckCollisions(GameState state)
        {
            Point head = state.Snake.Head;

            // Проверка столкновения со стенами (рамка поля)
            if(!state.Field.IsInside(head))
            {
                state.IsGameOver = true;
                return;
            }

            // Проверка столкновения с собственным телом
            // Проходим по всем сегментам, кроме головы (последнего)
            for(int i = 0; i < state.Snake.Body.Count - 1; i++)
            {
                Point segment = state.Snake.Body[i];
                if(segment.X == head.X && segment.Y == head.Y)
                {
                    state.IsGameOver = true;
                    return;
                }
            }
        }
    }
}