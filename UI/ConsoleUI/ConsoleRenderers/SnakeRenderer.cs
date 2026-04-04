using gameSnake.Models;

namespace gameSnake.UI.ConsoleUI.ConsoleRenderers
{
    /// <summary>
    /// Отвечает за отрисовку змейки на игровом поле.
    /// Голова и тело отображаются разными символами.
    /// </summary>
    public static class SnakeRenderer
    {
        /// <summary>
        /// Отрисовывает все сегменты змейки на игровом поле.
        /// </summary>
        /// <param name="snake">Змейка с телом</param>
        /// <param name="field">Игровое поле для проверки границ</param>
        /// <param name="headerHeight">Высота заголовка для смещения по вертикали</param>
        public static void Draw(Snake snake, PlayingField field, int headerHeight)
        {
            int lastSegmentIndex = snake.Body.Count - 1;
            for (int i = 0; i <= lastSegmentIndex; i++)
            {
                Point segment = snake.Body[i];
                if (!field.IsInside(segment)) continue;
                char symbol = (i == lastSegmentIndex) ? RenderConstants.SnakeHead : RenderConstants.SnakeBody;
                Console.SetCursorPosition(segment.X, segment.Y + headerHeight);
                Console.Write(symbol);
            }
        }
    }
}
