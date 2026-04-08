# 🐍 Как превратить Змейку из консольной игры в WebSocket-сервер

Это пошаговое руководство для начинающих.
Мы превратим обычную консольную игру «Змейка» в **сервер**, 
к которому смогут подключаться игроки с любых устройств
(компьютер, телефон, планшет) через браузер или приложение.

---

## 📋 Что нам нужно знать заранее

| Термин        | Что значит                                                                                    |
|---------------|-----------------------------------------------------------------------------------------------|
| **WebSocket** | Канал связи между клиентом и сервером. Оба могут отправлять сообщения в любой момент          |
| **Сервер**    | Программа, которая «слушает» подключения и управляет игрой                                    |
| **Клиент**    | Программа (браузер, приложение), которая подключается к серверу                               |
| **JSON**      | Формат данных, понятный и серверу, и клиенту: `{"type":"move","direction":"up"}`              |
| **DTO**       | «Data Transfer Object» — объект для передачи данных между сервером и клиентом                 |
| **Интерфейс** | «Контракт» — описание методов без реализации. Кто угодно может реализовать интерфейс по-своему|

---

## 🗺️ Общая картина

Мы делаем **минимальные изменения** в существующем коде:

```
┌─────────────────────────────────────────────────────┐
│                    ЯДРО (НЕ ТРОГАЕМ)                │
│  Core/   — Game, GameLoop, GameState                │
│  Models/ — Snake, Food, Point, Direction            │
│  Logic/  — шаги логики, CollisionDetector           │
│  Factories/ — SnakeFactory, FoodFactory, ...        │
└─────────────────────────────────────────────────────┘
         ▲                               ▲
         │ interfaces                    │ interfaces
         ▼                               ▼
┌─────────────────┐           ┌──────────────────────┐
│  БЫЛО: ConsoleUI│           │  СТАЛО: Server       │
│  ConsoleRenderer│  → УДАЛЯЕМ│  WebSocketRenderer   │
│  ConsoleInput   │           │  WebSocketInput      │
└─────────────────┘           └──────────────────────┘
```

---

## 🎯 Этап 1. Переключаем проект на ASP.NET Core

### Что было
Файл `gameSnake.csproj` начинался так:
```xml
<Project Sdk="Microsoft.NET.Sdk">
```
Это обычный консольный проект.

### Что делаем
Меняем SDK на веб-версию:
```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
```

### Зачем
`Microsoft.NET.Sdk.Web` добавляет:
- ASP.NET Core (веб-сервер)
- WebSocket API
- Всё, что нужно для сервера

### Где это
**Файл:** `gameSnake.csproj` (самый верх, строка 1)

---

## 🎯 Этап 2. Удаляем консольный интерфейс

### Что удаляем
Всю папку `UI/ConsoleUI/`. Там были:
- `ConsoleRenderer.cs` — рисовал игру в консоли
- `ConsoleInputHandler.cs` — читал клавиатуру
- `ConsoleWindowConfigurator.cs` — настраивал размер окна
- и другие файлы консоли

### Зачем
Сервер не рисует и не читает клавиатуру. Он отправляет данные клиентам через WebSocket.

### Как
В терминале:
```bash
rmdir /s /q UI
```
Или через проводник — удали папку `UI`.

---

## 🎯 Этап 3. Создаём DTO (объекты для передачи данных)

### Что такое DTO
Когда сервер и клиент общаются, они не могут передавать объекты C# напрямую.
Они отправляют **JSON-строки**. 
DTO — это классы, которые описывают, как данные будут выглядеть в JSON.

### Создаём папку
```
Server/DTO/
```

### Создаём файл `GameDto.cs`

```csharp
using System.Text.Json.Serialization;

namespace gameSnake.Server.DTO
{
    // Статус игры — клиент может показать его на экране
    public enum GameStatus
    {
        Playing,   // Игра идёт
        Paused,    // Пауза
        GameOver,  // Проигрыш
        Win        // Победа
    }

    // Точка на поле (X, Y) — для змейки и еды
    public record PointDto(int X, int Y);

    // Полное состояние игры — отправляется клиенту каждый кадр
    public class GameStateDto
    {
        [JsonPropertyName("status")]
        public GameStatus Status { get; set; }

        [JsonPropertyName("snake")]
        public List<PointDto> Snake { get; set; } = new();

        [JsonPropertyName("food")]
        public PointDto? Food { get; set; }

        [JsonPropertyName("score")]
        public int Score { get; set; }

        [JsonPropertyName("level")]
        public int Level { get; set; }

        [JsonPropertyName("lives")]
        public int Lives { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }

    // Команда от клиента — игрок нажал кнопку
    public class ClientCommand
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("direction")]
        public string? Direction { get; set; }
    }
}
```

