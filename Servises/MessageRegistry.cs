using System.Reflection;
using gameSnake.Attributes;

namespace gameSnake.Servises
{
    /// <summary>
    /// Реестр сервисных сообщений.
    /// Использует рефлексию для автоматического обнаружения всех методов с атрибутом [MessageInfo].
    /// </summary>
    public static class MessageRegistry
    {
        /// <summary>
        /// Возвращает все сообщения, помеченные атрибутом [MessageInfo].
        /// </summary>
        public static List<string[]> GetAll()
        {
            var messages = new List<string[]>();
            MethodInfo[] methods = typeof(GameMessages).GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (MethodInfo method in methods)
            {
                if (method.GetCustomAttribute<MessageInfoAttribute>() == null) continue;

                string[]? result = method.Invoke(null, null) as string[];
                if (result != null)
                {
                    messages.Add(result);
                }
            }

            return messages;
        }
    }
}
