namespace gameSnake.Attributes
{
    /// <summary>
    /// Атрибут-маркер для обозначения свойств, отображаемых в заголовке
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class HeaderInfoAttribute : Attribute { }
}
