using gameSnake.Models;
using gameSnake.Utils;

namespace gameSnake.Logic.GameLogicComponents
{
    /// <summary>
    /// Обработчик еды: проверка совпадения координат и спавн через FoodSpawner.
    /// </summary>
    public static class FoodHandler
    {
        /// <summary>
        /// Проверяет, съела ли змейка еду.
        /// </summary>
        /// <param name="snake">Змейка</param>
        /// <param name="food">Еда</param>
        /// <returns>True, если еда съедена</returns>
        public static bool IsFoodEaten(Snake snake, Food food)
        {
            if (!food.IsSuccess || food.Position == null) return false;
            return snake.Head.Equals(food.Position.Value);
        }

        /// <summary>
        /// Создаёт новую еду на поле.
        /// </summary>
        /// <param name="field">Игровое поле</param>
        /// <param name="snake">Змейка (для исключения её позиции)</param>
        /// <returns>Новый объект еды</returns>
        public static Food RespawnFood(PlayingField field, Snake snake)
            => FoodSpawner.CreateFood(field, snake);
    }
}
