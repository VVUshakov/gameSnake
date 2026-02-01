using SnakeGame.Core;
using SnakeGame.Interfaces;

namespace SnakeGame.GameObjects
{
    /// <summary>
    /// Класс, представляющий еду на игровом поле
    /// </summary>
    public class Food : IGameObject
    {
        private static readonly Random _random = new Random();

        /// <summary>
        /// Текущая позиция еды на поле
        /// </summary>
        public Position Position { get; private set; }

        /// <summary>
        /// Создает новую еду в случайной позиции
        /// </summary>
        /// <param name="width">Ширина игрового поля</param>
        /// <param name="height">Высота игрового поля</param>
        public Food(int width, int height)
        {
            Respawn(width, height);
        }

        /// <summary>
        /// Перемещает еду в новую случайную позицию
        /// </summary>
        /// <param name="width">Ширина игрового поля</param>
        /// <param name="height">Высота игрового поля</param>
        public void Respawn(int width, int height)
        {
            Position = new Position(
                _random.Next(0, width),
                _random.Next(0, height)
            );
        }

        /// <summary>
        /// Обновляет состояние еды (реализация IGameObject)
        /// </summary>
        public void Update()
        {
            // Еда не требует обновления, так как статична
        }
    }
}