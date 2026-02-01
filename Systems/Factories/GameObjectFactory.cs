using SnakeGame.Core;
using SnakeGame.GameObjects;
using SnakeGame.Interfaces;

namespace SnakeGame.Systems.Factories
{
    /// <summary>
    /// Фабрика для создания игровых объектов
    /// </summary>
    public class GameObjectFactory : IGameObjectFactory
    {
        /// <summary>
        /// Создает новый объект еды
        /// </summary>
        /// <param name="width">Ширина игрового поля</param>
        /// <param name="height">Высота игрового поля</param>
        /// <returns>Новый объект еды</returns>
        public Food CreateFood(int width, int height)
        {
            return new Food(width, height);
        }

        /// <summary>
        /// Создает новый объект змейки
        /// </summary>
        /// <param name="initialLength">Начальная длина змейки</param>
        /// <returns>Новый объект змейки</returns>
        public Snake CreateSnake(int initialLength)
        {
            var settings = new GameSettings();
            return new Snake(settings.InitialSnakePosition, initialLength);
        }
    }
}