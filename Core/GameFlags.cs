namespace gameSnake.Core
{
    /// <summary>
    /// Управляющие флаги состояния игры.
    /// </summary>
    public class GameFlags
    {
        /// <summary>
        /// Запрос на выход из игры
        /// </summary>
        public bool IsExit { get; set; }

        /// <summary>
        /// Игра окончена (проигрыш)
        /// </summary>
        public bool IsGameOver { get; set; }

        /// <summary>
        /// Победа (поле заполнено)
        /// </summary>
        public bool IsWin { get; set; }

        /// <summary>
        /// Пауза
        /// </summary>
        public bool IsPaused { get; set; }

        /// <summary>
        /// Запрос на перезапуск игры
        /// </summary>
        public bool IsRestartRequested { get; set; }
    }
}
