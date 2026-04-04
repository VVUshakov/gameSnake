using System.Reflection;
using gameSnake.Attributes;
using gameSnake.Models;

namespace gameSnake.UI.ConsoleUI.ConsoleRenderers
{
    /// <summary>
    /// Форматирует заголовок игры в список строк для отрисовки.
    /// Использует рефлексию и атрибут HeaderInfo для автоматического обнаружения свойств.
    /// </summary>
    public static class HeaderFormatter
    {
        /// <summary>
        /// Преобразует свойства заголовка в список строк для отображения.
        /// </summary>
        /// <param name="header">Заголовок с игровыми параметрами</param>
        /// <returns>Список строк для отрисовки</returns>
        public static List<string> FormatLines(Header header)
        {
            var lines = new List<string>();
            PropertyInfo[] properties = typeof(Header).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in properties)
            {
                if (prop.GetCustomAttribute<HeaderInfoAttribute>() == null) continue;
                var value = prop.GetValue(header);
                if (value != null)
                    lines.Add($"{prop.Name}: {value}");
            }
            return lines;
        }

        /// <summary>
        /// Возвращает количество строк, необходимых для отрисовки заголовка.
        /// </summary>
        /// <param name="header">Заголовок с игровыми параметрами</param>
        /// <returns>Количество строк</returns>
        public static int GetHeight(Header header) => FormatLines(header).Count;
    }
}