### Что здесь происходит
| Класс             | Зачем                                                                 |
|-------------------|-----------------------------------------------------------------------|
| `GameStatus`      | Перечисление: в каком состоянии игра                                  |
| `PointDto`        | Координата на поле. `record` = автоматически имеет Equals, GetHashCode|
| `GameStateDto`    | Всё, что нужно клиенту для отрисовки одного кадра                     |
| `ClientCommand`   | Команда от игрока: «двигайся вверх», «пауза», «перезапуск»            |

### Пример JSON
Сервер отправляет клиенту:
```json
{
  "status": "Playing",
  "snake": [{"x":5,"y":5}, {"x":4,"y":5}, {"x":3,"y":5}],
  "food": {"x":8,"y":3},
  "score": 50,
  "level": 1,
  "lives": 1,
  "message": null
}
```

Клиент отправляет серверу:
```json
{"type":"move","direction":"up"}
```

---

## 🎯 Этап 4. Создаём WebSocket-рендерер

### Что было
`ConsoleRenderer` рисовал в консоли: `Console.Write("#")`, `Console.SetCursorPosition(x, y)`.

### Что делаем
Создаём `WebSocketRenderer` — он **не рисует**, а **отправляет JSON** клиенту.

### Создаём папку
```
Server/Renderers/
```

### Создаём файл `WebSocketRenderer.cs`

```csharp
using gameSnake.Core.State;
using gameSnake.Interfaces;
using gameSnake.Models;
using gameSnake.Server.DTO;
using System.Net.WebSockets;
using System.Text.Json;

namespace gameSnake.Server.Renderers
{
    /// <summary>
    /// Рендерер, отправляющий состояние игры клиенту через WebSocket.
    /// </summary>
    public class WebSocketRenderer : IGameRenderer
    {
        private readonly WebSocket _webSocket;

        public WebSocketRenderer(WebSocket webSocket)
        {
            _webSocket = webSocket;
        }

        // Очищать консоль не нужно — клиент сам обновляет экран
        public void Clear() { }

        public void Render(GameState state)
        {
            // 1. Превращаем GameState в DTO
            var dto = ToDto(state);

            // 2. Превращаем DTO в JSON
            var json = JsonSerializer.Serialize(dto);

            // 3. Отправляем JSON клиенту
            var buffer = System.Text.Encoding.UTF8.GetBytes(json);
            _webSocket.SendAsync(
                new ArraySegment<byte>(buffer),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None).Wait();
        }

        // Превращаем внутреннее состояние игры в DTO для клиента
        private static GameStateDto ToDto(GameState state)
        {
            return new GameStateDto
            {
                Status = ResolveStatus(state.Flags),
                Snake = state.Snake.Body.Select(p => new PointDto(p.X, p.Y)).ToList(),
                Food = state.Food.IsSuccess && state.Food.Position.HasValue
                    ? new PointDto(state.Food.Position.Value.X, state.Food.Position.Value.Y)
                    : null,
                Score = state.Header.Score,
                Level = state.Header.Level,
                Lives = state.Header.Lives,
                Message = state.ActiveMessage switch
                {
                    GameMessageType.Pause    => "Пауза",
                    GameMessageType.GameOver => "Игра окончена!",
                    GameMessageType.Win      => "Победа!",
                    _ => null
                }
            };
        }

        private static GameStatus ResolveStatus(GameFlags flags)
        {
            if (flags.IsWin)       return GameStatus.Win;
            if (flags.IsGameOver)  return GameStatus.GameOver;
            if (flags.IsPaused)    return GameStatus.Paused;
            return GameStatus.Playing;
        }
    }
}
```

### Что здесь происходит
| Метод                 | Зачем                                                                 |
|-----------------------|-----------------------------------------------------------------------|
| `Clear()`             | Пустой — клиент сам решает, когда обновлять экран                     |
| `Render(state)`       | Главный метод. Превращает `GameState` → JSON → отправляет по WebSocket|
| `ToDto(state)`        | Берёт внутренние объекты (Snake, Food, Header) и превращает в DTO     |
| `ResolveStatus(flags)`| Определяет статус игры по флагам                                      |

