using System.Net.WebSockets;
using gameSnake.Core.Engine;
using gameSnake.Core.Factories;
using gameSnake.Core.State;
using gameSnake.Interfaces;
using gameSnake.Logic.SnakeLogic;
using gameSnake.Server.InputHandlers;
using gameSnake.Server.Renderers;
using gameSnake.Servises.MessageServise;
using gameSnake.Utils;

namespace gameSnake.Server.Session
{
    /// <summary>
    /// Управляет одной игровой сессией WebSocket-подключения.
    /// Запускает Game для каждого раунда и перезапускает при запросе клиента.
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
        private WebSocketInputHandler? _inputHandler;

        public WebSocketGameSession(WebSocket webSocket)
        {
            _webSocket = webSocket;
        }

        /// <summary>
        /// Запускает игровые циклы в фоновом потоке.
        /// При перезапуске создаёт новое состояние и запускает заново.
        /// </summary>
        public void Start()
        {
            _disconnectToken = new CancellationTokenSource();
            _gameTask = Task.Run(() => RunSession(_disconnectToken.Token), _disconnectToken.Token);
        }

        private void RunSession(CancellationToken ct)
        {
            try
            {
                _inputHandler = new WebSocketInputHandler(_webSocket, ct);

                while (!ct.IsCancellationRequested)
                {
                    var state = CreateGameState();
                    var game = CreateGame(_inputHandler);
                    game.Run(state);

                    if (state.Flags.IsExit || !state.Flags.IsRestartRequested)
                        break;
                }
            }
            finally
            {
                _inputHandler?.Dispose();
            }
        }

        private static GameState CreateGameState()
        {
            var messages = MessageRegistry.GetAll();
            var (maxMsgWidth, maxMsgHeight) = MessageSizer.GetMaxSize(messages);
            (int fieldWidth, int fieldHeight) = FieldSizeCalculator.Calculate(maxMsgWidth, maxMsgHeight);

            return GameStateFactory.Create(fieldWidth, fieldHeight);
        }

        private Game CreateGame(WebSocketInputHandler inputHandler)
        {
            var renderer = new WebSocketRenderer(_webSocket);
            return new Game(renderer, inputHandler, new SnakeGameLogic(), new Core.SystemTimer());
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
