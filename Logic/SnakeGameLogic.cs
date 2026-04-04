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
        private readonly IMovementStrategy _movement;
        private readonly IFoodHandler _foodHandler;
        private readonly ICollisionDetector _collisionDetector;

        public SnakeGameLogic()
        {
            _movement = new StandardMovement();
            _foodHandler = new StandardFoodHandler();
            _collisionDetector = new StandardCollisionDetector();
        }

        /// <summary>
        /// Обновляет состояние игры: перемещает змейку, проверяет столкновения,
        /// обрабатывает поедание еды.
        /// </summary>
        /// <param name="state">Текущее состояние игры</param>
        public void Update(GameState state)
        {
            if (state.IsGameOver || state.IsWin || state.IsPaused) return;

            // 1. Движение
            Point newHead = _movement.CalculateNewHead(state.Snake.Head, state.CurrentDirection);
            state.Snake.Body.Add(newHead);

            // 2. Проверка еды
            if (_foodHandler.IsFoodEaten(state.Snake, state.Food))
            {
                state.Header.Score += state.Food.PointsValue;
                state.Food = _foodHandler.RespawnFood(state.Field, state.Snake);

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
            if (_collisionDetector.HasCollision(state.Snake, state.Field))
                state.IsGameOver = true;
        }
    }
}
