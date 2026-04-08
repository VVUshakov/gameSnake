namespace ConsoleClient.UI.ConsoleRenderers
{
    /// <summary>
    /// Константы для отрисовки элементов игры в консоли
    /// </summary>
    public static class RenderConstants
    {
        /// <summary>Символ рамки игрового поля</summary>
        public const char BorderChar = '#';
        /// <summary>Символ головы змейки</summary>
        public const char SnakeHead = 'O';
        /// <summary>Символ тела змейки</summary>
        public const char SnakeBody = '*';
        /// <summary>Символ еды</summary>
        public const char FoodSymbol = '@';

        /// <summary>Цвет сообщения о проигрыше</summary>
        public const ConsoleColor GameOverColor = ConsoleColor.Red;
        /// <summary>Цвет сообщения о победе</summary>
        public const ConsoleColor GameWinColor = ConsoleColor.Green;
        /// <summary>Цвет сообщения о паузе</summary>
        public const ConsoleColor PauseColor = ConsoleColor.Yellow;
        /// <summary>Цвет сообщения по умолчанию</summary>
        public const ConsoleColor DefaultMessageColor = ConsoleColor.White;
    }
}
