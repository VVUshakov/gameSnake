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
        private readonly Func<GameState> _stateFactory;

        public GameLoop(
            IGameRenderer renderer,
            IInputHandler inputHandler,
            IGameLogic gameLogic,
            Func<GameState> stateFactory)
        {
            _renderer = renderer;
            _inputHandler = inputHandler;
            _gameLogic = gameLogic;
            _stateFactory = stateFactory;
        }

        /// <summary>
        /// Запускает игру с поддержкой перезапуска.
        /// </summary>
        public void Run()
        {
            while(true)
            {
                try
                {
                    GameState state = _stateFactory();
                    RunSingleGame(state);

                    if(state.IsExit) break;
                    if(!state.IsRestartRequested) break;
                }
                catch(InvalidOperationException ex)
                {
                    Clear();
                    WriteLine("ОШИБКА: " + ex.Message);
                    WriteLine("Нажмите любую клавишу для выхода...");
                    ReadKey();
                    break;
                }
            }
        }

        /// <summary>
        /// Запускает одну игровую сессию.
        /// </summary>
        private void RunSingleGame(GameState state)
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
