using gameSnake.Core.State;

namespace gameSnake.Logic.SnakeLogic
{
    /// <summary>
    /// Шаг обновления игрового состояния.
    /// Каждый шаг выполняет одну часть игровой логики.
    /// </summary>
    public interface IGameStep
    {
        /// <summary>
        /// Применяет шаг к состоянию игры.
        /// </summary>
        /// <param name="state">Текущее состояние игры</param>
        /// <returns>True, если шаг прерывает дальнейшее выполнение (например, победа или поражение)</returns>
        bool Apply(GameState state);
    }
}
