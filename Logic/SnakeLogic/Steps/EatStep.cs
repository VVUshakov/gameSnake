using gameSnake.Attributes;
using gameSnake.Core.State;
using gameSnake.Interfaces;
using gameSnake.Models;

namespace gameSnake.Logic.SnakeLogic.Steps
{
    /// <summary>
    /// Шаг поедания еды: проверяет, съела ли змейка еду, обновляет счёт и создаёт новую еду.
    /// Если еда не съедена, удаляет хвост змейки.
    /// </summary>
    [GameStepOrder(1)]
    public class EatStep : IUpdateStep
    {
        /// <summary>
        /// Проверяет поедание еды, обновляет счёт и создаёт новую еду.
        /// Если еда не съедена, удаляет хвост змейки.
        /// </summary>
        /// <param name="state">Текущее состояние игры</param>
        /// <returns>True, если змейка заполнила всё поле (победа)</returns>
        public bool Apply(GameState state)
        {
            if (FoodHandler.IsFoodEaten(state.Snake, state.Food))
            {
                state.Header.Score += state.Food.PointsValue;
                state.Food = FoodHandler.RespawnFood(state.Field, state.Snake);

                if (!state.Food.IsSuccess)
                {
                    state.Flags.IsWin = true;
                    state.ActiveMessage = GameMessage.Win;
                    return true;
                }
            }
            else
            {
                state.Snake.Body.Remove(state.Snake.Tail);
            }
            return false;
        }
    }
}
