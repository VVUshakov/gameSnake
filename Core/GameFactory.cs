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
            var state = new GameState();

            // Вычисляем минимальные размеры на основе сервисных сообщений
            var allMessages = ServiseMessange.GetAllMessages();
            int minWidth = MessageSizer.GetMaxWidth(allMessages) + 4;
            int minHeight = MessageSizer.GetMaxHeight(allMessages) + 4;

            // Корректируем размеры, если они меньше минимальных
            int finalWidth = fieldWidth;
            int finalHeight = fieldHeight;
            if(finalWidth < minWidth) finalWidth = minWidth;
            if(finalHeight < minHeight) finalHeight = minHeight;

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
    }
}
