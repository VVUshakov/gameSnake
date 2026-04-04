using System.Reflection;

namespace gameSnake.Models
{
    /// <summary>
    /// Служебная информация (заголовок), отображаемая над игровым полем.
    /// Содержит счёт, уровень, жизни и другие игровые параметры.
    /// </summary>
    public class Header
    {
        /// <summary>
        /// Текущий счёт игрока
        /// </summary>
        [HeaderInfo]
        public int Score { get; set; } = 0;

        /// <summary>
        /// Текущий уровень
        /// </summary>
        [HeaderInfo]
        public int Level { get; set; } = 1;

        /// <summary>
        /// Количество жизней
        /// </summary>
        [HeaderInfo]
        public int Lives { get; set; } = 1;

        /// <summary>
        /// Возвращает количество строк, необходимых для отрисовки заголовка
        /// </summary>
        public int Height => GetLines().Count;

        /// <summary>
        /// Возвращает список строк для отрисовки заголовка.
        /// Автоматически включает все свойства с атрибутом HeaderInfo.
        /// </summary>
        public List<string> GetLines()
        {
            var lines = new List<string>();

            PropertyInfo[] properties = typeof(Header).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                // Пропускаем свойства без атрибута HeaderInfo
                if (prop.GetCustomAttribute<HeaderInfoAttribute>() == null) continue;

                object? value = prop.GetValue(this);
                if (value != null)
                {
                    lines.Add($"{prop.Name}: {value}");
                }
            }

            return lines;
        }
    }
}
