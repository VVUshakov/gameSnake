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
        public int Score { get; set; } = 0;

        /// <summary>
        /// Текущий уровень
        /// </summary>
        public int Level { get; set; } = 1;

        /// <summary>
        /// Количество жизней
        /// </summary>
        public int Lives { get; set; } = 1;

        /// <summary>
        /// Возвращает количество строк, необходимых для отрисовки заголовка
        /// </summary>
        public int Height => GetLines().Length;

        /// <summary>
        /// Возвращает список строк для отрисовки заголовка.
        /// Автоматически включает все публичные свойства класса.
        /// </summary>
        public string[] GetLines()
        {
            var lines = new List<string>();

            // Автоматически находим все публичные свойства и добавляем их значения
            PropertyInfo[] properties = typeof(Header).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach(var prop in properties)
            {
                object? value = prop.GetValue(this);
                if(value != null)
                {
                    lines.Add($"{prop.Name}: {value}");
                }
            }

            return lines.ToArray();
        }
    }
}
