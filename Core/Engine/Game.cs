using gameSnake.Core.State;
using gameSnake.Interfaces;
using ITimer = gameSnake.Interfaces.ITimer;

namespace gameSnake.Core.Engine
{
    /// <summary>
    /// Запускает игровой цикл для переданного состояния.
    /// Не создаёт состояние сам — это задача фабрики (GameStateFactory).
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
        public Game(IGameRenderer renderer, IInputHandler inputHandler, IGameLogic gameLogic, ITimer timer)
        {
            _renderer = renderer;
            _inputHandler = inputHandler;
            _gameLogic = gameLogic;
            _timer = timer;
        }

        /// <summary>
        /// Запускает игровой цикл для переданного состояния.
        /// Возвращает управление, когда игра окончена или запрошен перезапуск.
        /// </summary>
        /// <param name="state">Состояние игры</param>
        public void Run(GameState state)
        {
            var gameLoop = new GameLoop(_renderer, _inputHandler, _gameLogic, _timer);
            gameLoop.Run(state);
        }
    }
}
