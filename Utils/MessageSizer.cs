namespace gameSnake.Utils
{
    /// <summary>
    /// Вычисляет размеры текстовых сообщений.
    /// </summary>
    public static class MessageSizer
    {
        /// <summary>
        /// Возвращает размеры одного сообщения (ширина, высота).
        /// </summary>
        /// <param name="lines">Строки сообщения</param>
        /// <returns>Кортеж (ширина, высота)</returns>
        public static (int width, int height) GetSize(string[] lines)
        {
            int maxWidth = 0;
            foreach (string line in lines)
            {
                if (line.Length > maxWidth) maxWidth = line.Length;
            }
            return (maxWidth, lines.Length);
        }

        /// <summary>
        /// Возвращает максимальные размеры среди коллекции сообщений.
        /// </summary>
        /// <param name="messages">Коллекция сообщений (массивов строк)</param>
        /// <returns>Кортеж (максимальная ширина, максимальная высота)</returns>
        public static (int width, int height) GetMaxSize(IEnumerable<string[]> messages)
        {
            int maxWidth = 0, maxHeight = 0;
            foreach (var msg in messages)
            {
                var (w, h) = GetSize(msg);
                if (w > maxWidth) maxWidth = w;
                if (h > maxHeight) maxHeight = h;
            }
            return (maxWidth, maxHeight);
        }
    }
}
