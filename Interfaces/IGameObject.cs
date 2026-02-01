using gameSnake.Core;

namespace gameSnake.Interfaces
{
    /// <summary>
    /// Базовый интерфейс для всех игровых объектов
    /// </summary>
    public interface IGameObject
    {
        /// <summary>
        /// Текущая позиция игрового объекта на поле
        /// </summary>
        Position Position { get; }

        /// <summary>
        /// Обновляет состояние игрового объекта
        /// </summary>
        void Update();
    }
}
