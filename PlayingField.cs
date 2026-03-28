namespace Snake
{
    /// <summary>
    /// Представляет игровое поле.
    /// Определяет размеры поля (ширину и высоту).
    /// </summary>
    public class PlayingField
    {
        /// <summary>
        /// Ширина игрового поля в клетках
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Высота игрового поля в клетках
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Инициализирует новое игровое поле с указанными размерами
        /// </summary>
        /// <param name="width">Ширина поля в клетках (по умолчанию 20)</param>
        /// <param name="height">Высота поля в клетках (по умолчанию 10)</param>
        public PlayingField(int width = 20, int height = 10)
        {
            Width = width;
            Height = height;
        }
    }
}