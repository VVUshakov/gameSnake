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

        /// <summary>
        /// Инициализирует новый экземпляр игрового цикла
        /// </summary>
        /// <param name="renderer">Рендерер для отрисовки игры</param>
        /// <param name="inputHandler">Обработчик ввода</param>
        /// <param name="gameLogic">Игровая логика</param>
        public GameLoop(IGameRenderer renderer, IInputHandler inputHandler, IGameLogic gameLogic)
        {
            _renderer = renderer;
            _inputHandler = inputHandler;
            _gameLogic = gameLogic;
        }

        /// <summary>
        /// Запускает игровой цикл с возможностью повторной игры
        /// </summary>
        public void RunWithRestart()
        {
            bool playAgain = true;

            while(playAgain)
            {
                try
                {
                    // Создаём новую игру
                    GameState state = new GameState();

                    // Запускаем игровой цикл
                    Run(state);

                    // Если игра проиграна — показываем сообщение и спрашиваем о повторной игре
                    if(state.IsGameOver)
                    {
                        _renderer.Render(state);  // Рисуем последний кадр с сообщением
                        
                        // Небольшая пауза, чтобы игрок успел прочитать сообщение
                        Thread.Sleep(500);
                        
                        playAgain = _inputHandler.AskPlayAgain();
                    }
                    else if(state.IsExit)
                    {
                        // Если вышли по Escape — не спрашиваем
                        playAgain = false;
                    }
                }
                catch(InvalidOperationException ex)
                {
                    // Если не удалось создать игру (нет места для еды)
                    Console.Clear();
                    Console.WriteLine("ОШИБКА: " + ex.Message);
                    Console.WriteLine("Нажмите любую клавишу для выхода...");
                    Console.ReadKey();
                    playAgain = false;
                }
            }
        }

        /// <summary>
        /// Запускает одиночный игровой цикл
        /// </summary>
        /// <param name="state">Начальное состояние игры</param>
        public void Run(GameState state)
        {
            while(!state.IsExit)
            {
                _renderer.Clear();
                _renderer.Render(state);
                Update(state);
                Thread.Sleep(state.Fps);
            }
        }

        /// <summary>
        /// Обновляет состояние игры: обрабатывает ввод и обновляет игровую логику
        /// </summary>
        /// <param name="state">Текущее состояние игры</param>
        private void Update(GameState state)
        {
            // обрабатываем ввод
            _inputHandler.ProcessInput(state);

            // Обновляем игру только если не на паузе
            if(!state.IsPaused) { _gameLogic.Update(state); }
        }
    }
}