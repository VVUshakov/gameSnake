using gameSnake.Core.State;
using gameSnake.Models;

namespace gameSnake.Interfaces
{
    /// <summary>
    /// Часть состояния игры, реагирующая на ввод.
    /// Позволяет изменять только флаги и направление.
    /// </summary>
    public interface IInputState
    {
        bool IsExit { get; set; }
        bool IsRestartRequested { get; set; }
        bool IsPaused { get; set; }
        Direction CurrentDirection { get; set; }
        GameMessage? ActiveMessage { get; set; }
    }
}
