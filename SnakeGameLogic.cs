namespace Snake
{
    /// <summary>
    /// Реализует игровую логику змейки
    /// </summary>
    public class SnakeGameLogic : IGameLogic
    {
        private Random _random = new Random();

        public void Update(GameState state)
        {
            // Если игра уже окончена, ничего не делаем
            if(state.IsGameOver)
            {
                return;
            }

            // 1. Получаем текущую голову (последний элемент в списке)
            Point head = state.Snake.Head;

            // 2. Вычисляем новую голову в зависимости от направления
            Point newHead = head;

            switch(state.CurrentDirection)
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

            // 3. Добавляем новую голову в конец списка
            state.Snake.Body.Add(newHead);

            // 4. Проверяем, съедена ли еда (учитываем, что Position может быть null)
            bool foodEaten = false;
            if(state.Food.Position != null)
            {
                foodEaten = (newHead.X == state.Food.Position.X &&
                             newHead.Y == state.Food.Position.Y);
            }

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

            // 5. Проверяем столкновения
            CheckCollisions(state);
        }

        private void GenerateNewFood(GameState state)
        {
            // Считаем общее количество клеток на поле
            int totalCells = state.Field.Width * state.Field.Height;

            // Если змейка заняла всё поле - игрок выиграл
            if(state.Snake.Body.Count >= totalCells)
            {
                state.IsGameOver = true;
                return;
            }

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
                    // Если не нашли место - игра окончена (победа)
                    state.IsGameOver = true;
                    return;
                }
            }
            while(IsPositionOccupiedBySnake(state, newPosition)); // Проверить занимает ли змея эту позицию

            // Создаём новую еду на свободном месте
            state.Food.Position = newPosition;
            state.Food.IsSuccess = true;
        }

        // Проверить занимает ли змея эту позицию
        private bool IsPositionOccupiedBySnake(GameState state, Point position)
        {
            foreach(Point segment in state.Snake.Body)
            {
                if(segment.X == position.X && segment.Y == position.Y)
                    return true;
            }
            return false;
        }

        // Проверяем столкновения
        private void CheckCollisions(GameState state)
        {
            Point head = state.Snake.Head;

            // Проверка столкновения со стенами
            if(head.X < 0 || head.X >= state.Field.Width ||
                head.Y < 0 || head.Y >= state.Field.Height)
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