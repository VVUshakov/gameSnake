using gameSnake.Models;

namespace gameSnake.Core
{
    /// <summary>
    /// Состояние игры. Содержит флаги, настройки и игровые объекты.
    /// Обязательные игровые объекты задаются через конструктор.
    /// </summary>
    public class GameState
    {
        /// <summary>
        /// Управляющие флаги
        /// </summary>
        public GameFlags Flags { get; } = new GameFlags();

        /// <summary>
        /// Настройки игры
        /// </summary>
        public GameSettings Settings { get; } = new GameSettings();

        /// <summary>
        /// Игровые объекты (обязательные — только через конструктор)
        /// </summary>
        public Header Header { get; }
        public Direction CurrentDirection { get; set; }
        public PlayingField Field { get; }
        public Snake Snake { get; }
        public Food Food { get; set; }

        /// <summary>
        /// Активное сервисное сообщение для отображения поверх игрового поля.
        /// Задаётся игровой логикой, рендерер только отображает.
        /// </summary>
        public GameMessage? ActiveMessage { get; set; }

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
