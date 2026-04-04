using gameSnake.Core;
using gameSnake.Interfaces;
using gameSnake.Servises;

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
            int headerHeight = state.Header.Height;
            HeaderRenderer.Draw(state.Header);
            FieldRenderer.Draw(state.Field, headerHeight);
            SnakeRenderer.Draw(state.Snake, state.Field, headerHeight);
            FoodRenderer.Draw(state.Food, state.Field, headerHeight);

            if (state.IsGameOver)
                MessageRenderer.Draw(state.Field, headerHeight, ServiseMessange.GetGameOverMessange(), RenderConstants.GameOverColor);
            if (state.IsWin)
                MessageRenderer.Draw(state.Field, headerHeight, ServiseMessange.GetGameWinMessange(), RenderConstants.GameWinColor);
            if (state.IsPaused)
                MessageRenderer.Draw(state.Field, headerHeight, ServiseMessange.GetPauseMessange(), RenderConstants.PauseColor);
        }
    }
}
