namespace Snake
{
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

        public void Run(GameState state)
        {
            while(!state.IsExit)
            {
                _renderer.Clear();
                _renderer.Render(state);
                _inputHandler.ProcessInput(state);
                _gameLogic.Update(state);
                Thread.Sleep(state.Fps);
            }
        }

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

                    // Если вышли не по Escape (т.е. проиграли)
                    if(state.IsGameOver)
                    {
                        // Спрашиваем, хочет ли игрок сыграть ещё
                        Console.SetCursorPosition(0, state.Field.Height + 5);
                        Console.Write("Хотите сыграть ещё? (y/n): ");

                        ConsoleKeyInfo key = Console.ReadKey();
                        playAgain = (key.KeyChar == 'y' || key.KeyChar == 'Y');

                        if(playAgain)
                        {
                            // Очищаем экран перед новой игрой
                            Console.Clear();
                        }
                    }
                    else
                    {
                        // Если вышли по Escape - не спрашиваем
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
    }
}