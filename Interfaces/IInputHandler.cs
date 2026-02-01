using SnakeGame.Core;

namespace SnakeGame.Interfaces
{
    /// <summary>
    /// Интерфейс для обработки пользовательского ввода
    /// </summary>
    public interface IInputHandler
    {
        /// <summary>
        /// Обрабатывает все накопленные события ввода
        /// </summary>
        void ProcessInput();

        /// <summary>
        /// Получает текущее направление движения из обработанного ввода
        /// </summary>
        /// <returns>Направление движения или null, если направление не изменилось</returns>
        Direction? GetDirection();

        /// <summary>
        /// Проверяет, запрошен ли выход из игры
        /// </summary>
        /// <returns>true, если игрок нажал клавишу выхода</returns>
        bool ShouldExit();
    }
}