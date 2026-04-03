using System.Reflection;
using gameSnake.Utils;

namespace gameSnake.Servises
{
    internal class ServiseMessange
    {
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
        /// Автоматически находит все методы, возвращающие string[], и вызывает их
        /// </summary>
        internal static List<string[]> GetAllMessages()
        {
            var messages = new List<string[]>();

            // Получаем все статические методы текущего класса
            MethodInfo[] methods = typeof(ServiseMessange).GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

            foreach(MethodInfo method in methods)
            {
                // Проверяем, что метод возвращает string[]
                if(method.ReturnType == typeof(string[]))
                {
                    // Вызываем метод (параметр null для статического метода)
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