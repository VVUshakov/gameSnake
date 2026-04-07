using System.Net.WebSockets;
using gameSnake.Core.Factories;
using gameSnake.Core.Engine;
using gameSnake.Core.State;
using gameSnake.Interfaces;
using gameSnake.Logic.SnakeLogic;
using gameSnake.Server.InputHandlers;
using gameSnake.Server.Renderers;

namespace gameSnake.Server.Session
{
    /// <summary>
    /// Управляет одной игровой сессией для WebSocket-подключения.
    /// Создаёт зависимости, запускает Game и корректно завершает сессию.
    /// </summary>
    public class WebSocketGameSession : IDisposable
    {
        private readonly WebSocket _webSocket;
        private WebSocketInputHandler? _inputHandler;
        private CancellationTokenSource? _disconnectToken;
        private Task? _gameTask;

        public WebSocketGameSession(WebSocket webSocket)
        {
            _webSocket = webSocket;
        }

        /// <summary>
        /// Запускает игровой цикл для данного WebSocket-подключения.
        /// </summary>
        public void Start()
        {
            _inputHandler = new WebSocketInputHandler(_webSocket);
            _disconnectToken = new CancellationTokenSource();

            _gameTask = Task.Run(() => RunGame(_disconnectToken.Token), _disconnectToken.Token);
        }

        private void RunGame(CancellationToken ct)
        {
            try
            {
                var messages = Servises.MessageServise.MessageRegistry.GetAll();
                var (maxMsgWidth, maxMsgHeight) = Utils.MessageSizer.GetMaxSize(messages);
                (int fieldWidth, int fieldHeight) = Utils.FieldSizeCalculator.Calculate(maxMsgWidth, maxMsgHeight);

                var renderer = new WebSocketRenderer(_webSocket);
                IWindowConfigurator windowConfigurator = new NoOpWindowConfigurator();
                windowConfigurator.Configure(fieldWidth, fieldHeight);

                GameState state = GameStateFactory.Create(fieldWidth, fieldHeight);
                var gameLoop = new GameLoop(renderer, _inputHandler!, new SnakeGameLogic(), new Core.SystemTimer());

                while (!ct.IsCancellationRequested
                       && !state.Flags.IsExit
                       && !state.Flags.IsRestartRequested)
                {
                    renderer.Clear();
                    renderer.Render(state);

                    _inputHandler!.ProcessInput(state, state.Snake.Body.Count);
                    if (!state.Flags.IsPaused)
                        gameLoop.Update(state);

                    Thread.Sleep(state.Settings.Fps);
                }
            }
            catch (WebSocketException) { }
            catch (OperationCanceledException) { }
        }

        public void Dispose()
        {
            _disconnectToken?.Cancel();
            _inputHandler?.Dispose();
            _gameTask?.Wait(TimeSpan.FromSeconds(2));
        }
    }
}
