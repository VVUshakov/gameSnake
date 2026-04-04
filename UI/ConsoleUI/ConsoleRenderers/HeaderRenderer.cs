using gameSnake.Models;

namespace gameSnake.UI.ConsoleUI.ConsoleRenderers
{
    /// <summary>
    /// Отвечает за отрисовку заголовка игры (счёт, уровень, жизни).
    /// Выводит строки заголовка в верхнюю часть консоли.
    /// </summary>
    public static class HeaderRenderer
    {
        /// <summary>
        /// Отрисовывает строки заголовка в консоли.
        /// </summary>
        /// <param name="header">Заголовок с игровыми параметрами</param>
        public static void Draw(Header header)
        {
            List<string> lines = HeaderFormatter.FormatLines(header);
            for (int i = 0; i < lines.Count; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(lines[i]);
            }
        }
    }
}
