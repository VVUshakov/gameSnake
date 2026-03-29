using Snake.Core;
using Snake.Models;

namespace Snake.Interfaces
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

        /// <summary>
        /// Запрашивает у пользователя повторную игру после окончания
        /// </summary>
        /// <returns>true, если пользователь хочет сыграть ещё, false в противном случае</returns>
        bool AskPlayAgain();
    }
}