### Ключевая идея
`WebSocketRenderer` реализует тот же интерфейс `IGameRenderer`, что и старый `ConsoleRenderer`.
Поэтому **ядро игры ничего не заметило** — оно вызывает `Render()`, как раньше.

---

## 🎯 Этап 5. Создаём WebSocket-обработчик ввода

### Что было
`ConsoleInputHandler` читал клавиатуру: `Console.ReadKey()`.

### Что делаем
Создаём `WebSocketInputHandler` — он **принимает команды** от клиента через WebSocket.

### Создаём папку
```
Server/InputHandlers/
```

### Создаём файл `WebSocketInputHandler.cs`

```csharp
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text.Json;
using gameSnake.Interfaces;
using gameSnake.Models;
using gameSnake.Server.DTO;

namespace gameSnake.Server.InputHandlers
{
    /// <summary>
    /// Обработчик ввода, получающий команды через WebSocket.
    /// Читает команды из очереди, заполняемой фоновым читателем.
    /// </summary>
    public class WebSocketInputHandler : IInputHandler, IDisposable
    {
        // Очередь команд от клиента (потокобезопасная)
        private readonly ConcurrentQueue<ClientCommand> _commandQueue = new();
        private readonly CancellationToken _cancellationToken;
        private readonly WebSocket _webSocket;
        private readonly Task _readTask;

        public WebSocketInputHandler(WebSocket webSocket, CancellationToken cancellationToken)
        {
            _webSocket = webSocket;
            _cancellationToken = cancellationToken;
            // Запускаем фоновое чтение команд
            _readTask = Task.Run(() => ReadLoop(cancellationToken));
        }

        // Вызывается каждый кадр игры — обрабатывает накопленные команды
        public void ProcessInput(IInputState inputState, int snakeLength)
        {
            // При отключении — выходим из игры
            if (_cancellationToken.IsCancellationRequested)
                inputState.IsExit = true;

            // Обрабатываем все накопленные команды
            while (_commandQueue.TryDequeue(out var command))
            {
                ApplyCommand(command, inputState, snakeLength);
            }
        }

        public void Dispose()
        {
            try { _readTask.Wait(TimeSpan.FromSeconds(2)); }
            catch (AggregateException) { }
        }

        // Применяем одну команду к состоянию игры
        private void ApplyCommand(ClientCommand command, IInputState state, int snakeLength)
        {
            switch (command.Type.ToLower())
            {
                case "pause":
                    state.IsPaused = !state.IsPaused;
                    break;
                case "restart":
                    state.IsRestartRequested = true;
                    break;
                case "exit":
                    state.IsExit = true;
                    break;
                case "move" when command.Direction != null:
                    ApplyDirection(state, command.Direction.ToLower(), snakeLength);
                    break;
            }
        }

        // Меняем направление с защитой от разворота на 180°
        private static void ApplyDirection(IInputState state, string dir, int snakeLength)
        {
            Direction newDir = dir switch
            {
                "up"    => Direction.Up,
                "down"  => Direction.Down,
                "left"  => Direction.Left,
                "right" => Direction.Right,
                _       => state.CurrentDirection
            };

            if (state.IsPaused) return;

            Direction opposite = dir switch
            {
                "up"    => Direction.Down,
                "down"  => Direction.Up,
                "left"  => Direction.Right,
                "right" => Direction.Left,
                _       => state.CurrentDirection
            };

            // Разворот на 180° запрещён (если змейка длиннее 1)
            if (snakeLength <= 1 || state.CurrentDirection != opposite)
                state.CurrentDirection = newDir;
        }

        // Фоновый цикл: читает WebSocket и складывает команды в очередь
        private async Task ReadLoop(CancellationToken ct)
        {
            var buffer = new byte[1024];
            while (!ct.IsCancellationRequested && _webSocket.State == WebSocketState.Open)
            {
                try
                {
                    var result = await _webSocket.ReceiveAsync(
                        new ArraySegment<byte>(buffer), ct);

                    if (result.MessageType == WebSocketMessageType.Close)
                        break;

                    if (result.MessageType == WebSocketMessageType.Text && result.Count > 0)
                    {
                        var json = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);
                        var command = JsonSerializer.Deserialize<ClientCommand>(json);
                        if (command != null)
                            _commandQueue.Enqueue(command);
                    }
                }
                catch (OperationCanceledException) { break; }
                catch (WebSocketException) { break; }
            }
        }
    }
}
```

