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
            // Создаем новое состояние
            var state = new GameState();

            // Вычисляем итоговые размеры с учётом минимально
            // необходимых габаритов для вывода сервисных сообщений
            int minWidth = GetMinWidth();
            int minHeight = GetMinHeight();
            int finalWidth = fieldWidth < minWidth ? minWidth : fieldWidth;
            int finalHeight = fieldHeight < minHeight ? minHeight : fieldHeight;

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
        /// Возвращает минимальную ширину поля (сообщение + рамка + запас)
        /// </summary>
        private static int GetMinWidth()
        {
            return MessageSizer.GetMaxWidth(ServiseMessange.GetAllMessages()) + 4;
        }

        /// <summary>
        /// Возвращает минимальную высоту поля (сообщение + рамка + запас)
        /// </summary>
        private static int GetMinHeight()
        {
            return MessageSizer.GetMaxHeight(ServiseMessange.GetAllMessages()) + 4;
        }
    }
}
