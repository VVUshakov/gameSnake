namespace Snake
{
    /// <summary>
    /// Служебная информация (заголовок), отображаемая над игровым полем
    /// </summary>
    public class Header
    {
        public int Score { get; set; } = 0;       // счёт
        //public int Level { get; set; } = 1;       // уровень (можно добавить в будущем)
        //public int Lives { get; set; } = 1;       // жизни (можно добавить в будущем)

        /// <summary>
        /// Возвращает количество строк, необходимых для отрисовки заголовка
        /// </summary>
        public int Height => GetLinesHeader().Length;

        /// <summary>
        /// Возвращает список строк для отрисовки заголовка
        /// </summary>
        public string[] GetLinesHeader()
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
