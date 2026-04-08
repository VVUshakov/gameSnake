using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using ConsoleClient.DTO;

namespace ConsoleClient.Network
{
    /// <summary>
    /// WebSocket-подключение к игровому серверу.
    /// Отвечает за отправку команд и получение состояния игры.
    /// </summary>
    public class GameClient : IDisposable
    {
        /// <summary>
        /// Внутреннее WebSocket-соединение.
        /// </summary>
        private readonly ClientWebSocket _webSocket = new();

        /// <summary>
        /// Буфер для приёма данных от сервера. Размер — 4 КБ.
        /// </summary>
        private readonly byte[] _buffer = new byte[4096];

        /// <summary>
        /// Текущее состояние подключения.
        /// </summary>
        public WebSocketState State => _webSocket.State;

        /// <summary>
        /// Устанавливает соединение с сервером по указанному URL.
        /// </summary>
        /// <param name="url">Адрес сервера (например, ws://localhost:5000/ws)</param>
        /// <param name="ct">Токен отмены для прерывания подключения</param>
        public async Task ConnectAsync(string url, CancellationToken ct)
        {
            await _webSocket.ConnectAsync(new Uri(url), ct);
        }

        /// <summary>
        /// Сериализует команду в JSON и отправляет серверу.
        /// </summary>
        /// <param name="command">Команда (move, pause, restart, exit)</param>
        /// <param name="ct">Токен отмены</param>
        public async Task SendAsync(ClientCommand command, CancellationToken ct)
        {
            var json = JsonSerializer.Serialize(command);
            var bytes = Encoding.UTF8.GetBytes(json);
            await _webSocket.SendAsync(
                new ArraySegment<byte>(bytes),
                WebSocketMessageType.Text, true, ct);
        }

        /// <summary>
        /// Принимает сообщение от сервера и десериализует его в GameStateDto.
        /// Возвращает null, если сообщение не текстовое или пустое.
        /// </summary>
        /// <param name="ct">Токен отмены</param>
        /// <returns>Состояние игры или null при ошибке</returns>
        public async Task<GameStateDto?> ReceiveStateAsync(CancellationToken ct)
        {
            var result = await _webSocket.ReceiveAsync(
                new ArraySegment<byte>(_buffer), ct);

            if (result.MessageType != WebSocketMessageType.Text || result.Count == 0)
                return null;

            var json = Encoding.UTF8.GetString(_buffer, 0, result.Count);
            return JsonSerializer.Deserialize<GameStateDto>(json);
        }

        /// <summary>
        /// Корректно закрывает соединение, отправляя сигнал закрытия серверу.
        /// </summary>
        /// <param name="ct">Токен отмены</param>
        public async Task CloseAsync(CancellationToken ct)
        {
            if (_webSocket.State == WebSocketState.Open)
                await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Bye", ct);
        }

        /// <summary>
        /// Освобождает ресурсы WebSocket-соединения.
        /// </summary>
        public void Dispose() => _webSocket.Dispose();
    }
}
