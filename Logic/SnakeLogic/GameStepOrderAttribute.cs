namespace gameSnake.Logic.SnakeLogic
{
    /// <summary>
    /// Атрибут для указания порядка выполнения шага игровой логики.
    /// Чем меньше число, тем раньше выполняется шаг.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class GameStepOrderAttribute : Attribute
    {
        /// <summary>
        /// Порядок выполнения шага.
        /// </summary>
        public int Order { get; }

        public GameStepOrderAttribute(int order)
        {
            Order = order;
        }
    }
}
