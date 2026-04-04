using gameSnake.Models;

namespace gameSnake.Core
{
    /// <summary>
    /// Состояние игры. Содержит все данные, необходимые для работы игры.
    /// Обязательные игровые объекты задаются через конструктор.
    /// </summary>
    public class GameState
    {
        // Управляющие флаги
        public bool IsExit { get; set; }
        public bool IsGameOver { get; set; }
        public bool IsWin { get; set; }
        public bool IsPaused { get; set; }
        public bool IsRestartRequested { get; set; }

        // Настройки
        public int Fps { get; set; } = 100;

        // Игровые объекты (обязательные — только через конструктор)
        public Header Header { get; }
        public Direction CurrentDirection { get; set; }
        public PlayingField Field { get; }
        public Snake Snake { get; }
        public Food Food { get; set; }

        public GameState(Header header, PlayingField field, Snake snake, Food food)
        {
            Header = header;
            Field = field;
            Snake = snake;
            Food = food;
            CurrentDirection = Direction.Right;
        }
    }
}
