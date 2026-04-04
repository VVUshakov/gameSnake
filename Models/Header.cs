using gameSnake.Attributes;

namespace gameSnake.Models
{
    /// <summary>
    /// Служебная информация (заголовок), отображаемая над игровым полем.
    /// Содержит счёт, уровень, жизни и другие игровые параметры.
    /// </summary>
    public class Header
    {
        /// <summary>
        /// Текущий счёт игрока
        /// </summary>
        [HeaderInfo]
        public int Score { get; set; } = 0;

        /// <summary>
        /// Текущий уровень
        /// </summary>
        [HeaderInfo]
        public int Level { get; set; } = 1;

        /// <summary>
        /// Количество жизней
        /// </summary>
        [HeaderInfo]
        public int Lives { get; set; } = 1;
    }
}
