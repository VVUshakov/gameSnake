using SnakeGame.GameObjects;

namespace SnakeGame.Interfaces
{
    /// <summary>
    /// Интерфейс для отрисовки игрового состояния
    /// </summary>
    public interface IRenderer
    {
        /// <summary>
        /// Отрисовывает текущее состояние игры
        /// </summary>
        /// <param name="snake">Змейка для отрисовки</param>
        /// <param name="food">Еда для отрисовки</param>
        /// <param name="score">Текущий счет</param>
        void DrawGame(Snake snake, Food food, int score);

        /// <summary>
        /// Отрисовывает экран завершения игры
        /// </summary>
        /// <param name="score">Финальный счет</param>
        void DrawGameOver(int score);

        /// <summary>
        /// Очищает экран игры
        /// </summary>
        void Clear();
    }
}