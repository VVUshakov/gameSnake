using gameSnake.Core;
using gameSnake.GameObjects.Food;
using gameSnake.GameObjects.Snake;
using gameSnake.Interfaces;

namespace gameSnake.Systems.Rendering
{
    /// <summary>
    /// Класс для отрисовки игры в консоли
    /// </summary>
    public class ConsoleRenderer : IRenderer
    {
        private const char BorderSymbol = '#';
        private const char SnakeBodySymbol = 'O';
        private const char FoodSymbol = '@';

        private readonly GameSettings _settings;

        /// <summary>
        /// Создает новый рендерер для консоли
        /// </summary>
        /// <param name="settings">Настройки игры</param>
        public ConsoleRenderer(GameSettings settings)
        {
            _settings = settings;
        }

        /// <summary>
        /// Отрисовывает текущее состояние игры
        /// </summary>
        /// <param name="snake">Змейка для отрисовки</param>
        /// <param name="food">Еда для отрисовки</param>
        /// <param name="score">Текущий счет</param>
        public void DrawGame(Snake snake, Food food, int score)
        {
            Clear();
            DrawBorder();
            DrawSnake(snake);
            DrawFood(food);
            DrawScore(score);
            DrawControls();
        }

        /// <summary>
        /// Отрисовывает экран завершения игры
        /// </summary>
        /// <param name="score">Финальный счет</param>
        public void DrawGameOver(int score)
        {
            Console.SetCursorPosition(0, _settings.Height + 4);
            Console.WriteLine("Игра окончена! Счет: " + score);
        }

        /// <summary>
        /// Очищает экран игры
        /// </summary>
        public void Clear()
        {
            Console.Clear();
        }

        /// <summary>
        /// Рисует границы игрового поля
        /// </summary>
        private void DrawBorder()
        {
            int borderLength = _settings.Width + 2;
            Console.WriteLine(new string(BorderSymbol, borderLength));

            for(int y = 0; y < _settings.Height; y++)
            {
                Console.Write(BorderSymbol);
                for(int x = 0; x < _settings.Width; x++)
                {
                    Console.Write(' ');
                }
                Console.WriteLine(BorderSymbol);
            }

            Console.WriteLine(new string(BorderSymbol, borderLength));
        }

        /// <summary>
        /// Рисует змейку на игровом поле
        /// </summary>
        /// <param name="snake">Змейка для отрисовки</param>
        private void DrawSnake(Snake snake)
        {
            foreach(var segment in snake.Body)
            {
                Console.SetCursorPosition(segment.X + 1, segment.Y + 1);
                Console.Write(SnakeBodySymbol);
            }
        }

        /// <summary>
        /// Рисует еду на игровом поле
        /// </summary>
        /// <param name="food">Еда для отрисовки</param>
        private void DrawFood(Food food)
        {
            Console.SetCursorPosition(food.Position.X + 1, food.Position.Y + 1);
            Console.Write(FoodSymbol);
        }

        /// <summary>
        /// Отображает текущий счет
        /// </summary>
        /// <param name="score">Текущий счет</param>
        private void DrawScore(int score)
        {
            Console.SetCursorPosition(0, _settings.Height + 2);
            Console.WriteLine($"Счет: {score}");
        }

        /// <summary>
        /// Отображает подсказку по управлению
        /// </summary>
        private void DrawControls()
        {
            Console.SetCursorPosition(0, _settings.Height + 3);
            Console.WriteLine("Управление: WASD/Стрелки, Выход: Esc");
        }
    }
}
