using gameSnake.Core;
using gameSnake.Interfaces;
using gameSnake.Models;
using gameSnake.Utils;

namespace gameSnake.Logic
{
    /// <summary>
    /// Реализует игровую логику змейки.
    /// Управляет движением змейки, столкновениями и поеданием еды.
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
            if(state.IsGameOver || state.IsWin) { return; }

            Point newHead = CalculateNewHeadPosition(state.Snake.Head, state.CurrentDirection);
            state.Snake.Body.Add(newHead);

            bool foodEaten = IsFoodEaten(newHead, state.Food);

            if(foodEaten)
            {
                state.Header.Score += state.Food.PointsValue;
                state.Food = FoodSpawner.SpawnFood(state.Field, state.Snake);

                // Если не удалось разместить еду — змейка заполнила всё поле
                if(!state.Food.IsSuccess)
                {
                    state.IsWin = true;
                    return;
                }
            }
            else
            {
                state.Snake.Body.Remove(state.Snake.Tail);
            }

            CheckCollisions(state);
        }

        /// <summary>
        /// Вычисляет новую позицию головы на основе текущего направления
        /// </summary>
        private static Point CalculateNewHeadPosition(Point head, Direction direction)
        {
            switch(direction)
            {
                case Direction.Up:
                    return new Point(head.X, head.Y - 1);
                case Direction.Down:
                    return new Point(head.X, head.Y + 1);
                case Direction.Left:
                    return new Point(head.X - 1, head.Y);
                case Direction.Right:
                    return new Point(head.X + 1, head.Y);
                default:
                    return head;
            }
        }

        /// <summary>
        /// Проверяет, съела ли змейка еду
        /// </summary>
        private static bool IsFoodEaten(Point head, Food food)
        {
            if(food.Position == null) return false;
            return head.X == food.Position.X && head.Y == food.Position.Y;
        }

        /// <summary>
        /// Проверяет столкновения змейки со стенами и собственным телом
        /// </summary>
        private void CheckCollisions(GameState state)
        {
            Point head = state.Snake.Head;

            // Столкновение со стенами (рамка поля)
            if(!state.Field.IsInside(head))
            {
                state.IsGameOver = true;
                return;
            }

            // Столкновение с собственным телом
            for(int i = 0; i < state.Snake.Body.Count - 1; i++)
            {
                Point segment = state.Snake.Body[i];
                if(segment.X == head.X && segment.Y == head.Y)
                {
                    state.IsGameOver = true;
                    return;
                }
            }
        }
    }
}

