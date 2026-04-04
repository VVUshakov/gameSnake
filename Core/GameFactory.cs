using gameSnake.Models;
using gameSnake.Servises;
using gameSnake.Utils;

namespace gameSnake.Core
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
        public static GameState CreateGameState(
            int fieldWidth = 30,
            int fieldHeight = 15,
            int initialSnakeLength = 3)
        {
            // Создаем начальное состояние игры
            var state = new GameState();

            // Вычисляем итоговые размеры игрового поля
            // с учётом минимальных габаритов для сервисных сообщений
            (int finalWidth, int finalHeight) = GetFinalSize(fieldWidth, fieldHeight);

            // Создаём поле
            state.Field = new PlayingField(finalWidth, finalHeight);

            // Рассчитываем позицию головы для центрирования змейки
            Point headPosition = PositionCalculator.CalculateCenteredHeadPosition(
                state.Field.Width,
                state.Field.Height,
                initialSnakeLength,
                Direction.Right
            );

            // Создаём змейку
            state.Snake = new Snake(headPosition, Direction.Right, initialSnakeLength);

            // Создаём еду
            state.Food = FoodSpawner.CreateFood(state.Field, state.Snake);

            if(!state.Food.IsSuccess)
                throw new InvalidOperationException("Нет свободного места для еды! Невозможно начать игру.");

            return state;
        }

        /// <summary>
        /// Возвращает итоговые размеры поля, не меньше минимальных.
        /// Минимум вычисляется на основе габаритов сервисных сообщений + рамка(2) + запас(2).
        /// </summary>
        private static (int width, int height) GetFinalSize(int width, int height)
        {
            var messages = ServiseMessange.GetAllMessages();
            int minWidth = MessageSizer.GetMaxWidth(messages) + 4;
            int minHeight = MessageSizer.GetMaxHeight(messages) + 4;

            return (
                width < minWidth ? minWidth : width,
                height < minHeight ? minHeight : height
            );
        }
    }
}
