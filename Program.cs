using Snake.UI.ConsoleUI;
using Snake.Core;
using Snake.Interfaces;
using Snake.Logic;
using Snake.Models;

namespace Snake
{
    /// <summary>
    /// Точка входа в приложение
    /// </summary>
    public class Program
    {
        static void Main()
        {
            // Настраиваем размер консольного окна
            SetupConsoleWindow();

            // Создаём зависимости
            IGameRenderer renderer = new ConsoleRenderer();
            IInputHandler input = new ConsoleInputHandler();
            IGameLogic logic = new SnakeGameLogic();

            // Создаём и запускаем игровой цикл
            GameLoop gameLoop = new GameLoop(renderer, input, logic);
            gameLoop.RunWithRestart();
        }

        /// <summary>
        /// Настраивает размер консольного окна под игровое поле
        /// </summary>
        private static void SetupConsoleWindow()
        {
            // Размер поля по умолчанию 30x30, добавляем запас для рамки и заголовка
            int windowWidth = PlayingField.MinWidth + 5;   // +5 для запаса
            int windowHeight = PlayingField.MinHeight + 10; // +10 для заголовка и запаса

            // Устанавливаем размер буфера консоли
            Console.SetWindowSize(
                Math.Max(windowWidth, Console.LargestWindowWidth / 2),
                Math.Max(windowHeight, Console.LargestWindowHeight / 2)
            );

            // Устанавливаем размер буфера больше окна для прокрутки
            Console.SetBufferSize(
                Math.Max(windowWidth, Console.LargestWindowWidth / 2),
                Math.Max(windowHeight * 2, Console.LargestWindowHeight)
            );
        }
    }
}