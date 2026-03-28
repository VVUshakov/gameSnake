namespace Snake
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
        /// Текущий уровень (закомментировано для будущего расширения)
        /// </summary>
        //public int Level { get; set; } = 1;

        /// <summary>
        /// Количество жизней (закомментировано для будущего расширения)
        /// </summary>
        //public int Lives { get; set; } = 1;

        /// <summary>
        /// Возвращает количество строк, необходимых для отрисовки заголовка
        /// </summary>
        public int Height => GetLines().Length;

        /// <summary>
        /// Возвращает список строк для отрисовки заголовка
        /// </summary>
        public string[] GetLines()
        {
            var lines = new List<string>();

            // Счёт всегда отображается
            lines.Add($"Счёт: {Score}");

            //// Уровень отображается, если > 1
            //if (Level > 1)
            //    lines.Add($"Уровень: {Level}");

            //// Жизни отображаются, если > 1
            //if (Lives > 1)
            //    lines.Add($"Жизни: {Lives}");

            return lines.ToArray();
        }
    }
}
