using gameSnake.Models;

namespace gameSnake.Logic.GameLogicComponents
{
    /// <summary>
    /// Обработчик логики еды: проверка поедания и создание новой еды.
    /// </summary>
    public interface IFoodHandler
    {
        /// <summary>
        /// Проверяет, съела ли змейка еду.
        /// </summary>
        /// <param name="snake">Змейка</param>
        /// <param name="food">Еда</param>
        /// <returns>True, если еда съедена</returns>
        bool IsFoodEaten(Snake snake, Food food);

        /// <summary>
        /// Создаёт новую еду на поле.
        /// </summary>
        /// <param name="field">Игровое поле</param>
        /// <param name="snake">Змейка (для исключения её позиции)</param>
        /// <returns>Новый объект еды</returns>
        Food RespawnFood(PlayingField field, Snake snake);
    }
}
