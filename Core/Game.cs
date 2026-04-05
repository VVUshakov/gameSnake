using gameSnake.Core.State;
using gameSnake.Interfaces;
using ITimer = gameSnake.Interfaces.ITimer;
using gameSnake.Servises;
using gameSnake.Utils;

namespace gameSnake.Core
{
    /// <summary>
    /// Управляет жизненным циклом игры: инициализация, запуск, перезапуск.
    /// </summary>
    public class Game
    {
        private readonly IGameRenderer _renderer;
        private readonly IInputHandler _inputHandler;
        private readonly IGameLogic _gameLogic;
        private readonly ITimer _timer;

        /// <summary>
        /// Создаёт экземпляр игры с указанными зависимостями.
        /// </summary>
        /// <param name="renderer">Рендерер для отрисовки игры</param>
        /// <param name="inputHandler">Обработчик пользовательского ввода</param>
        /// <param name="gameLogic">Логика игры</param>
        /// <param name="timer">Таймер для управления скоростью игры</param>
        public Game(IGameRenderer renderer, IInputHandler inputHandler, IGameLogic gameLogic, ITimer timer)
        {
            _renderer = renderer;
            _inputHandler = inputHandler;
            _gameLogic = gameLogic;
            _timer = timer;
        }

        /// <summary>
        /// Запускает игру с циклом перезапуска.
        /// </summary>
        public void Run()
        {
            while (true)
            {
                GameState state = CreateGameState();
                var gameLoop = new GameLoop(_renderer, _inputHandler, _gameLogic, _timer);
                gameLoop.Run(state);

                if (state.Flags.IsExit || !state.Flags.IsRestartRequested) break;
            }
        }

        /// <summary>
        /// Создаёт начальное состояние игры: вычисляет размеры поля,
        /// настраивает консоль и инициализирует игровые объекты.
        /// </summary>
        /// <returns>Готовое начальное состояние игры</returns>
        private static GameState CreateGameState()
        {
            var messages = MessageRegistry.GetAll();
            var (maxMsgWidth, maxMsgHeight) = MessageSizer.GetMaxSize(messages);
            (int fieldWidth, int fieldHeight) = FieldSizeCalculator.Calculate(maxMsgWidth, maxMsgHeight);
            ConsoleWindowConfigurator.Configure(fieldWidth, fieldHeight);
            return GameStateFactory.Create(fieldWidth, fieldHeight);
        }
    }
}
