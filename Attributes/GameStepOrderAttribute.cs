namespace gameSnake.Attributes
{
    /// <summary>
    /// Атрибут для указания порядка выполнения шага игровой логики.
    /// Чем меньше число, тем раньше выполняется шаг.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class GameStepOrderAttribute : Attribute
    {
        /// <summary>
        /// Получает порядок выполнения шага.
        /// </summary>
        public int Order { get; }

        /// <summary>
        /// Создаёт атрибут с указанным порядком выполнения.
        /// </summary>
        /// <param name="order">Порядок выполнения шага (меньше — раньше)</param>
        public GameStepOrderAttribute(int order)
        {
            Order = order;
        }
    }
}
