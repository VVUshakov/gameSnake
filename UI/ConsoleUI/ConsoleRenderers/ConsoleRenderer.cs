using gameSnake.Core;
using gameSnake.Interfaces;
using gameSnake.UI.ConsoleUI.ConsoleRenderers;

namespace gameSnake.UI.ConsoleUI.ConsoleRenderers
{
    /// <summary>
    /// Основной рендерер игры для консоли.
    /// Реализует интерфейс IGameRenderer и координирует отрисовку всех элементов игры:
    /// заголовка, поля, змейки, еды, сервисных сообщений и т.п.
    /// </summary>
    public class ConsoleRenderer : IGameRenderer
    {
        /// <summary>
        /// Очищает консоль перед отрисовкой нового кадра
        /// </summary>
        public void Clear() => Console.Clear();

        /// <summary>
        /// Отрисовывает текущее состояние игры.
        /// Последовательно вызывает рендереры: заголовок, поле, змейку, еду,
        /// а при необходимости — сообщения о паузе, победе или проигрыше.
        /// </summary>
        /// <param name="state">Текущее состояние игры со всеми игровыми объектами</param>
        public void Render(GameState state)
        {
            int headerHeight = state.Header.Height;
            HeaderRenderer.Draw(state.Header);
            FieldRenderer.Draw(state.Field, headerHeight);
            SnakeRenderer.Draw(state.Snake, state.Field, headerHeight);
            FoodRenderer.Draw(state.Food, state.Field, headerHeight);

            if (state.IsGameOver) MessageRenderer.DrawGameOver(state.Field, headerHeight);
            if (state.IsWin) MessageRenderer.DrawGameWin(state.Field, headerHeight);
            if (state.IsPaused) MessageRenderer.DrawPause(state.Field, headerHeight);
        }
    }
}
