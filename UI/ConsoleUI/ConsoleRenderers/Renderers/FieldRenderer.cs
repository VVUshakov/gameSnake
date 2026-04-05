using gameSnake.Models;

namespace gameSnake.UI.ConsoleUI.ConsoleRenderers.Renderers
{
    /// <summary>
    /// Отвечает за отрисовку игрового поля с рамкой.
    /// Рамка обозначает границы, за которые змейка не может выйти.
    /// </summary>
    public static class FieldRenderer
    {
        /// <summary>
        /// Отрисовывает игровое поле с рамкой.
        /// </summary>
        /// <param name="field">Игровое поле с размерами</param>
        /// <param name="headerHeight">Высота заголовка для смещения по вертикали</param>
        public static void Draw(PlayingField field, int headerHeight)
        {
            int lastRow = field.Height - 1;
            int lastCol = field.Width - 1;
            for (int y = 0; y <= lastRow; y++)
            {
                Console.SetCursorPosition(0, y + headerHeight);
                for (int x = 0; x <= lastCol; x++)
                {
                    bool isBorder = (y == 0) || (y == lastRow) || (x == 0) || (x == lastCol);
                    Console.Write(isBorder ? RenderConstants.BorderChar : ' ');
                }
            }
        }
    }
}
