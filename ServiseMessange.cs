using System;
using Snake.Utils;

namespace gameSnake
{
    internal class ServiseMessange
    {
        // Список всех методов, возвращающих сообщения
        private static readonly Func<string[]>[] _messageSources =
        {
            GetPauseMessange,
            GetGameOverMessange,
            GetGameWinMessange
        };

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
        /// Возвращает все сервисные сообщения, вызывая каждый делегат из списка
        /// </summary>
        internal static List<string[]> GetAllMessages()
        {
            var messages = new List<string[]>();

            foreach(var source in _messageSources)
            {
                messages.Add(source());
            }

            return messages;
        }
    }
}