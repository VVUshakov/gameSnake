using System.Net.WebSockets;
using gameSnake.Core.Engine;
using gameSnake.Core.Factories;
using gameSnake.Interfaces;
using gameSnake.Logic.SnakeLogic;
using gameSnake.Server.InputHandlers;
using gameSnake.Server.Renderers;

namespace gameSnake.Server.Session
{
    /// <summary>
    /// Управляет одной игровой сессией WebSocket-подключения.
    /// Запускает Game в фоновом потоке и корректно завершает при отключении.
    /// </summary>
    public class WebSocketGameSession : IDisposable
    {
        /// <summary>
        /// Уникальный идентификатор сессии.
        /// </summary>
        public Guid Id { get; } = Guid.NewGuid();

        private readonly WebSocket _webSocket;
        private CancellationTokenSource? _disconnectToken;
        private Task? _gameTask;

        public WebSocketGameSession(WebSocket webSocket)
        {
            _webSocket = webSocket;
        }

        /// <summary>
        /// Запускает игровой цикл в фоновом потоке.
        /// </summary>
        public void Start()
        {
            _disconnectToken = new CancellationTokenSource();
            _gameTask = Task.Run(() => RunGame(_disconnectToken.Token), _disconnectToken.Token);
        }

        private void RunGame(CancellationToken ct)
        {
            try
            {
                var renderer = new WebSocketRenderer(_webSocket);
                var inputHandler = new WebSocketInputHandler(_webSocket, ct);
                var windowConfigurator = new NoOpWindowConfigurator();

                var game = new Game(renderer, inputHandler, new SnakeGameLogic(), new Core.SystemTimer(), windowConfigurator);
                game.Run();
            }
            catch (WebSocketException) { }
            catch (OperationCanceledException) { }
        }

        /// <summary>
        /// Корректно останавливает сессию и дожидается завершения фонового потока.
        /// </summary>
        public void Dispose()
        {
            _disconnectToken?.Cancel();
            try
            {
                _gameTask?.Wait(TimeSpan.FromSeconds(3));
            }
            catch (AggregateException) { }
        }
    }
}
