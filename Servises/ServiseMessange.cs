using System.Reflection;
using gameSnake.Utils;

namespace gameSnake.Servises
{
    internal class ServiseMessange
    {
        [MessageInfo]
        internal static string[] GetPauseMessange()
        {
            return new string[]
            {
                "-------------------------¬",
                "¦       -- ПАУЗА --      ¦",
                "¦                        ¦",
                "¦  Spacebar - продолжить ¦",
                "¦  Escape - выйти        ¦",
                "L-------------------------"
            };
        }

        [MessageInfo]
        internal static string[] GetGameOverMessange()
        {
            return new string[]
            {
                "ИГРА ОКОНЧЕНА!",
                "",
                "Хотите сыграть ещё?",
                "Нажмите Enter для продолжения",
                "Нажмите Escape для выхода"
            };
        }

        [MessageInfo]
        internal static string[] GetGameWinMessange()
        {
            return new string[]
            {
                "ПОБЕДА!",
                "",
                "Хотите сыграть ещё?",
                "Нажмите Enter для продолжения",
                "Нажмите Escape для выхода"
            };
        }

        /// <summary>
        /// Автоматически находит все методы с атрибутом MessageInfo и вызывает их
        /// </summary>
        internal static List<string[]> GetAllMessages()
        {
            var messages = new List<string[]>();

            MethodInfo[] methods = typeof(ServiseMessange).GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (MethodInfo method in methods)
            {
                // Пропускаем методы без атрибута MessageInfo
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

