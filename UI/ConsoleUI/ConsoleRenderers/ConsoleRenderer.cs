using gameSnake.Core;
using gameSnake.Interfaces;

namespace gameSnake.UI.ConsoleUI.ConsoleRenderers
{
    /// <summary>
    /// Основной рендерер игры для консоли.
    /// Реализует интерфейс IGameRenderer и координирует отрисовку всех элементов игры.
    /// </summary>
    public class ConsoleRenderer : IGameRenderer
    {
        public void Clear() => Console.Clear();

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
