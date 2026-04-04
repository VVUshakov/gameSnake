namespace gameSnake.Attributes
{
    /// <summary>
    /// Атрибут-маркер для обозначения методов, возвращающих сообщения
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class MessageInfoAttribute : Attribute { }
}
