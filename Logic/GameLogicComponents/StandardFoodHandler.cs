using gameSnake.Models;
using gameSnake.Utils;

namespace gameSnake.Logic.GameLogicComponents
{
    /// <summary>
    /// Стандартный обработчик еды: проверка совпадения координат и спавн через FoodSpawner.
    /// </summary>
    public class StandardFoodHandler : IFoodHandler
    {
        /// <summary>
        /// Проверяет, съела ли змейка еду.
        /// </summary>
        public bool IsFoodEaten(Snake snake, Food food)
        {
            if (!food.IsSuccess || food.Position == null) return false;
            return snake.Head.Equals(food.Position.Value);
        }

        /// <summary>
        /// Создаёт новую еду на поле.
        /// </summary>
        public Food RespawnFood(PlayingField field, Snake snake)
            => FoodSpawner.CreateFood(field, snake);
    }
}
