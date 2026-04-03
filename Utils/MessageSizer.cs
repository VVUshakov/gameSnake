namespace Snake.Utils
{
    /// <summary>
    /// Предоставляет методы для вычисления размеров текстовых сообщений.
    /// </summary>
    public static class MessageSizer
    {
        /// <summary>
        /// Вычисляет ширину сообщения (длину самой длинной строки).
        /// </summary>
        /// <param name="lines">Строки сообщения</param>
        /// <returns>Максимальная длина строки</returns>
        public static int GetWidth(string[] lines)
        {
            int maxWidth = 0;
            foreach(string line in lines)
            {
                if(line.Length > maxWidth)
                {
                    maxWidth = line.Length;
                }
            }
            return maxWidth;
        }

        /// <summary>
        /// Вычисляет высоту сообщения (количество строк).
        /// </summary>
        /// <param name="lines">Строки сообщения</param>
        /// <returns>Количество строк</returns>
        public static int GetHeight(string[] lines)
        {
            return lines.Length;
        }

        /// <summary>
        /// Вычисляет максимальную ширину среди коллекции сообщений.
        /// </summary>
        /// <param name="messages">Коллекция сообщений (массивов строк)</param>
        /// <returns>Максимальная длина строки среди всех сообщений</returns>
        public static int GetMaxWidth(IEnumerable<string[]> messages)
        {
            int maxWidth = 0;
            foreach(var message in messages)
            {
                int width = GetWidth(message);
                if(width > maxWidth)
                {
                    maxWidth = width;
                }
            }
            return maxWidth;
        }

        /// <summary>
        /// Вычисляет максимальную высоту среди коллекции сообщений.
        /// </summary>
        /// <param name="messages">Коллекция сообщений (массивов строк)</param>
        /// <returns>Максимальное количество строк среди всех сообщений</returns>
        public static int GetMaxHeight(IEnumerable<string[]> messages)
        {
            int maxHeight = 0;
            foreach(var message in messages)
            {
                int height = GetHeight(message);
                if(height > maxHeight)
                {
                    maxHeight = height;
                }
            }
            return maxHeight;
        }
    }
}
