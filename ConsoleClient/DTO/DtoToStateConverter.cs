using gameSnake.Core.State;
using gameSnake.Models;
using ConsoleClient.DTO;

namespace ConsoleClient.DTO
{
    /// <summary>
    /// Конвертирует DTO от сервера в GameState для переиспользования рендереров.
    /// Позволяет клиенту использовать те же рендереры, что и сервер.
    /// </summary>
    public static class DtoToStateConverter
    {
        /// <summary>
        /// Преобразует полученное от сервера DTO во внутренний GameState.
        /// Создаёт игровое поле, змейку, еду и заголовок из данных сервера,
        /// а также применяет статус к флагам состояния.
        /// </summary>
        /// <param name="serverState">Состояние игры, полученное от сервера</param>
        /// <returns>Готовый GameState для рендереров</returns>
        public static GameState Convert(GameStateDto serverState)
        {
            var field = new PlayingField(
                CalculateFieldWidth(serverState),
                CalculateFieldHeight(serverState));

            var snake = new Snake(
                serverState.Snake.Select(point => new Point(point.X, point.Y)));

            Point? foodPosition = serverState.Food is PointDto foodPoint
                ? new Point(foodPoint.X, foodPoint.Y)
                : null;
            var food = new Food(foodPosition, foodPosition != null);

            var header = new Header
            {
                Score = serverState.Score,
                Level = serverState.Level,
                Lives = serverState.Lives
            };

            var state = new GameState(header, field, snake, food);

            // Применяем статус к флагам
            switch (serverState.Status)
            {
                case GameStatus.Paused:
                    state.Flags.IsPaused = true;
                    state.ActiveMessage = GameMessageType.Pause;
                    break;
                case GameStatus.GameOver:
                    state.Flags.IsGameOver = true;
                    state.ActiveMessage = GameMessageType.GameOver;
                    break;
                case GameStatus.Win:
                    state.Flags.IsWin = true;
                    state.ActiveMessage = GameMessageType.Win;
                    break;
            }

            // Отображаем сообщение из сервера, если есть
            if (!string.IsNullOrEmpty(serverState.Message) && !state.ActiveMessage.HasValue)
            {
                state.Flags.IsPaused = true;
            }

            return state;
        }

        /// <summary>
        /// Вычисляет ширину игрового поля по максимальным координатам змейки и еды.
        /// Минимальная ширина — 20 клеток.
        /// </summary>
        /// <param name="serverState">Состояние игры от сервера</param>
        /// <returns>Ширина поля в клетках</returns>
        private static int CalculateFieldWidth(GameStateDto serverState)
        {
            int maximumCoordinate = 20;
            foreach (var segment in serverState.Snake)
                if (segment.X + 2 > maximumCoordinate) maximumCoordinate = segment.X + 2;

            if (serverState.Food is PointDto foodWidth && foodWidth.X + 2 > maximumCoordinate)
                maximumCoordinate = foodWidth.X + 2;

            return maximumCoordinate;
        }

        /// <summary>
        /// Вычисляет высоту игрового поля по максимальным координатам змейки и еды.
        /// Минимальная высота — 10 клеток.
        /// </summary>
        /// <param name="serverState">Состояние игры от сервера</param>
        /// <returns>Высота поля в клетках</returns>
        private static int CalculateFieldHeight(GameStateDto serverState)
        {
            int maximumCoordinate = 10;
            foreach (var segment in serverState.Snake)
                if (segment.Y + 2 > maximumCoordinate) maximumCoordinate = segment.Y + 2;

            if (serverState.Food is PointDto foodHeight && foodHeight.Y + 2 > maximumCoordinate)
                maximumCoordinate = foodHeight.Y + 2;

            return maximumCoordinate;
        }
    }
}
