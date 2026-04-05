using gameSnake.Core.State;
using gameSnake.Models;

namespace gameSnake.Logic.SnakeLogic
{
    /// <summary>
    /// Шаг движения змейки: вычисляет новую позицию головы и добавляет её к телу.
    /// </summary>
    [GameStepOrder(0)]
    public class MoveStep : IGameStep
    {
        /// <summary>
        /// Вычисляет новую позицию головы и добавляет её к телу змейки.
        /// </summary>
        /// <param name="state">Текущее состояние игры</param>
        /// <returns>False — шаг не прерывает выполнение</returns>
        public bool Apply(GameState state)
        {
            Point newHead = SnakeMovement.CalculateNewHead(state.Snake.Head, state.CurrentDirection);
            state.Snake.Body.Add(newHead);
            return false;
        }
    }
}
