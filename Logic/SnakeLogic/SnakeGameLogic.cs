using System.Reflection;
using gameSnake.Core.State;
using gameSnake.Interfaces;

namespace gameSnake.Logic.SnakeLogic
{
    /// <summary>
    /// Основная логика игры: обновление состояния змейки.
    /// Автоматически обнаруживает шаги обновления (IUpdateStep) через рефлексию и сортирует их по атрибуту [GameStepOrder].
    /// </summary>
    public class SnakeGameLogic : IGameLogic
    {
        private readonly IUpdateStep[] _steps;

        /// <summary>
        /// Создаёт логику игры и автоматически обнаруживает все шаги в текущей сборке.
        /// Шаги сортируются по атрибуту [GameStepOrder] (от меньшего к большему).
        /// </summary>
        public SnakeGameLogic()
        {
            _steps = DiscoverSteps();
        }

        /// <summary>
        /// Обновляет состояние игры, последовательно применяя все обнаруженные шаги.
        /// </summary>
        /// <param name="state">Текущее состояние игры</param>
        public void Update(GameState state)
        {
            if (state.Flags.IsGameOver || state.Flags.IsWin || state.Flags.IsPaused) return;

            foreach (var step in _steps)
            {
                if (step.Apply(state)) return;
            }
        }

        /// <summary>
        /// Автоматически обнаруживает все классы, реализующие IUpdateStep, в текущей сборке.
        /// Сортирует их по атрибуту [GameStepOrder].
        /// </summary>
        /// <returns>Отсортированный массив шагов</returns>
        private static IUpdateStep[] DiscoverSteps()
        {
            var steps = new List<(int Order, IUpdateStep Step)>();

            // Находим все типы в текущей сборке
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                // Проверяем, что тип реализует IUpdateStep и имеет атрибут GameStepOrder
                if (typeof(IUpdateStep).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
                {
                    var attr = type.GetCustomAttribute<GameStepOrderAttribute>();
                    if (attr != null)
                    {
                        var step = (IUpdateStep)Activator.CreateInstance(type)!;
                        steps.Add((attr.Order, step));
                    }
                }
            }

            // Сортируем по порядку и возвращаем
            return steps.OrderBy(x => x.Order).Select(x => x.Step).ToArray();
        }
    }
}
