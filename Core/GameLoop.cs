using gameSnake.Interfaces;
using gameSnake.Models;
using static System.Console;

namespace gameSnake.Core
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
        public void Run(State.GameState state)
        {
            while (!state.Flags.IsExit && !state.Flags.IsRestartRequested)
            {
                _renderer.Clear();
                _renderer.Render(state);
                Update(state);
                Thread.Sleep(state.Settings.Fps);
            }
        }

        private void Update(State.GameState state)
        {
            _inputHandler.ProcessInput(state, state.Snake.Body.Count);
            if (!state.Flags.IsPaused) _gameLogic.Update(state);
        }
    }
}

