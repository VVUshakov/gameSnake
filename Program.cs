// Основной класс программы
class Program
{
    // Константы для игрового поля
    private const int InitialWidth = 20; // Начальная ширина игрового поля
    private const int InitialHeight = 15; // Начальная высота игрового поля

    // Константы для змейки
    private const int InitialSnakeLength = 3; // Начальная длина змейки
    private const int InitialSnakeHeadX = 5; // Начальная X-координата головы змейки
    private const int InitialSnakeHeadY = 5; // Начальная Y-координата головы змейки
    private const char InitialDirection = 'R'; // Начальное направление движения (R - вправо)

    // Константы для скорости игры
    private const int InitialGameSpeed = 100; // Начальная скорость игры (в миллисекундах)

    // Константы для очков
    private const int FoodScoreValue = 10; // Количество очков за съеденную еду

    // Символы для отрисовки
    private const char BorderSymbol = '#'; // Символ для границ игрового поля
    private const char SnakeBodySymbol = 'O'; // Символ для отрисовки тела змейки
    private const char FoodSymbol = '@'; // Символ для отрисовки еды

    // Направления движения
    private const char DirectionUp = 'U'; // Направление вверх
    private const char DirectionDown = 'D'; // Направление вниз
    private const char DirectionLeft = 'L'; // Направление влево
    private const char DirectionRight = 'R'; // Направление вправо

    // Переменные состояния игры
    static int width = InitialWidth; // Текущая ширина игрового поля
    static int height = InitialHeight; // Текущая высота игрового поля
    static int score = 0; // Текущий счет игрока
    static int gameSpeed = InitialGameSpeed; // Текущая скорость игры

    // Списки для хранения координат змейки
    // snakeX хранит X-координаты каждого сегмента змейки
    // snakeY хранит Y-координаты каждого сегмента змейки
    static List<int> snakeX = new List<int>();
    static List<int> snakeY = new List<int>();

    // Координаты еды
    static int foodX, foodY;

    // Текущее направление движения змейки
    static char direction = InitialDirection;

    // Флаг окончания игры
    static bool gameOver = false;

    // Главный метод программы
    static void Main()
    {
        Console.Title = "Змейка"; // Устанавливаем заголовок окна консоли
        SetupGame(); // Инициализируем начальное состояние игры

        // Основной игровой цикл
        // Выполняется, пока игра не окончена (gameOver == false)
        while(!gameOver)
        {
            Draw(); // Отрисовываем текущее состояние игры
            Input(); // Обрабатываем ввод пользователя
            Logic(); // Обновляем логику игры
            System.Threading.Thread.Sleep(gameSpeed); // Задержка для управления скоростью игры
        }

        // После окончания игры выводим финальное сообщение
        Console.SetCursorPosition(0, height + 2); // Устанавливаем позицию курсора ниже игрового поля
        Console.WriteLine("Игра окончена! Счет: " + score); // Выводим итоговый счет
        Console.ReadKey(); // Ждем нажатия любой клавиши для выхода
    }

    // Метод инициализации начального состояния игры
    static void SetupGame()
    {
        // Очищаем списки координат змейки
        snakeX.Clear();
        snakeY.Clear();

        // Создаем начальную змейку из трех сегментов
        for(int i = 0; i < InitialSnakeLength; i++)
        {
            // Добавляем сегменты змейки по горизонтали
            snakeX.Add(InitialSnakeHeadX - i); // Каждый следующий сегмент левее предыдущего
            snakeY.Add(InitialSnakeHeadY); // Все сегменты на одной высоте
        }

        CreateFood(); // Создаем первую еду на поле
    }

    // Метод создания еды в случайном месте
    static void CreateFood()
    {
        Random random = new Random(); // Создаем генератор случайных чисел
        foodX = random.Next(0, width); // Генерируем случайную X-координату в пределах поля
        foodY = random.Next(0, height); // Генерируем случайную Y-координату в пределах поля
    }

