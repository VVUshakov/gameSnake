using Snake.Models;
using SnakeType = Snake.Models.Snake;

namespace Snake.Core
{
    /// <summary>
    /// Состояние игры. Содержит все данные, необходимые для работы игры.
    /// Это чистый контейнер данных — инициализация через GameFactory.
    /// </summary>
    public class GameState
    {
        // Управляющие флаги
        public bool IsExit { get; set; } = false;
        public bool IsGameOver { get; set; } = false;
        public bool IsWin { get; set; } = false;
        public bool IsPaused { get; set; } = false;
        public bool IsRestartRequested { get; set; } = false;

        // Настройки
        public int Fps { get; set; } = 100;

        // Игровые данные
        public Header Header { get; } = new Header();
        public Direction CurrentDirection { get; set; } = Direction.Right;

        // Компоненты игры (инициализируются через GameFactory)
        public PlayingField Field { get; set; } = null!;
        public SnakeType Snake { get; set; } = null!;
        public Food Food { get; set; } = null!;
    }
}
