class Program
{
    static int width = 20;
    static int height = 15;
    static int score = 0;

    static List<int> snakeX = new List<int>();
    static List<int> snakeY = new List<int>();
    static int foodX, foodY;

    static char direction = 'R'; // Начальное направление: вправо
    static bool gameOver = false;

    static void Main()
    {
        Console.Title = "Змейка";
        SetupGame();

        // Игровой цикл
        while(!gameOver)
        {
            Draw();
            Input();
            Logic();
            System.Threading.Thread.Sleep(100); // Скорость игры
        }

        Console.SetCursorPosition(0, height + 2);
        Console.WriteLine("Игра окончена! Счет: " + score);
        Console.ReadKey();
    }

    static void SetupGame()
    {
        // Начальная позиция змейки (3 сегмента)
        snakeX.Clear();
        snakeY.Clear();

        snakeX.Add(5);
        snakeY.Add(5);

        snakeX.Add(4);
        snakeY.Add(5);

        snakeX.Add(3);
        snakeY.Add(5);

        // Создаем первую еду
        CreateFood();
    }

    static void CreateFood()
    {
        Random random = new Random();
        foodX = random.Next(0, width);
        foodY = random.Next(0, height);
    }

    static void Draw()
    {
        Console.Clear();

        // Рисуем верхнюю границу
        Console.WriteLine(new string('#', width + 2));

        // Рисуем поле
        for(int y = 0; y < height; y++)
        {
            Console.Write("#"); // Левая граница

            for(int x = 0; x < width; x++)
            {
                if(IsSnake(x, y))
                {
                    Console.Write("O"); // Тело змейки
                }
                else if(x == foodX && y == foodY)
                {
                    Console.Write("@"); // Еда
                }
                else
                {
                    Console.Write(" ");
                }
            }

            Console.WriteLine("#"); // Правая граница
        }

        // Рисуем нижнюю границу
        Console.WriteLine(new string('#', width + 2));

        // Показываем счет
        Console.WriteLine("Счет: " + score);
        Console.WriteLine("Управление: WASD, Выход: Esc");
    }

    static bool IsSnake(int x, int y)
    {
        for(int i = 0; i < snakeX.Count; i++)
        {
            if(snakeX[i] == x && snakeY[i] == y)
                return true;
        }
        return false;
    }

    static void Input()
    {
        if(Console.KeyAvailable)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);

            switch(key.Key)
            {
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    if(direction != 'D') direction = 'U';
                    break;

                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    if(direction != 'U') direction = 'D';
                    break;

                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    if(direction != 'R') direction = 'L';
                    break;

                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    if(direction != 'L') direction = 'R';
                    break;

                case ConsoleKey.Escape:
                    gameOver = true;
                    break;
            }
        }
    }

    static void Logic()
    {
        // Сохраняем текущую голову
        int headX = snakeX[0];
        int headY = snakeY[0];

        // Двигаем голову в зависимости от направления
        switch(direction)
        {
            case 'U': headY--; break;
            case 'D': headY++; break;
            case 'L': headX--; break;
            case 'R': headX++; break;
        }

        // Проверяем столкновение с границами
        if(headX < 0 || headX >= width || headY < 0 || headY >= height)
        {
            gameOver = true;
            return;
        }

        // Проверяем столкновение с собой
        for(int i = 0; i < snakeX.Count; i++)
        {
            if(snakeX[i] == headX && snakeY[i] == headY)
            {
                gameOver = true;
                return;
            }
        }

        // Добавляем новую голову
        snakeX.Insert(0, headX);
        snakeY.Insert(0, headY);

        // Проверяем, съели ли еду
        if(headX == foodX && headY == foodY)
        {
            score += 10;
            CreateFood();
        }
        else
        {
            // Удаляем хвост, если не съели еду
            snakeX.RemoveAt(snakeX.Count - 1);
            snakeY.RemoveAt(snakeY.Count - 1);
        }
    }
}