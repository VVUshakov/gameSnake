namespace Snake
{
    public class Program
    {
        static void Main()
        {
            IGameRenderer renderer = new ConsoleRenderer();
            IInputHandler input = new ConsoleInputHandler();
            IGameLogic logic = new SnakeGameLogic();

            GameLoop gameLoop = new GameLoop(renderer, input, logic);
            gameLoop.RunWithRestart();
        }
    }
}