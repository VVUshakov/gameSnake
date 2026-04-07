using gameSnake.Core.State;
using gameSnake.Interfaces;
using gameSnake.Models;
using gameSnake.Server.DTO;
using System.Net.WebSockets;
using System.Text.Json;

namespace gameSnake.Server.Renderers
{
    /// <summary>
    /// Рендерер, отправляющий состояние игры клиенту через WebSocket.
    /// </summary>
    public class WebSocketRenderer : IGameRenderer
    {
        private readonly WebSocket _webSocket;

        public WebSocketRenderer(WebSocket webSocket)
        {
            _webSocket = webSocket;
        }

        public void Clear() { }

        public void Render(GameState state)
        {
            var dto = ToDto(state);
            var json = JsonSerializer.Serialize(dto);
            var buffer = System.Text.Encoding.UTF8.GetBytes(json);
            _webSocket.SendAsync(
                new ArraySegment<byte>(buffer),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None).Wait();
        }

        private static GameStateDto ToDto(GameState state)
        {
            var dto = new GameStateDto
            {
                Status = ResolveStatus(state.Flags),
                Snake = state.Snake.Body.Select(p => new PointDto(p.X, p.Y)).ToList(),
                Food = state.Food.IsSuccess && state.Food.Position.HasValue
                    ? new PointDto(state.Food.Position.Value.X, state.Food.Position.Value.Y)
                    : null,
                Score = state.Header.Score,
                Level = state.Header.Level,
                Lives = state.Header.Lives,
                Message = state.ActiveMessage switch
                {
                    GameMessageType.Pause => "Пауза",
                    GameMessageType.GameOver => "Игра окончена!",
                    GameMessageType.Win => "Победа!",
                    _ => null
                }
            };
            return dto;
        }

        private static GameStatus ResolveStatus(GameFlags flags)
        {
            if (flags.IsWin) return GameStatus.Win;
            if (flags.IsGameOver) return GameStatus.GameOver;
            if (flags.IsPaused) return GameStatus.Paused;
            return GameStatus.Playing;
        }
    }
}
