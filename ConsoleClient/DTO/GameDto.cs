using System.Text.Json.Serialization;

namespace ConsoleClient.DTO
{
    /// <summary>
    /// Статус игровой сессии, отправляемый сервером.
    /// </summary>
    public enum GameStatus
    {
        /// <summary>
        /// Игра идёт — змейка двигается, ввод обрабатывается
        /// </summary>
        Playing,

        /// <summary>
        /// Пауза — змейка не двигывается до снятия паузы
        /// </summary>
        Paused,

        /// <summary>
        /// Игра окончена — змейка врезалась в стену или в себя
        /// </summary>
        GameOver,

        /// <summary>
        /// Победа — всё поле заполнено змейкой
        /// </summary>
        Win
    }

    /// <summary>
    /// Координата точки на игровом поле.
    /// Используется для позиций сегментов змейки и еды.
    /// </summary>
    /// <param name="X">Координата по горизонтали</param>
    /// <param name="Y">Координата по вертикали</param>
    public record PointDto(int X, int Y);

    /// <summary>
    /// Полное состояние игры, отправляемое сервером каждый кадр.
    /// Содержит все данные, необходимые клиенту для отрисовки.
    /// </summary>
    public class GameStateDto
    {
        /// <summary>
        /// Текущий статус игры (игра идёт, пауза, проигрыш, победа)
        /// </summary>
        [JsonPropertyName("status")]
        public GameStatus Status { get; set; }

        /// <summary>
        /// Список координат всех сегментов змейки (от хвоста к голове)
        /// </summary>
        [JsonPropertyName("snake")]
        public List<PointDto> Snake { get; set; } = new();

        /// <summary>
        /// Координата еды на поле. Null — если еды нет
        /// </summary>
        [JsonPropertyName("food")]
        public PointDto? Food { get; set; }

        /// <summary>
        /// Текущий счёт игрока
        /// </summary>
        [JsonPropertyName("score")]
        public int Score { get; set; }

        /// <summary>
        /// Текущий уровень сложности
        /// </summary>
        [JsonPropertyName("level")]
        public int Level { get; set; }

        /// <summary>
        /// Количество оставшихся жизней
        /// </summary>
        [JsonPropertyName("lives")]
        public int Lives { get; set; }

        /// <summary>
        /// Текстовое сообщение для отображения поверх поля (пауза, победа и т.д.)
        /// </summary>
        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }

    /// <summary>
    /// Команда, отправляемая клиентом на сервер.
    /// Описывает одно действие игрока.
    /// </summary>
    public class ClientCommand
    {
        /// <summary>
        /// Тип команды: "move", "pause", "restart", "exit"
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Направление движения (заполняется только для type="move")
        /// </summary>
        [JsonPropertyName("direction")]
        public string? Direction { get; set; }
    }
}
