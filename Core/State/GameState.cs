using gameSnake.Interfaces;
using gameSnake.Models;

namespace gameSnake.Core.State
{
    /// <summary>
    /// Состояние игры. Содержит флаги, настройки и игровые объекты.
    /// Является центральным хранилищем данных, которые используются
    /// игровой логикой, рендерером и обработчиком ввода.
    /// </summary>
    /// <remarks>
    /// Обязательные игровые объекты задаются через конструктор.
    /// Необязательные (например, <see cref="ActiveMessage"/>) могут изменяться во время игры.
    /// </remarks>
    public class GameState : IInputState
    {
        /// <summary>
        /// Управляющие флаги игры (выход, перезапуск, пауза, конец игры, победа).
        /// </summary>
        public GameFlags Flags { get; } = new GameFlags();

        /// <summary>
        /// Настройки игры (FPS и другие параметры).
        /// </summary>
        public GameSettings Settings { get; } = new GameSettings();

        /// <summary>
        /// Заголовок игры — отображает счёт, уровень, жизни и т.д.
        /// </summary>
        public Header Header { get; }

        /// <summary>
        /// Текущее направление движения змейки.
        /// </summary>
        public Direction CurrentDirection { get; set; }

        /// <summary>
        /// Игровое поле с заданными размерами и границами.
        /// </summary>
        public PlayingField Field { get; }

        /// <summary>
        /// Змейка — основной игровой объект.
        /// </summary>
        public Snake Snake { get; }

        /// <summary>
        /// Еда на игровом поле. Может быть пересоздана в процессе игры.
        /// </summary>
        public Food Food { get; set; }

        /// <summary>
        /// Активное сервисное сообщение для отображения поверх игрового поля.
        /// Задаётся игровой логикой, рендерер только отображает.
        /// </summary>
        /// <seealso cref="GameMessageType"/>
        public GameMessageType? ActiveMessage { get; set; }

        /// <summary>
        /// Реализация <see cref="IInputState.IsExit"/> — делегирование к <see cref="Flags"/>.
        /// </summary>
        bool IInputState.IsExit
        {
            get => Flags.IsExit;
            set => Flags.IsExit = value;
        }

        /// <summary>
        /// Реализация <see cref="IInputState.IsRestartRequested"/> — делегирование к <see cref="Flags"/>.
        /// </summary>
        bool IInputState.IsRestartRequested
        {
            get => Flags.IsRestartRequested;
            set => Flags.IsRestartRequested = value;
        }

        /// <summary>
        /// Реализация <see cref="IInputState.IsPaused"/> — делегирование к <see cref="Flags"/>.
        /// </summary>
        bool IInputState.IsPaused
        {
            get => Flags.IsPaused;
            set => Flags.IsPaused = value;
        }

        /// <summary>
        /// Создаёт состояние игры с обязательными игровыми объектами.
        /// </summary>
        /// <param name="header">Заголовок игры (счёт, уровень, жизни)</param>
        /// <param name="field">Игровое поле</param>
        /// <param name="snake">Змейка</param>
        /// <param name="food">Еда</param>
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
