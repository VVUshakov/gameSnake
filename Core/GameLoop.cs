using Snake.Interfaces;
using Snake.Models;
using static System.Console;

namespace Snake.Core
{
    /// <summary>
    /// Игровой цикл. Управляет отрисовкой, вводом и обновлением состояния игры.
    /// </summary>
    public class GameLoop
    {
        private readonly IGameRenderer _renderer;
        private readonly IInputHandler _inputHandler;
        private readonly IGameLogic _gameLogic;

        public GameLoop(IGameRenderer renderer, IInputHandler inputHandler, IGameLogic gameLogic)
        {
            _renderer = renderer;
            _inputHandler = inputHandler;
            _gameLogic = gameLogic;
        }

        /// <summary>
        /// Запускает игровой цикл для одного состояния игры.
        /// Возвращает управление, когда игра окончена или запрошен перезапуск.
        /// </summary>
        public void Run(GameState state)
        {
            while(!state.IsExit && !state.IsRestartRequested)
            {
                _renderer.Clear();
                _renderer.Render(state);
                Update(state);
                Thread.Sleep(state.Fps);
            }
        }

        private void Update(GameState state)
        {
            _inputHandler.ProcessInput(state);
            if(!state.IsPaused) _gameLogic.Update(state);
        }
    }
}
