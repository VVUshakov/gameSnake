using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text.Json;
using gameSnake.Interfaces;
using gameSnake.Models;
using gameSnake.Server.DTO;

namespace gameSnake.Server.InputHandlers
{
    /// <summary>
    /// Обработчик ввода, получающий команды через WebSocket.
    /// Читает команды из очереди, заполняемой фоновым читателем.
    /// </summary>
    public class WebSocketInputHandler : IInputHandler, IDisposable
    {
        private readonly ConcurrentQueue<ClientCommand> _commandQueue = new();
        private readonly CancellationTokenSource _cts = new();

        public WebSocketInputHandler(WebSocket webSocket)
        {
            _ = Task.Run(() => ReadLoop(webSocket, _cts.Token));
        }

        public void ProcessInput(IInputState inputState, int snakeLength)
        {
            while (_commandQueue.TryDequeue(out var command))
            {
                ApplyCommand(command, inputState, snakeLength);
            }
        }

        public void Dispose() => _cts.Cancel();

        private void ApplyCommand(ClientCommand command, IInputState state, int snakeLength)
        {
            switch (command.Type.ToLower())
            {
                case "pause":
                    state.IsPaused = !state.IsPaused;
                    break;
                case "restart":
                    state.IsRestartRequested = true;
                    break;
                case "exit":
                    state.IsExit = true;
                    break;
                case "move" when command.Direction != null:
                    ApplyDirection(state, command.Direction.ToLower(), snakeLength);
                    break;
            }
        }

        private static void ApplyDirection(IInputState state, string dir, int snakeLength)
        {
            Direction newDir = dir switch
            {
                "up"    => Direction.Up,
                "down"  => Direction.Down,
                "left"  => Direction.Left,
                "right" => Direction.Right,
                _       => state.CurrentDirection
            };

            if (state.IsPaused) return;

            Direction opposite = dir switch
            {
                "up"    => Direction.Down,
                "down"  => Direction.Up,
                "left"  => Direction.Right,
                "right" => Direction.Left,
                _       => state.CurrentDirection
            };

            if (snakeLength <= 1 || state.CurrentDirection != opposite)
                state.CurrentDirection = newDir;
        }

        private async Task ReadLoop(WebSocket webSocket, CancellationToken ct)
        {
            var buffer = new byte[1024];
            while (!ct.IsCancellationRequested && webSocket.State == WebSocketState.Open)
            {
                try
                {
                    var result = await webSocket.ReceiveAsync(
                        new ArraySegment<byte>(buffer), ct);

                    if (result.MessageType == WebSocketMessageType.Close)
                        break;

                    if (result.MessageType == WebSocketMessageType.Text && result.Count > 0)
                    {
                        var json = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);
                        var command = JsonSerializer.Deserialize<ClientCommand>(json);
                        if (command != null)
                            _commandQueue.Enqueue(command);
                    }
                }
                catch (OperationCanceledException) { break; }
                catch (WebSocketException) { break; }
            }
        }
    }
}
