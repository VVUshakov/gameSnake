using SnakeGame;

namespace gameSnake.Interfaces
{
    /// <summary>
    /// Интерфейс для создания игровых объектов
    /// </summary>
    public interface IGameObjectFactory
    {
        /// <summary>
        /// Создает новый объект еды
        /// </summary>
        /// <param name="width">Ширина игрового поля</param>
        /// <param name="height">Высота игрового поля</param>
        /// <returns>Новый объект еды</returns>
        Food CreateFood(int width, int height);

        /// <summary>
        /// Создает новый объект змейки
        /// </summary>
        /// <param name="initialLength">Начальная длина змейки</param>
        /// <returns>Новый объект змейки</returns>
        Snake CreateSnake(int initialLength);
    }
}