### Что здесь происходит
| Компонент                       | Зачем                                                                              |
|---------------------------------|------------------------------------------------------------------------------------|
| `ConcurrentQueue<ClientCommand>`| Очередь команд. Клиент может отправить 5 команд за кадр — все попадут в очередь    |
| `ReadLoop()`                    | Фоновый `Task`. Постоянно слушает WebSocket и складывает команды в очередь         |
| `ProcessInput()`                | Вызывается каждый кадр игры. Берёт команды из очереди и применяет их               |
| `ApplyCommand()`                | Расшифровывает команду: «pause» → переключить паузу, «move» → изменить направление |

### Почему очередь?
Клиент может нажать «вверх» три раза, пока сервер обрабатывает предыдущий кадр.
Очередь сохраняет все команды.
`ReadLoop` пишет в очередь, `ProcessInput` читает — и всё работает без конфликтов.

---

## 🎯 Этап 6. Создаём заглушку конфигуратора окна

### Что было
`ConsoleWindowConfigurator` устанавливал размер окна консоли.

### Что делаем
Серверу не нужно окно. Создаём «пустышку».

### Создаём файл
```
Server/NoOpWindowConfigurator.cs
```

```csharp
using gameSnake.Interfaces;

namespace gameSnake.Server
{
    /// <summary>
    /// Пустая реализация конфигуратора окна для серверной среды.
    /// На сервере нет окна — размеры игрового поля хранятся в GameState.
    /// </summary>
    public class NoOpWindowConfigurator : IWindowConfigurator
    {
        public void Configure(int fieldWidth, int fieldHeight) { }
    }
}
```

### Зачем
Интерфейс `IWindowConfigurator` требует что-то реализовать.
На сервере — делаем ничего. Это называется **Null Object Pattern**.

---

## 🎯 Этап 7. Создаём менеджер сессий

### Зачем
Каждый игрок — это отдельная «сессия».
Менеджер хранит всех подключённых игроков и считает статистику.

### Создаём папку
```
Server/Session/
```

### Создаём файл
```
Server/Session/SessionManager.cs
```

```csharp
using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace gameSnake.Server.Session
{
    /// <summary>
    /// Реестр активных игровых сессий.
    /// </summary>
    public static class SessionManager
    {
        // Одновременный словарь — можно читать и писать из разных потоков
        private static readonly ConcurrentDictionary<Guid, WebSocketGameSession> _sessions = new();

        /// <summary>Количество активных сессий</summary>
        public static int ActiveCount => _sessions.Count;

        /// <summary>Регистрирует новую сессию и запускает игру</summary>
        public static Guid Register(WebSocket webSocket)
        {
            var session = new WebSocketGameSession(webSocket);
            if (_sessions.TryAdd(session.Id, session))
            {
                session.Start();
                return session.Id;
            }
            throw new InvalidOperationException("Не удалось зарегистрировать сессию");
        }

        /// <summary>Удаляет сессию и освобождает ресурсы</summary>
        public static void Unregister(Guid sessionId)
        {
            if (_sessions.TryRemove(sessionId, out var session))
                session.Dispose();
        }

        /// <summary>Останавливает все сессии (при выключении сервера)</summary>
        public static void StopAll()
        {
            foreach (var id in _sessions.Keys.ToArray())
                Unregister(id);
        }
    }
}
```

### Что здесь происходит
| Метод         | Зачем                                                   |
|---------------|---------------------------------------------------------|
| `Register()`  | Игрок подключился → создаём сессию → запускаем игру     |
| `Unregister()`| Игрок отключился → удаляем сессию → освобождаем ресурсы |
| `StopAll()`   | Сервер выключается → останавливаем всех                 |
| `ActiveCount` | Сколько сейчас игроков онлайн                           |

---

## 🎯 Этап 8. Создаём игровую сессию

### Зачем
Сессия — это «один игрок = одна игра». Она:
1. Создаёт зависимости (рендерер, обработчик ввода, логику)
2. Запускает игру
3. Обрабатывает перезапуск
4. Корректно завершается при отключении

### Создаём файл
```
Server/Session/WebSocketGameSession.cs
```

