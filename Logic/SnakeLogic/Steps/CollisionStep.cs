using gameSnake.Attributes;
using gameSnake.Core.State;
using gameSnake.Interfaces;
using gameSnake.Models;

namespace gameSnake.Logic.SnakeLogic.Steps
{
    /// <summary>
    /// Шаг проверки столкновений: определяет столкновение змейки со стеной или собственным телом.
    /// </summary>
    [GameStepOrder(2)]
    public class CollisionStep : IUpdateStep
    {
        /// <summary>
        /// Проверяет столкновение змейки со стеной или собственным телом.
        /// </summary>
        /// <param name="state">Текущее состояние игры</param>
        /// <returns>True, если произошло столкновение (поражение)</returns>
        public bool Apply(GameState state)
        {
            if (CollisionDetector.HasCollision(state.Snake, state.Field))
            {
                state.Flags.IsGameOver = true;
                state.ActiveMessage = GameMessageType.GameOver;
                return true;
            }
            return false;
        }
    }
}
