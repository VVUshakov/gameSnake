using gameSnake.Attributes;

namespace gameSnake.Servises
{
    /// <summary>
    /// Содержит текст сервисных сообщений игры
    /// </summary>
    public static class GameMessages
    {
        /// <summary>
        /// Сообщение паузы
        /// </summary>
        [MessageInfo]
        public static string[] GetPauseMessage()
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

        /// <summary>
        /// Сообщение о проигрыше
        /// </summary>
        [MessageInfo]
        public static string[] GetGameOverMessage()
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

        /// <summary>
        /// Сообщение о победе
        /// </summary>
        [MessageInfo]
        public static string[] GetWinMessage()
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
    }
}
