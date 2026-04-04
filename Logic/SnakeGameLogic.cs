using gameSnake.Core;
using gameSnake.Interfaces;
using gameSnake.Logic.GameLogicComponents;
using gameSnake.Models;

namespace gameSnake.Logic
{
    /// <summary>
    /// Основная логика игры: обновление состояния змейки.
    /// Делегирует подзадачи отдельным компонентам: движению, еде, коллизиям.
    /// </summary>
    public class SnakeGameLogic : IGameLogic
    {
        /// <summary>
        /// Обновляет состояние игры: перемещает змейку, проверяет столкновения,
        /// обрабатывает поедание еды.
        /// </summary>
        /// <param name="state">Текущее состояние игры</param>
        public void Update(GameState state)
        {
            if (state.IsGameOver || state.IsWin || state.IsPaused) return;

            // 1. Движение
            Point newHead = StandardMovement.CalculateNewHead(state.Snake.Head, state.CurrentDirection);
            state.Snake.Body.Add(newHead);

            // 2. Проверка еды и удаление хвоста у змейки, если не сьедена
            if (StandardFoodHandler.IsFoodEaten(state.Snake, state.Food))
            {
                state.Header.Score += state.Food.PointsValue;
                state.Food = StandardFoodHandler.RespawnFood(state.Field, state.Snake);

                if (!state.Food.IsSuccess)
                {
                    state.IsWin = true;
                    return;
                }
            }
            else
            {
                state.Snake.Body.Remove(state.Snake.Tail);
            }

            // 3. Проверка столкновений
            if (StandardCollisionDetector.HasCollision(state.Snake, state.Field))
                state.IsGameOver = true;
        }
    }
}
