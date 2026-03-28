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

        // Цвета сообщений
        private const ConsoleColor GameOverColor = ConsoleColor.Red;        // цвет проигрыша
        private const ConsoleColor GameWinColor = ConsoleColor.Green;       // цвет выигрыша
        private const ConsoleColor PauseColor = ConsoleColor.Yellow;        // цвет паузы
        private const ConsoleColor DefaultMessageColor = ConsoleColor.White;// цвет по умолчанию

        public void Clear()
        {
            Console.Clear();
        }

        public void Render(GameState state)
        {
            int headerHeight = state.Header.Height;

            // Отрисовать служебную информацию
            DrawHeader(state.Header);

            // Нарисовать игровое поле
            DrawField(state.Field, headerHeight);

            // Нарисовать змейку
            DrawSnake(state.Snake, headerHeight);

            // Нарисовать еду
            DrawFood(state.Food, headerHeight);

            // Если игра проиграна - показать сообщение о проигрыше
            if(state.IsGameOver)
            {
                DrawGameOver(state.Field, headerHeight);
            }

            // Если игра выиграна - показать сообщение о победе
            if(state.IsWin)
            {
                DrawGameWin(state.Field, headerHeight);
            }

            // Если пауза - показать сообщение о паузе
            if(state.IsPaused)
            {
                DrawPause(state.Field, headerHeight);
            }
        }
                
        /// <summary>
        /// Отрисовывает служебную информацию (счёт, уровень и т.п.)
        /// </summary>
        private static void DrawHeader(Header header)
        {
            string[] lines = header.GetLines();

            for(int i = 0; i < lines.Length; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(lines[i]);
            }
        }

        private static void DrawField(PlayingField field, int headerHeight)
        {
            int lastRow = field.Height - 1;   // последний индекс строки (ширина - 1)
            int lastCol = field.Width - 1;    // последний индекс столбца (высота - 1)

            for(int y = 0; y <= lastRow; y++)
            {
                Console.SetCursorPosition(0, y + headerHeight);
                for(int x = 0; x <= lastCol; x++)
                {
                    bool isBorder = (y == 0) || (y == lastRow) || (x == 0) || (x == lastCol);

                    Console.Write(isBorder ? BorderChar : ' ');
                }
            }
        }

        private static void DrawSnake(Snake snake, int headerHeight)
        {
            int lastSegmentIndex = snake.Body.Count - 1;  // индекс последнего сегмента (голова)

            for(int i = 0; i <= lastSegmentIndex; i++)
            {
                Point segment = snake.Body[i];

                char symbol = (i == lastSegmentIndex) ? SnakeHead : SnakeBody;

                Console.SetCursorPosition(segment.X, segment.Y + headerHeight);
                Console.Write(symbol);
            }
        }

        private static void DrawFood(Food food, int headerHeight)
        {
            if(food.IsSuccess)
            {
                Console.SetCursorPosition(food.Position.X, food.Position.Y + headerHeight);
                Console.Write(FoodSymbol);
            }
        }

        private static void DrawGameOver(PlayingField field, int headerHeight)
        {
            string[] message = new string[] { "ИГРА ОКОНЧЕНА!" };
            DrawCenteredMessage(field, message, headerHeight, GameOverColor);
        }

        private static void DrawGameWin(PlayingField field, int headerHeight)
        {
            string[] message = new string[] { "ПОБЕДА!" };
            DrawCenteredMessage(field, message, headerHeight, GameWinColor);
        }

        private static void DrawPause(PlayingField field, int headerHeight)
        {
            string[] pauseBox = new string[]
            {
                "┌────────────────────────┐",
                "│       ── ПАУЗА ──      │",
                "│                        │",
                "│  P - продолжить        │",
                "│  Escape - выйти        │",
                "└────────────────────────┘"
            };

            DrawCenteredMessage(field, pauseBox, headerHeight, PauseColor);
        }

        private static void DrawCenteredMessage(PlayingField field, string[] lines, int headerHeight, ConsoleColor color = DefaultMessageColor)
        {
            int boxWidth = 0;
            foreach(string line in lines)
            {
                if(line.Length > boxWidth)
                {
                    boxWidth = line.Length;
                }
            }

            int boxHeight = lines.Length;
            int startX = (field.Width - boxWidth) / 2;
            // Центрируем сообщение внутри игрового поля (с учётом headerHeight)
            int startY = headerHeight + (field.Height - boxHeight) / 2;

            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;

            for(int i = 0; i < lines.Length; i++)
            {
                Console.SetCursorPosition(startX, startY + i);
                Console.Write(lines[i]);
            }

            Console.ForegroundColor = originalColor;
        }
    }
}