# gameSnake

Как играть:
W / ↑ - движение вверх
S / ↓ - движение вниз
A / ← - движение влево
D / → - движение вправо
Esc - выход из игры

Правила игры:
Управляйте змейкой с помощью клавиш
Собирайте еду (@) чтобы увеличить счет
Не врезайтесь в стены и в себя
Каждая съеденная еда дает 10 очков

Переменные:
width, height - размеры игрового поля
snakeX, snakeY - списки координат змейки
direction - текущее направление движения
score - счет игрока

Основные методы:
SetupGame() - начальная настройка игры
Draw() - отрисовка игрового поля
Input() - обработка нажатий клавиш
Logic() - игровая логика (движение, проверка столкновений)

Как работает змейка:
Змейка хранится как список координат
При движении добавляется новая голова в начале списка
Если не съели еду - удаляется последний элемент (хвост)

-------------------------------------------------


SnakeGame/
├── README.md                    # Описание проекта
├── SnakeGame.sln                # Файл решения
├── SnakeGame.csproj            # Файл проекта
├── Program.cs                   # Точка входа
│
├── Core/                       # Ядро игры
│   ├── GameSettings.cs         # Настройки игры
│   ├── Position.cs             # Структура позиции
│   ├── Direction.cs            # Перечисление направлений
│   └── GameState.cs            # Состояние игры (опционально)
│
├── Interfaces/                 # Интерфейсы (SOLID)
│   ├── IRenderer.cs
│   ├── IInputHandler.cs
│   ├── IGameObject.cs
│   ├── ICollisionDetector.cs
│   └── IGameObjectFactory.cs
│
├── GameObjects/                # Игровые объекты
│   ├── Snake/
│   │   ├── Snake.cs
│   │   ├── SnakeSegment.cs    # (опционально, для расширения)
│   │   └── SnakeState.cs      # (опционально)
│   │
│   └── Food/
│       ├── Food.cs
│       └── IFood.cs           # (опционально, для разных типов еды)
│
├── Systems/                    # Системы и сервисы
│   ├── Rendering/
│   │   └── ConsoleRenderer.cs
│   │
│   ├── Input/
│   │   └── ConsoleInputHandler.cs
│   │
│   ├── Collision/
│   │   └── CollisionDetector.cs
│   │
│   └── Factories/
│       ├── GameObjectFactory.cs
│       └── SnakeFactory.cs    # (опционально, для сложных сценариев)
│
├── Engine/                     # Игровой движок
│   ├── GameEngine.cs
│   ├── GameLoop.cs            # (опционально, для разделения логики)
│   └── GameInitializer.cs     # (опционально)
│
├── Game/                       # Фасад и запуск игры
│   ├── SnakeGame.cs           # Фасад
│   └── GameController.cs      # (опционально, для управления игрой)
│
├── Utils/                      # Вспомогательные классы
│   ├── RandomGenerator.cs     # Обертка для Random
│   ├── ConsoleHelper.cs       # Помощник для работы с консолью
│   └── Extensions/
│       └── PositionExtensions.cs
│
├── Exceptions/                 # Пользовательские исключения
│   ├── GameException.cs
│   ├── CollisionException.cs
│   └── InputException.cs
│
└── Tests/                      # Тесты (опционально)
    ├── UnitTests/
    │   ├── SnakeTests.cs
    │   ├── CollisionDetectorTests.cs
    │   └── GameEngineTests.cs
    │
    └── IntegrationTests/
        └── GameIntegrationTests.cs


Подробное описание структуры:
Core/
Базовые типы данных и настройки:
GameSettings - конфигурация игры
Position - структура координат
Direction - enum направлений
GameState - состояние игры (может содержать текущий счет, уровень и т.д.)

Interfaces/
Интерфейсы по SOLID:
IRenderer - отрисовка
IInputHandler - ввод
IGameObject - базовый интерфейс для игровых объектов
ICollisionDetector - обнаружение столкновений
IGameObjectFactory - фабрика объектов

GameObjects/
Сущности игрового мира:
Snake/ - змейка и ее компоненты
Food/ - еда (можно расширить для разных типов)

Systems/
Реализации систем:
Rendering/ - рендеринг (ConsoleRenderer, можно добавить GraphicalRenderer)
Input/ - обработка ввода
Collision/ - система столкновений
Factories/ - фабрики для создания объектов

Engine/
Игровой движок:
GameEngine - основной класс, управляющий игровым циклом
GameLoop - отдельный класс для игрового цикла (опционально)
GameInitializer - инициализатор игры

Game/
Фасад и запуск:
SnakeGame - фасад для упрощенного запуска
GameController - контроллер игры (опционально)

Utils/
Вспомогательные утилиты:
RandomGenerator - обертка для Random для лучшего тестирования
ConsoleHelper - методы для работы с консолью
Extensions/ - методы расширения

Exceptions/
Пользовательские исключения:
GameException - базовое исключение игры
CollisionException - исключения столкновений
InputException - исключения ввода

Tests/
Тесты:
UnitTests/ - модульные тесты
IntegrationTests/ - интеграционные тесты