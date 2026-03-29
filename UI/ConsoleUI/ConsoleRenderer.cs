using Snake.Core;
using Snake.Interfaces;
using Snake.Models;
using Snake.Utils;
using SnakeType = Snake.Models.Snake;

namespace Snake.UI.ConsoleUI
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
        /// Запрашивает у пользователя повторную игру после окончания (ожидает нажатия клавиши)
        /// </summary>
        /// <returns>true, если пользователь хочет сыграть ещё, false в противном случае</returns>
        public bool AskPlayAgain()
        {
            ConsoleKeyInfo key = Console.ReadKey();
            
            // Обрабатываем английскую и русскую раскладку
            bool playAgain = key.KeyChar == 'y' || key.KeyChar == 'Y' ||
                            key.KeyChar == 'н' || key.KeyChar == 'Н';
            
            bool exit = key.KeyChar == 'n' || key.KeyChar == 'N' ||
                       key.KeyChar == 'т' || key.KeyChar == 'Т';

            // Если нажали Y/н — очищаем консоль для новой игры
            if(playAgain)
            {
                Console.Clear();
            }

            return playAgain;
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
            DrawSnake(state.Snake, state.Field, headerHeight);

            // Нарисовать еду
            DrawFood(state.Food, state.Field, headerHeight);

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
        /// <param name="field">Игровое поле для проверки границ</param>
        /// <param name="headerHeight">Высота заголовка для сдвига поля вниз</param>
        private static void DrawSnake(SnakeType snake, PlayingField field, int headerHeight)
        {
            int lastSegmentIndex = snake.Body.Count - 1;  // индекс последнего сегмента (голова)

            for(int i = 0; i <= lastSegmentIndex; i++)
            {
                Point segment = snake.Body[i];

                // Пропускаем сегменты за границами поля (рисуем только между рамками)
                if(!field.IsInside(segment))
                    continue;

                char symbol = (i == lastSegmentIndex) ? SnakeHead : SnakeBody;

                Console.SetCursorPosition(segment.X, segment.Y + headerHeight);
                Console.Write(symbol);
            }
        }

        /// <summary>
        /// Отрисовывает еду на игровом поле
        /// </summary>
        /// <param name="food">Объект еды</param>
        /// <param name="field">Игровое поле для проверки границ</param>
        /// <param name="headerHeight">Высота заголовка для сдвига поля вниз</param>
        private static void DrawFood(Food food, PlayingField field, int headerHeight)
        {
            if(!food.IsSuccess || food.Position == null)
                return;

            Point pos = food.Position;

            // Пропускаем еду за границами поля (рисуем только между рамками)
            if(!field.IsInside(pos))
                return;

            Console.SetCursorPosition(pos.X, pos.Y + headerHeight);
            Console.Write(FoodSymbol);
        }

        /// <summary>
        /// Отрисовывает сообщение о проигрыше
        /// </summary>
        /// <param name="field">Объект игрового поля</param>
        /// <param name="headerHeight">Высота заголовка для сдвига поля вниз</param>
        private static void DrawGameOver(PlayingField field, int headerHeight)
        {
            string[] message = new string[]
            {
                "ИГРА ОКОНЧЕНА!",
                "",
                "Хотите сыграть ещё?",
                "Нажмите Y для продолжения",
                "Нажмите N для выхода"
            };

            DrawCenteredMessage(field, message, headerHeight, GameOverColor);
        }

        /// <summary>
        /// Отрисовывает сообщение о победе
        /// </summary>
        /// <param name="field">Объект игрового поля</param>
        /// <param name="headerHeight">Высота заголовка для сдвига поля вниз</param>
        private static void DrawGameWin(PlayingField field, int headerHeight)
        {
            string[] message = new string[]
            {
                "ПОБЕДА!",
                "",
                "Хотите сыграть ещё?",
                "Нажмите Y для продолжения",
                "Нажмите N для выхода"
            };

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
            int messageWidth = PositionCalculator.GetMessageWidth(lines);
            int messageHeight = PositionCalculator.GetMessageHeight(lines);

            // Передаём высоту поля с учётом заголовка для правильного центрирования
            Point startPosition = PositionCalculator.CalculateCenteredMessagePosition(
                field.Width,
                field.Height + headerHeight,
                messageWidth,
                messageHeight);

            // Проверяем, что позиция в пределах поля
            if(startPosition.X < 0 || startPosition.Y < headerHeight)
                return;

            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;

            for(int i = 0; i < lines.Length; i++)
            {
                int y = startPosition.Y + i;
                // Проверяем, что строка не выходит за границы поля
                if(y >= headerHeight && y < headerHeight + field.Height)
                {
                    Console.SetCursorPosition(startPosition.X, y);
                    Console.Write(lines[i]);
                }
            }

            Console.ForegroundColor = originalColor;
        }
    }
}