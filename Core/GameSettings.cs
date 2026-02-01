namespace SnakeGame.Core
{
    /// <summary>
    /// Класс, содержащий настройки игры
    /// </summary>
    public class GameSettings
    {
        /// <summary>
        /// Ширина игрового поля
        /// </summary>
        public int Width { get; } = 20;

        /// <summary>
        /// Высота игрового поля
        /// </summary>
        public int Height { get; } = 15;

        /// <summary>
        /// Начальная длина змейки
        /// </summary>
        public int InitialSnakeLength { get; } = 3;

        /// <summary>
        /// Количество очков за съеденную еду
        /// </summary>
        public int FoodScoreValue { get; } = 10;

        /// <summary>
        /// Скорость игры в миллисекундах
        /// </summary>
        public int GameSpeed { get; } = 100;

        /// <summary>
        /// Начальная позиция змейки
        /// </summary>
        public Position InitialSnakePosition { get; } = new Position(5, 5);

        /// <summary>
        /// Создает настройки игры со значениями по умолчанию
        /// </summary>
        public GameSettings() { }

        /// <summary>
        /// Создает настройки игры с пользовательскими значениями
        /// </summary>
        /// <param name="width">Ширина игрового поля</param>
        /// <param name="height">Высота игрового поля</param>
        /// <param name="initialSnakeLength">Начальная длина змейки</param>
        /// <param name="foodScoreValue">Количество очков за еду</param>
        /// <param name="gameSpeed">Скорость игры</param>
        /// <param name="initialPosition">Начальная позиция змейки</param>
        public GameSettings(int width, int height, int initialSnakeLength, int foodScoreValue,
                           int gameSpeed, Position initialPosition)
        {
            Width = width;
            Height = height;
            InitialSnakeLength = initialSnakeLength;
            FoodScoreValue = foodScoreValue;
            GameSpeed = gameSpeed;
            InitialSnakePosition = initialPosition;
        }
    }
}