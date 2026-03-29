namespace Snake.Models
{
    /// <summary>
    /// Направления движения змейки в игровом поле
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// Движение вверх (уменьшение координаты Y)
        /// </summary>
        Up,

        /// <summary>
        /// Движение вниз (увеличение координаты Y)
        /// </summary>
        Down,

        /// <summary>
        /// Движение влево (уменьшение координаты X)
        /// </summary>
        Left,

        /// <summary>
        /// Движение вправо (увеличение координаты X)
        /// </summary>
        Right
    }
}