using Snake.Core;
using Snake.Models;

namespace Snake.Interfaces
{
    public interface IGameRenderer
    {
        /// <summary>
        /// Очищает экран для отрисовки нового кадра
        /// </summary>
        void Clear();

        /// <summary>
        /// Отрисовывает текущее состояние игры
        /// </summary>
        /// <param name="state">Состояние игры</param>
        void Render(GameState state);
    }
}