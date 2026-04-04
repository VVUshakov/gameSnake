using gameSnake.Servises;
using gameSnake.Core;
using gameSnake.Interfaces;
using gameSnake.Models;
using gameSnake.Utils;

namespace gameSnake.UI.ConsoleUI
{
    /// <summary>
    /// Отрисовывает игру в консоли.
    /// Реализует отрисовку игрового поля, змейки, еды, служебной информации
    /// и сообщений (пауза, проигрыш, победа).
    /// </summary>
    public class ConsoleRenderer : IGameRenderer
    {
        // Символы отрисовки
        private const char BorderChar = '#';
        private const char SnakeHead = 'O';
        private const char SnakeBody = '*';
        private const char FoodSymbol = '@';

        // Цвета сообщений
        private const ConsoleColor GameOverColor = ConsoleColor.Red;
        private const ConsoleColor GameWinColor = ConsoleColor.Green;
        private const ConsoleColor PauseColor = ConsoleColor.Yellow;
        private const ConsoleColor DefaultMessageColor = ConsoleColor.White;

        /// <summary>
        /// Очищает консоль перед отрисовкой нового кадра
        /// </summary>
        public void Clear() => Console.Clear();

        /// <summary>
        /// Отрисовывает текущее состояние игры
        /// </summary>
        public void Render(GameState state)
        {
            int headerHeight = state.Header.Height;
            DrawHeader(state.Header);
            DrawField(state.Field, headerHeight);
            DrawSnake(state.Snake, state.Field, headerHeight);
            DrawFood(state.Food, state.Field, headerHeight);

            if(state.IsGameOver) DrawGameOver(state.Field, headerHeight);
            if(state.IsWin) DrawGameWin(state.Field, headerHeight);
            if(state.IsPaused) DrawPause(state.Field, headerHeight);
        }

        /// <summary>
        /// Отрисовывает служебную информацию (счёт, уровень и т.п.)
        /// </summary>
        private static void DrawHeader(Header header)
        {
            List<string> lines = header.GetLines();
            for(int i = 0; i < lines.Count; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(lines[i]);
            }
        }

        /// <summary>
        /// Отрисовывает игровое поле с рамкой
        /// </summary>
        private static void DrawField(PlayingField field, int headerHeight)
        {
            int lastRow = field.Height - 1;
            int lastCol = field.Width - 1;
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
        private static void DrawSnake(Snake snake, PlayingField field, int headerHeight)
        {
            int lastSegmentIndex = snake.Body.Count - 1;
            for(int i = 0; i <= lastSegmentIndex; i++)
            {
                Point segment = snake.Body[i];
                if(!field.IsInside(segment)) continue;
                char symbol = (i == lastSegmentIndex) ? SnakeHead : SnakeBody;
                Console.SetCursorPosition(segment.X, segment.Y + headerHeight);
                Console.Write(symbol);
            }
        }

        /// <summary>
        /// Отрисовывает еду на игровом поле
        /// </summary>
        private static void DrawFood(Food food, PlayingField field, int headerHeight)
        {
            if(!food.IsSuccess || food.Position == null) return;
            Point pos = food.Position;
            if(!field.IsInside(pos)) return;
            Console.SetCursorPosition(pos.X, pos.Y + headerHeight);
            Console.Write(FoodSymbol);
        }

        /// <summary>
        /// Отрисовывает сообщение о проигрыше
        /// </summary>
        private static void DrawGameOver(PlayingField field, int headerHeight)
        {
            string[] message = ServiseMessange.GetGameOverMessange();
            DrawCenteredMessage(field, message, headerHeight, GameOverColor);
        }

        /// <summary>
        /// Отрисовывает сообщение о победе
        /// </summary>
        private static void DrawGameWin(PlayingField field, int headerHeight)
        {
            string[] message = ServiseMessange.GetGameWinMessange();
            DrawCenteredMessage(field, message, headerHeight, GameWinColor);
        }

        /// <summary>Отрисовывает сообщение о паузе</summary>
        private static void DrawPause(PlayingField field, int headerHeight)
        {
            string[] message = ServiseMessange.GetPauseMessange();
            DrawCenteredMessage(field, message, headerHeight, PauseColor);
        }

        /// <summary>
        /// Отрисовывает центрированное сообщение на игровом поле
        /// </summary>
        private static void DrawCenteredMessage(PlayingField field, string[] lines, int headerHeight, ConsoleColor color = DefaultMessageColor)
        {
            int messageWidth = MessageSizer.GetWidth(lines);
            int messageHeight = MessageSizer.GetHeight(lines);

            Point startPosition = PositionCalculator.CalculateCenteredMessagePosition(
                field.Width, field.Height + headerHeight, messageWidth, messageHeight);

            if(startPosition.X < 0 || startPosition.Y < headerHeight) return;

            ConsoleColor originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;

            for(int i = 0; i < lines.Length; i++)
            {
                int y = startPosition.Y + i;
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