using System.Text.Json.Serialization;

namespace gameSnake.Server.DTO
{
    /// <summary>
    /// Статус игровой сессии
    /// </summary>
    public enum GameStatus
    {
        Playing,
        Paused,
        GameOver,
        Win
    }

    /// <summary>
    /// Точка для сериализации (позиция сегмента змейки или еды)
    /// </summary>
    public record PointDto(int X, int Y);

    /// <summary>
    /// Состояние игры для отправки клиенту
    /// </summary>
    public class GameStateDto
    {
        [JsonPropertyName("status")]
        public GameStatus Status { get; set; }

        [JsonPropertyName("snake")]
        public List<PointDto> Snake { get; set; } = new();

        [JsonPropertyName("food")]
        public PointDto? Food { get; set; }

        [JsonPropertyName("score")]
        public int Score { get; set; }

        [JsonPropertyName("level")]
        public int Level { get; set; }

        [JsonPropertyName("lives")]
        public int Lives { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }

    /// <summary>
    /// Команда от клиента к серверу
    /// </summary>
    public class ClientCommand
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("direction")]
        public string? Direction { get; set; }
    }
}
