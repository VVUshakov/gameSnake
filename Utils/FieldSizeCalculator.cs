namespace gameSnake.Utils
{
    /// <summary>
    /// Вычисляет размеры игрового поля на основе максимальных размеров сообщений.
    /// </summary>
    public static class FieldSizeCalculator
    {
        private const int BorderPadding = 4;

        /// <summary>
        /// Вычисляет размеры игрового поля на основе максимальных размеров сообщений.
        /// </summary>
        /// <param name="maxMessageWidth">Максимальная ширина сообщения</param>
        /// <param name="maxMessageHeight">Максимальная высота сообщения</param>
        /// <returns>Размеры игрового поля (ширина, высота)</returns>
        public static (int width, int height) Calculate(int maxMessageWidth, int maxMessageHeight)
        {
            return (
                maxMessageWidth + BorderPadding,
                maxMessageHeight + BorderPadding
            );
        }
    }
}
