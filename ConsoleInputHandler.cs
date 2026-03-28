namespace Snake
{
    /// <summary>
    /// Обрабатывает ввод с клавиатуры в консоли
    /// </summary>
    public class ConsoleInputHandler : IInputHandler
    {
        public void ProcessInput(GameState state)
        {
            // Если нет нажатых клавиш - выходим
            if(!Console.KeyAvailable) return;

            // Читаем клавишу (true - не отображать её на экране)
            ConsoleKeyInfo key = Console.ReadKey(true);

            // Если игра окончена - реагируем только на Escape
            if(state.IsGameOver)
            {
                if(key.Key == ConsoleKey.Escape)
                    state.IsExit = true;
                return;
            }

            // Обрабатываем стрелки
            switch(key.Key)
            {
                case ConsoleKey.UpArrow:
                    // Нельзя развернуться в противоположную сторону
                    if(state.CurrentDirection != Direction.Down)
                        state.CurrentDirection = Direction.Up;
                    break;

                case ConsoleKey.DownArrow:
                    if(state.CurrentDirection != Direction.Up)
                        state.CurrentDirection = Direction.Down;
                    break;

                case ConsoleKey.LeftArrow:
                    if(state.CurrentDirection != Direction.Right)
                        state.CurrentDirection = Direction.Left;
                    break;

                case ConsoleKey.RightArrow:
                    if(state.CurrentDirection != Direction.Left)
                        state.CurrentDirection = Direction.Right;
                    break;

                case ConsoleKey.Escape:
                    state.IsExit = true;
                    break;
            }
        }
    }
}