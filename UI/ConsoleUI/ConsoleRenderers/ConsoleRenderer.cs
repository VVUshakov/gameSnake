using GameState = gameSnake.Core.State.GameState;
using gameSnake.Interfaces;
using gameSnake.UI.ConsoleUI.ConsoleRenderers.Renderers;

namespace gameSnake.UI.ConsoleUI.ConsoleRenderers
{
    /// <summary>
    /// Основной рендерер игры для консоли.
    /// Реализует интерфейс IGameRenderer и координирует отрисовку всех элементов игры.
    /// </summary>
    public class ConsoleRenderer : IGameRenderer
    {
        /// <summary>
        /// Очищает консоль перед отрисовкой нового кадра.
        /// </summary>
        public void Clear() => Console.Clear();

        /// <summary>
        /// Отрисовывает текущее состояние игры.
        /// Последовательно вызывает рендереры: заголовок, поле, змейку, еду,
        /// а при необходимости — активное сервисное сообщение.
        /// </summary>
        /// <param name="state">Текущее состояние игры со всеми игровыми объектами</param>
        public void Render(GameState state)
        {
            int headerHeight = HeaderFormatter.GetHeight(state.Header);
            HeaderRenderer.Draw(state.Header);
            FieldRenderer.Draw(state.Field, headerHeight);
            SnakeRenderer.Draw(state.Snake, state.Field, headerHeight);
            FoodRenderer.Draw(state.Food, state.Field, headerHeight);

            if (state.ActiveMessage.HasValue)
                MessageRenderer.Draw(state.Field, headerHeight, state.ActiveMessage.Value);
        }
    }
}
