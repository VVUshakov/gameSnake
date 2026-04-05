using GameState = gameSnake.Core.State.GameState;
using gameSnake.Models;

namespace gameSnake.Interfaces
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