```csharp
using System.Net.WebSockets;
using gameSnake.Core.Engine;
using gameSnake.Core.Factories;
using gameSnake.Core.State;
using gameSnake.Logic.SnakeLogic;
using gameSnake.Server.InputHandlers;
using gameSnake.Server.Renderers;
using gameSnake.Servises.MessageServise;
using gameSnake.Utils;

namespace gameSnake.Server.Session
{
    /// <summary>
    /// Управляет одной игровой сессией WebSocket-подключения.
    /// </summary>
    public class WebSocketGameSession : IDisposable
    {
        public Guid Id { get; } = Guid.NewGuid();

        private readonly WebSocket _webSocket;
        private CancellationTokenSource? _disconnectToken;
        private Task? _gameTask;
        private WebSocketInputHandler? _inputHandler;

        public WebSocketGameSession(WebSocket webSocket)
        {
            _webSocket = webSocket;
        }

        /// <summary>Запускает игровые циклы в фоновом потоке</summary>
        public void Start()
        {
            _disconnectToken = new CancellationTokenSource();
            _gameTask = Task.Run(() => RunSession(_disconnectToken.Token));
        }

        private void RunSession(CancellationToken ct)
        {
            try
            {
                // Один обработчик ввода на всю сессию (все перезапуски)
                _inputHandler = new WebSocketInputHandler(_webSocket, ct);

                while (!ct.IsCancellationRequested)
                {
                    var state = CreateGameState();
                    var game = CreateGame(_inputHandler);
                    game.Run(state);

                    // Если игрок нажал «выход» — завершаем сессию
                    // Если «рестарт» — запускаем заново
                    if (state.Flags.IsExit || !state.Flags.IsRestartRequested)
                        break;
                }
            }
            finally
            {
                _inputHandler?.Dispose();
            }
        }

        // Создаём начальное состояние игры через фабрики
        private static GameState CreateGameState()
        {
            var messages = MessageRegistry.GetAll();
            var (maxMsgWidth, maxMsgHeight) = MessageSizer.GetMaxSize(messages);
            (int fieldWidth, int fieldHeight) = FieldSizeCalculator.Calculate(maxMsgWidth, maxMsgHeight);
            return GameStateFactory.Create(fieldWidth, fieldHeight);
        }

        // Собираем игру из зависимостей
        private Game CreateGame(WebSocketInputHandler inputHandler)
        {
            var renderer = new WebSocketRenderer(_webSocket);
            return new Game(renderer, inputHandler, new SnakeGameLogic(), new Core.SystemTimer());
        }

        /// <summary>Корректно останавливает сессию</summary>
        public void Dispose()
        {
            _disconnectToken?.Cancel();
            try { _gameTask?.Wait(TimeSpan.FromSeconds(3)); }
            catch (AggregateException) { }
        }
    }
}
```

### Цикл жизни сессии
```
Подключение
    ↓
SessionManager.Register() → new WebSocketGameSession()
    ↓
session.Start() → Task.Run(RunSession)
    ↓
┌──────────────────────────────────┐
│ while (!отключение)              │
│   CreateGameState()              │
│   CreateGame()                   │
│   game.Run(state) ← игровой цикл │
│   if (рестарт) → продолжить      │
│   if (выход)   → break           │
└──────────────────────────────────┘
    ↓
session.Dispose() → очистка
```

---

## 🎯 Этап 9. Делаем GameLoop.Update() публичным

### Проблема
`GameLoop.Update()` был `private`.
Наша сессия вызывает его напрямую (для гибкости).

### Решение
В файле `Core/Engine/GameLoop.cs` меняем:

```csharp
// Было:
private void Update(State.GameState state)

// Стало:
public void Update(State.GameState state)
```

### Зачем
Сессия может захотеть обновить игру вручную, а не через `Game.Run()`.
Публичный метод даёт гибкость.

---

## 🎯 Этап 10. Переписываем Program.cs

### Что было
Обычный `Main()`, создающий `Game` с консольными зависимостями.

### Что делаем
ASP.NET Core сервер с WebSocket endpoint'ом.

### Заменяем `Program.cs`

```csharp
using Microsoft.AspNetCore.Builder;
using System.Net.WebSockets;
using gameSnake.Server.Session;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Главная страница
app.MapGet("/", () => "GameSnake WebSocket Server. Connect to /ws to play.");

// Статистика: сколько игроков онлайн
app.MapGet("/stats", () => new { Players = SessionManager.ActiveCount });

// Включаем WebSocket
app.UseWebSockets();

// Endpoint /ws — сюда подключаются клиенты
app.Map("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        using var webSocket = await context.WebSockets.AcceptWebSocketAsync();

        // Регистрируем игрока
        var sessionId = SessionManager.Register(webSocket);

        // Ждём, пока клиент не отключится
        while (webSocket.State == WebSocketState.Open)
        {
            await Task.Delay(1000);
        }

        // Клиент ушёл — удаляем сессию
        SessionManager.Unregister(sessionId);
    }
    else
    {
        context.Response.StatusCode = 400; // Bad Request
    }
});

app.Run();
```

