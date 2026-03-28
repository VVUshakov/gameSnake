namespace Snake
{
    /// <summary>
    /// Интерфейс обработчика ввода.
    /// Определяет метод обработки пользовательского ввода.
    /// </summary>
    public interface IInputHandler
    {
        /// <summary>
        /// Обрабатывает пользовательский ввод и обновляет состояние игры.
        /// Управляет движением змейки, паузой и выходом из игры.
        /// </summary>
        /// <param name="state">Текущее состояние игры, которое будет обновлено</param>
        void ProcessInput(GameState state);
    }
}