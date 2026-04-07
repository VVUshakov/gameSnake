using gameSnake.Core.State;
using gameSnake.Models;
using gameSnake.Utils;

namespace gameSnake.Core.Factories
{
    /// <summary>
    /// Фабрика для создания начального состояния игры.
    /// Принимает размеры поля как параметры — не зависит от UI-слоёв.
    /// </summary>
    public static class GameStateFactory
    {
        /// <summary>
        /// Создаёт новое начальное состояние игры.
        /// </summary>
        /// <param name="fieldWidth">Ширина игрового поля</param>
        /// <param name="fieldHeight">Высота игрового поля</param>
        /// <param name="initialSnakeLength">Начальная длина змейки (по умолчанию 3)</param>
        /// <returns>Готовое начальное состояние игры</returns>
        public static GameState Create(
            int fieldWidth,
            int fieldHeight,
            int initialSnakeLength = 3)
        {
            PlayingField field = new PlayingField(fieldWidth, fieldHeight);

            var (headPosition, finalSnakeLength) = PositionCalculator.CalculateCenteredHeadPosition(
                field.Width, field.Height, initialSnakeLength, Direction.Right);

            Snake snake = SnakeFactory.Create(headPosition, Direction.Right, finalSnakeLength);
            Food food = FoodFactory.CreateFood(field, snake);
            Header header = new Header();

            return new GameState(header, field, snake, food);
        }
    }
}
