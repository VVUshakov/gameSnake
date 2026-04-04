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
            // Вычисляем итоговые размеры игрового поля
            (int finalWidth, int finalHeight) = GetFinalSize(fieldWidth, fieldHeight);

            // Создаём поле
            PlayingField field = new PlayingField(finalWidth, finalHeight);

            // Рассчитываем позицию головы для центрирования змейки
            var (headPosition, adjustedLength) = PositionCalculator.CalculateCenteredHeadPosition(
                field.Width, field.Height, initialSnakeLength, Direction.Right);

            // Создаём змейку
            Snake snake = new Snake(headPosition, Direction.Right, adjustedLength);

            // Создаём еду
            Food food = FoodSpawner.CreateFood(field, snake);

            // Создаём заголовок
            Header header = new Header();

            // Передаём всё в конструктор GameState
            return new GameState(header, field, snake, food);
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
