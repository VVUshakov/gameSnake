namespace gameSnake.Interfaces
{
    /// <summary>
    /// Обрабатывает пользовательский ввод.
    /// </summary>
    public interface IInputHandler
    {
        /// <summary>
        /// Обрабатывает ввод и применяет команды.
        /// </summary>
        /// <param name="inputState">Часть состояния, реагирующая на ввод</param>
        /// <param name="snakeLength">Длина змейки (для проверки разворота на 180)</param>
        void ProcessInput(IInputState inputState, int snakeLength);
    }
}
