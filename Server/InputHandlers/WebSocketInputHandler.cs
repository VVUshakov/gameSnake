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
    /// При отключении клиента помечает выход из игры.
    /// </summary>
    public class WebSocketInputHandler : IInputHandler, IDisposable
    {
        private readonly ConcurrentQueue<ClientCommand> _commandQueue = new();
        private readonly CancellationToken _cancellationToken;
        private readonly WebSocket _webSocket;
        private readonly Task _readTask;

        public WebSocketInputHandler(WebSocket webSocket, CancellationToken cancellationToken)
        {
            _webSocket = webSocket;
            _cancellationToken = cancellationToken;
            _readTask = Task.Run(() => ReadLoop(cancellationToken));
        }

        public void ProcessInput(IInputState inputState, int snakeLength)
        {
            if (_cancellationToken.IsCancellationRequested)
                inputState.IsExit = true;

            while (_commandQueue.TryDequeue(out var command))
            {
                ApplyCommand(command, inputState, snakeLength);
            }
        }

        public void Dispose()
        {
            try
            {
                _readTask.Wait(TimeSpan.FromSeconds(2));
            }
            catch (AggregateException) { }
        }

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

        private async Task ReadLoop(CancellationToken ct)
        {
            var buffer = new byte[1024];
            while (!ct.IsCancellationRequested && _webSocket.State == WebSocketState.Open)
            {
                try
                {
                    var result = await _webSocket.ReceiveAsync(
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
