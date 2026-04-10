using gameSnake.Interfaces;
using ITimer = gameSnake.Interfaces.ITimer;

namespace gameSnake.Core.Engine
{
    /// <summary>
    /// Игровой цикл. Управляет отрисовкой, вводом и обновлением состояния игры.
    /// </summary>
    public class GameLoop
    {
        private readonly IGameRenderer _renderer;
        private readonly IInputHandler _inputHandler;
        private readonly IGameLogic _gameLogic;
        private readonly ITimer _timer;

        /// <summary>
        /// Создаёт экземпляр игрового цикла с указанными зависимостями.
        /// </summary>
        /// <param name="renderer">Рендерер для отрисовки кадров</param>
        /// <param name="inputHandler">Обработчик пользовательского ввода</param>
        /// <param name="gameLogic">Логика игры для обновления состояния</param>
        /// <param name="timer">Таймер для контроля частоты кадров</param>
        public GameLoop(IGameRenderer renderer, IInputHandler inputHandler, IGameLogic gameLogic, ITimer timer)
        {
            _renderer = renderer;
            _inputHandler = inputHandler;
            _gameLogic = gameLogic;
            _timer = timer;
        }

        /// <summary>
        /// Запускает игровой цикл для одного состояния игры.
        /// Возвращает управление, когда игра окончена или запрошен перезапуск.
        /// </summary>
        /// <param name="state">Состояние игры для данного цикла</param>
        public void Run(State.GameState state)
        {
            while (!state.Flags.IsExit && !state.Flags.IsRestartRequested)
            {
                _renderer.Clear();
                _renderer.Render(state);
                Update(state);
                _timer.Sleep(state.Settings.Fps);
            }
        }

        /// <summary>
        /// Выполняет один шаг обновления: обрабатывает ввод и обновляет игровую логику.
        /// </summary>
        /// <param name="state">Текущее состояние игры</param>
        private void Update(State.GameState state)
        {
            _inputHandler.ProcessInput(state, state.Snake.Body.Count);
            if (!state.Flags.IsPaused) _gameLogic.Update(state);
        }
    }
}