    // Метод отрисовки игрового поля
    static void Draw()
    {
        Console.Clear(); // Очищаем консоль перед каждой отрисовкой

        // Рисуем верхнюю границу поля
        int borderLength = width + 2; // Длина границы (поле + 2 символа по бокам)
        Console.WriteLine(new string(BorderSymbol, borderLength)); // Создаем строку из символов границы

        // Отрисовываем каждую строку игрового поля
        for(int y = 0; y < height; y++)
        {
            Console.Write(BorderSymbol); // Левая граница поля

            // Отрисовываем каждый столбец в текущей строке
            for(int x = 0; x < width; x++)
            {
                // Проверяем, находится ли в этой клетке змейка
                if(IsSnake(x, y))
                {
                    Console.Write(SnakeBodySymbol); // Отрисовываем тело змейки
                }
                // Проверяем, находится ли в этой клетке еда
                else if(x == foodX && y == foodY)
                {
                    Console.Write(FoodSymbol); // Отрисовываем еду
                }
                else
                {
                    Console.Write(" "); // Отрисовываем пустую клетку
                }
            }

            Console.WriteLine(BorderSymbol); // Правая граница поля и переход на новую строку
        }

        // Рисуем нижнюю границу поля
        Console.WriteLine(new string(BorderSymbol, borderLength));

        // Выводим информацию о текущем счете
        Console.WriteLine("Счет: " + score);

        // Выводим подсказку по управлению
        Console.WriteLine("Управление: WASD, Выход: Esc");
    }

    // Метод проверки, находится ли змейка в указанных координатах
    static bool IsSnake(int x, int y)
    {
        // Проходим по всем сегментам змейки
        for(int i = 0; i < snakeX.Count; i++)
        {
            // Если координаты совпадают с координатами сегмента змейки
            if(snakeX[i] == x && snakeY[i] == y)
                return true; // Змейка находится в этой клетке
        }
        return false; // В этой клетке нет змейки
    }

    // Метод обработки ввода с клавиатуры
    static void Input()
    {
        // Проверяем, была ли нажата клавиша
        if(Console.KeyAvailable)
        {
            ConsoleKeyInfo key = Console.ReadKey(true); // Считываем нажатую клавишу

            // Обрабатываем нажатую клавишу
            switch(key.Key)
            {
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    // Если змейка не движется вниз, меняем направление наверх
                    if(direction != DirectionDown) direction = DirectionUp;
                    break;

                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    // Если змейка не движется вверх, меняем направление вниз
                    if(direction != DirectionUp) direction = DirectionDown;
                    break;

                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    // Если змейка не движется вправо, меняем направление влево
                    if(direction != DirectionRight) direction = DirectionLeft;
                    break;

                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    // Если змейка не движется влево, меняем направление вправо
                    if(direction != DirectionLeft) direction = DirectionRight;
                    break;

                case ConsoleKey.Escape:
                    // Выход из игры
                    gameOver = true;
                    break;
            }
        }
    }

    // Основная логика игры
    static void Logic()
    {
        // Сохраняем текущие координаты головы змейки
        int headX = snakeX[0];
        int headY = snakeY[0];

        // Двигаем голову в зависимости от текущего направления
        switch(direction)
        {
            case DirectionUp:
                headY--; // Движение вверх уменьшает Y-координату
                break;
            case DirectionDown:
                headY++; // Движение вниз увеличивает Y-координату
                break;
            case DirectionLeft:
                headX--; // Движение влево уменьшает X-координату
                break;
            case DirectionRight:
                headX++; // Движение вправо увеличивает X-координату
                break;
        }

        // Проверяем столкновение с границами поля
        int minBoundary = 0; // Минимальная координата (верхний левый угол)
        if(headX < minBoundary || headX >= width || headY < minBoundary || headY >= height)
        {
            gameOver = true; // Если вышли за границы - игра окончена
            return;
        }

        // Проверяем столкновение с собственным телом
        for(int i = 0; i < snakeX.Count; i++)
        {
            // Если голова совпадает с координатами любого сегмента тела
            if(snakeX[i] == headX && snakeY[i] == headY)
            {
                gameOver = true; // Столкновение с собой - игра окончена
                return;
            }
        }

        // Добавляем новую голову змейки
        int headIndex = 0; // Индекс для вставки новой головы
        snakeX.Insert(headIndex, headX); // Вставляем новые координаты в начало списка
        snakeY.Insert(headIndex, headY);

        // Проверяем, съели ли еду
        if(headX == foodX && headY == foodY)
        {
            score += FoodScoreValue; // Увеличиваем счет
            CreateFood(); // Создаем новую еду в случайном месте
        }
        else
        {
            // Если не съели еду, удаляем хвост змейки
            int tailIndex = snakeX.Count - 1; // Индекс последнего элемента (хвоста)
            snakeX.RemoveAt(tailIndex); // Удаляем хвост из списка X-координат
            snakeY.RemoveAt(tailIndex); // Удаляем хвост из списка Y-координат
        }
    }
}