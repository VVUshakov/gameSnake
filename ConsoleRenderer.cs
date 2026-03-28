namespace Snake
{
    /// <summary>
    /// Отрисовывает игру в консоли
    /// </summary>
    public class ConsoleRenderer : IGameRenderer
    {
        // Символы отрисовки
        private const char BorderChar = '#';// символ рамки игрового поля
        private const char SnakeHead = 'O'; // символ головы змейки 
        private const char SnakeBody = '*'; // символ тела змейки
        private const char FoodSymbol = '@';// символ еды

        public void Clear()
        {
            Console.Clear();
        }

        public void Render(GameState state)
        {
            // Нарисовать игровое поле
            DrawField(state.Field);

            // Нарисовать змейку
            DrawSnake(state.Snake);

            // Нарисовать еду
            DrawFood(state.Food);

            // Нарисовать счёт
            DrawScore(state.Score);

            // Если игра окончена - показать сообщение
            if(state.IsGameOver)
            {
                DrawGameOver(state.Field);
            }

            // Если пауза - показать экран паузы
            if(state.IsPaused)
            {
                DrawPause(state.Field);
            }
        }

        private void DrawField(PlayingField field)
        {
            // Верхняя граница
            Console.SetCursorPosition(0, 0);
            Console.Write(BorderChar + new string(BorderChar, field.Width) + BorderChar);

            // Боковые границы
            for(int y = 0; y < field.Height; y++)
            {
                Console.SetCursorPosition(0, y + 1);
                Console.Write(BorderChar);
                Console.SetCursorPosition(field.Width + 1, y + 1);
                Console.Write(BorderChar);
            }

            // Нижняя граница
            Console.SetCursorPosition(0, field.Height + 1);
            Console.Write(BorderChar + new string(BorderChar, field.Width) + BorderChar);
        }

        private void DrawSnake(Snake snake)
        {
            for(int i = 0; i < snake.Body.Count; i++)
            {
                Point segment = snake.Body[i];

                // Голова змейки - другим символом
                char symbol = (i == snake.Body.Count - 1) ? SnakeHead : SnakeBody;

                Console.SetCursorPosition(segment.X + 1, segment.Y + 1);
                Console.Write(symbol);
            }
        }

        private void DrawFood(Food food)
        {
            if(food.Position != null)
            {
                Console.SetCursorPosition(food.Position.X + 1, food.Position.Y + 1);
                Console.Write(FoodSymbol);
            }
        }

        private void DrawScore(int score)
        {
            Console.SetCursorPosition(0, 0);
            Console.Write($"Счёт: {score}");
        }

        private void DrawGameOver(PlayingField field)
        {
            DrawCenteredMessage(field, "ИГРА ОКОНЧЕНА!");
        }

        private void DrawPause(PlayingField field)
        {
            DrawCenteredMessage(field, "ПАУЗА");
        }

        private void DrawCenteredMessage(PlayingField field, string message)
        {
            int centerX = field.Width / 2;
            int centerY = field.Height / 2;
            int messageX = centerX - message.Length / 2;

            Console.SetCursorPosition(messageX, centerY);
            Console.Write(message);
        }
    }
}