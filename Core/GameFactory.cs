using Snake.Models;
using Snake.Utils;
using SnakeType = Snake.Models.Snake;

namespace Snake.Core
{
    /// <summary>
    /// Фабрика для создания начального состояния игры.
    /// Отвечает за инициализацию поля, змейки и еды.
    /// </summary>
    public static class GameFactory
    {
        /// <summary>
        /// Создаёт новое начальное состояние игры.
        /// </summary>
        /// <param name="fieldWidth">Ширина поля (по умолчанию 30)</param>
        /// <param name="fieldHeight">Высота поля (по умолчанию 15)</param>
        /// <param name="initialSnakeLength">Начальная длина змейки (по умолчанию 3)</param>
        /// <returns>Готовое состояние игры</returns>
        /// <exception cref="InvalidOperationException">Если нет места для еды</exception>
        public static GameState CreateGameState(
            int fieldWidth = 30,
            int fieldHeight = 15,
            int initialSnakeLength = 3)
        {
            var state = new GameState();

            // Создаём поле
            state.Field = new PlayingField(fieldWidth, fieldHeight);

            // Рассчитываем позицию головы для центрирования змейки
            Point headPosition = PositionCalculator.CalculateCenteredHeadPosition(
                state.Field.Width,
                state.Field.Height,
                initialSnakeLength,
                Direction.Right
            );

            // Создаём змейку
            state.Snake = new SnakeType(headPosition, Direction.Right, initialSnakeLength);

            // Создаём еду
            state.Food = FoodSpawner.CreateFood(state.Field, state.Snake);

            if(!state.Food.IsSuccess)
            {
                throw new InvalidOperationException("Нет свободного места для еды! Невозможно начать игру.");
            }

            return state;
        }
    }
}