### Что здесь происходит
| Строка                           | Зачем                                             |
|----------------------------------|---------------------------------------------------|
| `WebApplication.CreateBuilder()` | Создаём ASP.NET Core приложение                   |
| `app.MapGet("/", ...)`           | Главная страница — просто текст                   |
| `app.MapGet("/stats", ...)`      | Статистика: `{"players": 3}`                      |
| `app.UseWebSockets()`            | Включаем поддержку WebSocket                      |
| `app.Map("/ws", ...)`            | Endpoint для подключений: `ws://localhost:5000/ws`|
| `SessionManager.Register()`      | Запускаем игру для нового игрока                  |
| `while (webSocket.State == Open)`| Ждём отключения                                   |
| `SessionManager.Unregister()`    | Убираем игрока из списка                          |

### Адреса
- `http://localhost:5000/` — главная страница
- `http://localhost:5000/stats` — статистика
- `ws://localhost:5000/ws` — WebSocket для подключения

---

## 🎯 Этап 11. Запускаем и проверяем

### Команда запуска
```bash
dotnet run
```

### Проверка
Открой браузер и перейди по адресам:

| Адрес                         | Ожидаемый ответ                                      |
|-------------------------------|------------------------------------------------------|
| `http://localhost:5000/`      | `GameSnake WebSocket Server. Connect to /ws to play.`|
| `http://localhost:5000/stats` | `{"players":0}` (без подключённых игроков)           |

### Тест WebSocket (через браузерную консоль)
```javascript
const ws = new WebSocket("ws://localhost:5000/ws");
ws.onmessage = (e) => console.log(JSON.parse(e.data));
ws.onopen = () => ws.send(JSON.stringify({type:"move", direction:"right"}));
```

---

## 📂 Итоговая структура проекта

```
gameSnake/
├── gameSnake.csproj              ← Microsoft.NET.Sdk.Web
├── Program.cs                    ← ASP.NET Core сервер
│
├── Core/                         ← ЯДРО (не трогали, только Game упростили)
│   ├── Engine/
│   │   ├── Game.cs              ← Теперь принимает GameState извне
│   │   └── GameLoop.cs          ← Update() стал public
│   ├── State/                    ← GameState, GameFlags, GameSettings
│   └── Factories/                ← GameStateFactory, SnakeFactory, FoodFactory
│
├── Models/                       ← Змейка, Еда, Точка, Направление
├── Logic/                        ← Шаги логики, CollisionDetector
├── Interfaces/                   ← IGameRenderer, IInputHandler, ...
├── Attributes/                   ← GameStepOrder, HeaderInfo
├── Servises/                     ← Сообщения
├── Utils/                        ← Утилиты расчёта
│
└── Server/                       ← НОВОЕ
    ├── DTO/
    │   └── GameDto.cs           ← GameStateDto, ClientCommand, PointDto
    ├── Renderers/
    │   └── WebSocketRenderer.cs ← Отправляет JSON клиенту
    ├── InputHandlers/
    │   └── WebSocketInputHandler.cs ← Принимает команды от клиента
    ├── Session/
    │   ├── SessionManager.cs    ← Реестр сессий
    │   └── WebSocketGameSession.cs ← Одна сессия = один игрок
    └── NoOpWindowConfigurator.cs ← Заглушка
```

---

## 🧠 Ключевые выводы

1. **Интерфейсы — это мост**. `IGameRenderer` может быть `ConsoleRenderer` или `WebSocketRenderer`. Ядро не знает разницы.

2. **DTO — это переводчик**. Внутри игры — `Snake.Body` (список `Point`), снаружи — `snake` (список `PointDto` в JSON).

3. **Сессия = игрок**. Каждый игрок получает свою змейку, своё поле, свой счёт.

4. **Очередь команд** — клиент может отправить 10 команд за раз. Очередь их запомнит и обработает по одной.

5. **Минимальные изменения**. Ядро (`Core/`, `Models/`, `Logic/`) осталось как было. Мы только заменили UI-слой.
