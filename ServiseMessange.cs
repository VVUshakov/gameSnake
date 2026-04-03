using System.Reflection;
using Snake.Utils;

namespace gameSnake
{
    internal class ServiseMessange
    {
        [ServiceMessage]
        internal static string[] GetPauseMessange()
        {
            return new string[]
            {
                "┌────────────────────────┐",
                "│       ── ПАУЗА ──      │",
                "│                        │",
                "│  Spacebar - продолжить │",
                "│  Escape - выйти        │",
                "└────────────────────────┘"
            };
        }

        [ServiceMessage]
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

        [ServiceMessage]
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
        /// Находит все методы с атрибутом [ServiceMessage] и вызывает их
        /// </summary>
        internal static List<string[]> GetAllMessages()
        {
            var messages = new List<string[]>();

            MethodInfo[] methods = typeof(ServiseMessange).GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

            foreach(MethodInfo method in methods)
            {
                // Проверяем наличие атрибута [ServiceMessage]
                bool hasAttribute = method.IsDefined(typeof(ServiceMessageAttribute), inherit: false);

                if(hasAttribute)
                {
                    string[]? result = method.Invoke(null, null) as string[];
                    if(result != null)
                    {
                        messages.Add(result);
                    }
                }
            }

            return messages;
        }
    }
}
