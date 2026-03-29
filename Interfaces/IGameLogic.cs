using Snake.Core;
using Snake.Models;

namespace Snake.Interfaces
{
    /// <summary>
    /// Интерфейс игровой логики.
    /// Определяет метод обновления состояния игры.
    /// </summary>
    public interface IGameLogic
    {
        /// <summary>
        /// Обновляет состояние игры: перемещает змейку, проверяет столкновения,
        /// обрабатывает поедание еды.
        /// </summary>
        /// <param name="state">Текущее состояние игры, которое будет обновлено</param>
        void Update(GameState state);
    }
}