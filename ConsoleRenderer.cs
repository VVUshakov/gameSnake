namespace Snake
{
    /// <summary>
    /// Отрисовывает игру в консоли
    /// </summary>
    public class ConsoleRenderer : IGameRenderer
    {
        public void Clear()
        {
            Console.Clear();
        }

        public void Render(GameState state)
        {
            // Логика отрисовки кадра в консоли:

            // Нарисовать игровое поле
            // Нарисовать змейку
            // Нарисовать еду
        }
    }
}