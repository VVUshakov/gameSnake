namespace gameSnake.Core
{
    /// <summary>
    /// Стандартный таймер на основе Thread.Sleep.
    /// </summary>
    public class SystemTimer : Interfaces.ITimer
    {
        public void Sleep(int milliseconds) => Thread.Sleep(milliseconds);
    }
}
