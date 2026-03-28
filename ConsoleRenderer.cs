namespace Snake
{
    /// <summary>
    /// Отрисовывает игру в консоли.
    /// Реализует отрисовку игрового поля, змейки, еды, служебной информации
    /// и сообщений (пауза, проигрыш, победа).
    /// </summary>
    public class ConsoleRenderer : IGameRenderer
    {
        // Символы отрисовки
        /// <summary>
        /// Символ рамки игрового поля
        /// </summary>
        private const char BorderChar = '#';

        /// <summary>
        /// Символ головы змейки
        /// </summary>
        private const char SnakeHead = 'O';

        /// <summary>
        /// Символ тела змейки
        /// </summary>
        private const char SnakeBody = '*';

        /// <summary>
        /// Символ еды
        /// </summary>
        private const char FoodSymbol = '@';

        // Цвета сообщений
        /// <summary>
        /// Цвет сообщения о проигрыше
        /// </summary>
        private const ConsoleColor GameOverColor = ConsoleColor.Red;

        /// <summary>
        /// Цвет сообщения о победе
        /// </summary>
        private const ConsoleColor GameWinColor = ConsoleColor.Green;

        /// <summary>
        /// Цвет сообщения о паузе
        /// </summary>
        private const ConsoleColor PauseColor = ConsoleColor.Yellow;

        /// <summary>
        /// Цвет сообщения по умолчанию
        /// </summary>
        private const ConsoleColor DefaultMessageColor = ConsoleColor.White;

        /// <summary>
        /// Очищает консоль перед отрисовкой нового кадра
        /// </summary>
        public void Clear()
        {
            Console.Clear();
        }

        /// <summary>
        /// Отрисовывает текущее состояние игры
        /// </summary>
        /// <param name="state">Состояние игры</param>
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
        /// <param name="header">Объект служебной информации</param>
        private static void DrawHeader(Header header)
        {
            string[] lines = header.GetLines();

            for(int i = 0; i < lines.Length; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(lines[i]);
            }
        }

        /// <summary>
        /// Отрисовывает игровое поле с рамкой
        /// </summary>
        /// <param name="field">Объект игрового поля</param>
        /// <param name="headerHeight">Высота заголовка для сдвига поля вниз</param>
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

        /// <summary>
        /// Отрисовывает змейку на игровом поле
        /// </summary>
        /// <param name="snake">Объект змейки</param>
        /// <param name="headerHeight">Высота заголовка для сдвига поля вниз</param>
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

        /// <summary>
        /// Отрисовывает еду на игровом поле
        /// </summary>
        /// <param name="food">Объект еды</param>
        /// <param name="headerHeight">Высота заголовка для сдвига поля вниз</param>
        private static void DrawFood(Food food, int headerHeight)
        {
            if(food.IsSuccess)
            {
                Console.SetCursorPosition(food.Position.X, food.Position.Y + headerHeight);
                Console.Write(FoodSymbol);
            }
        }

        /// <summary>
        /// Отрисовывает сообщение о проигрыше
        /// </summary>
        /// <param name="field">Объект игрового поля</param>
        /// <param name="headerHeight">Высота заголовка для сдвига поля вниз</param>
        private static void DrawGameOver(PlayingField field, int headerHeight)
        {
            string[] message = new string[] { "ИГРА ОКОНЧЕНА!" };
            DrawCenteredMessage(field, message, headerHeight, GameOverColor);
        }

        /// <summary>
        /// Отрисовывает сообщение о победе
        /// </summary>
        /// <param name="field">Объект игрового поля</param>
        /// <param name="headerHeight">Высота заголовка для сдвига поля вниз</param>
        private static void DrawGameWin(PlayingField field, int headerHeight)
        {
            string[] message = new string[] { "ПОБЕДА!" };
            DrawCenteredMessage(field, message, headerHeight, GameWinColor);
        }

        /// <summary>
        /// Отрисовывает сообщение о паузе
        /// </summary>
        /// <param name="field">Объект игрового поля</param>
        /// <param name="headerHeight">Высота заголовка для сдвига поля вниз</param>
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

        /// <summary>
        /// Отрисовывает центрированное сообщение на игровом поле
        /// </summary>
        /// <param name="field">Объект игрового поля</param>
        /// <param name="lines">Строки сообщения</param>
        /// <param name="headerHeight">Высота заголовка для сдвига поля вниз</param>
        /// <param name="color">Цвет текста сообщения</param>
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