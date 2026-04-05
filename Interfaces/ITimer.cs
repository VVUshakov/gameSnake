namespace gameSnake.Interfaces
{
    /// <summary>
    /// Абстракция задержки для игрового цикла.
    /// Позволяет заменить механизм тайминга (Thread.Sleep, высокоточный таймер и т.д.).
    /// </summary>
    public interface ITimer
    {
        /// <summary>
        /// Выполняет синхронную задержку на указанное количество миллисекунд.
        /// </summary>
        /// <param name="milliseconds">Длительность задержки</param>
        void Sleep(int milliseconds);
    }
}
