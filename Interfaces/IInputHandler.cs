using gameSnake.Core;
using gameSnake.Models;

namespace gameSnake.Interfaces
{
    /// <summary>
    /// Интерфейс обработчика ввода.
    /// Определяет метод обработки пользовательского ввода.
    /// </summary>
    public interface IInputHandler
    {
        /// <summary>
        /// Обрабатывает пользовательский ввод и обновляет состояние игры.
        /// </summary>
        void ProcessInput(GameState state);
    }
}