using gameSnake.Interfaces;
using gameSnake.Models;

namespace gameSnake.Core.State
{
    /// <summary>
    /// Состояние игры. Содержит флаги, настройки и игровые объекты.
    /// Обязательные игровые объекты задаются через конструктор.
    /// </summary>
    public class GameState : IInputState
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

        // Реализация IInputState — делегирование к Flags
        bool IInputState.IsExit
        {
            get => Flags.IsExit;
            set => Flags.IsExit = value;
        }

        bool IInputState.IsRestartRequested
        {
            get => Flags.IsRestartRequested;
            set => Flags.IsRestartRequested = value;
        }

        bool IInputState.IsPaused
        {
            get => Flags.IsPaused;
            set => Flags.IsPaused = value;
        }

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
