# 🐍 Как сделать консольный клиент для WebSocket-сервера

Это пошаговое руководство для начинающих. Мы превратим исходный код `UI/ConsoleUI/` (который раньше работал локально с игрой) в **клиент**, который подключается к серверу по WebSocket и рисует игру в консоли.

---

## 📋 Что нам нужно знать заранее

| Термин | Что значит |
|---|---|
| **WebSocket** | Канал связи между клиентом и сервером. Оба могут отправлять сообщения в любой момент |
| **Клиент** | Программа, которая подключается к серверу, получает данные и рисует игру |
| **Сервер** | Программа, которая хранит состояние игры и рассылает его всем подключённым игрокам |
| **JSON** | Формат данных, понятный и серверу, и клиенту: `{"status":"Playing","snake":[{"x":5,"y":5}]}` |
| **DTO** | «Data Transfer Object» — объект для передачи данных между сервером и клиентом |
| **Конвертер** | Превращает DTO из сервера во внутренние объекты, которые понимают наши рендереры |

---

## 🗺️ Общая картина

Мы **не пишем с нуля**. Мы берём готовый `UI/ConsoleUI/` (который уже рисовал игру в консоли) и минимально адаптируем:

```
┌───────────────────────────────────────────────┐
│          Раньше: всё локально                 │
│                                               │
│  ConsoleInputHandler → GameState → Renderers  │
│        (клавиатура)      (в памяти)   (консоль)│
└───────────────────────────────────────────────┘

┌───────────────────────────────────────────────┐
│          Теперь: через сервер                 │
│                                               │
│  Клавиатура → ConsoleInputHandler             │
│                     ↓                         │
│              GameClient ───WebSocket──→ Сервер│
│                     ↓                         │
│            DtoToStateConverter                │
│                     ↓                         │
│          GameState → Рендереры (консоль)      │
│          (те же самые! без изменений!)         │
└───────────────────────────────────────────────┘
```

**Ключевая идея:** рендереры остаются **точно такими же**, как были. Мы только добавляем слой конвертации между DTO от сервера и GameState, который понимают рендереры.

---

## 🎯 Этап 1. Создаём проект клиента

### Что делаем
Создаём отдельный консольный проект рядом с сервером.

### Структура файлов
```
ConsoleClient/
├── ConsoleClient.csproj          ← Проект
├── Program.cs                    ← Главная точка входа
├── DTO/
│   ├── GameDto.cs               ← Объекты для JSON (те же, что на сервере)
│   └── DtoToStateConverter.cs   ← Конвертер DTO → GameState
├── Network/
│   └── GameClient.cs            ← WebSocket подключение
└── UI/
    ├── ConsoleRenderers/         ← Восстановлено из master:UI/ConsoleUI/
    │   ├── ConsoleRenderer.cs
    │   ├── HeaderFormatter.cs
    │   ├── MessageStyleProvider.cs
    │   ├── RenderConstants.cs
    │   └── Renderers/
    │       ├── FieldRenderer.cs
    │       ├── FoodRenderer.cs
    │       ├── HeaderRenderer.cs
    │       ├── MessageRenderer.cs
    │       └── SnakeRenderer.cs
    └── InputHandlers/            ← Восстановлено из master:UI/ConsoleUI/
        ├── ConsoleInputHandler.cs  ← Адаптирован
        ├── InputReader.cs
        └── KeyActionProvider.cs
```

### Создаём файл `ConsoleClient.csproj`

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <GenerateAssemblyInfo>false</GenerateAssemblyInfo>
  </PropertyGroup>

  <ItemGroup>
    <!-- Исключаем файлы сервера, чтобы не было конфликтов -->
    <Compile Remove="Servise\**" />
    <Compile Remove="Core\**" />
    <Compile Remove="Logic\**" />
    <Compile Remove="Server\**" />
  </ItemGroup>

  <ItemGroup>
    <!-- Ссылка на основной проект — рендереры используют его интерфейсы -->
    <ProjectReference Include="..\gameSnake.csproj" />
  </ItemGroup>

</Project>
```

### Зачем
| Строка | Зачем |
|---|---|
| `Microsoft.NET.Sdk` | Обычный консольный проект (не веб-сервер) |
| `Compile Remove="Server\**"` | Не компилировать файлы сервера — они не нужны клиенту |
| `ProjectReference` | Дать доступ к интерфейсам (`IGameRenderer`, `GameState`, модели) |

### Добавляем в solution
```bash
dotnet sln gameSnake.sln add ConsoleClient/ConsoleClient.csproj
```

---

## 🎯 Этап 2. Восстанавливаем рендереры из `master`

### Что делаем
Все файлы из папки `UI/ConsoleUI/` (ветка `master`) копируем в `ConsoleClient/UI/`.

### Какие файлы
| Файл | Зачем | Изменения |
|---|---|---|
| `ConsoleRenderer.cs` | Главный рендерер | **Без изменений** |
| `HeaderFormatter.cs` | Форматирует заголовок | **Без изменений** |
| `MessageStyleProvider.cs` | Цвета сообщений | **Без изменений** |
| `RenderConstants.cs` | Символы (`#`, `O`, `*`, `@`) | **Без изменений** |
| `Renderers/FieldRenderer.cs` | Рисует рамку поля | **Без изменений** |
| `Renderers/FoodRenderer.cs` | Рисует еду | **Без изменений** |
| `Renderers/HeaderRenderer.cs` | Рисует заголовок | **Без изменений** |
| `Renderers/MessageRenderer.cs` | Рисует сообщения | **Без изменений** |
| `Renderers/SnakeRenderer.cs` | Рисует змейку | **Без изменений** |
| `InputHandlers/InputReader.cs` | Читает клавиатуру | **Без изменений** |
| `InputHandlers/KeyActionProvider.cs` | Маппинг клавиш | **Без изменений** |

> **Не восстанавливаем:** `ConsoleWindowConfigurator.cs` — он не используется в клиенте. Сервер сам управляет сессиями.

### Команда для восстановления
```bash
git show master:UI/ConsoleUI/ConsoleRenderers/ConsoleRenderer.cs > ConsoleClient/UI/ConsoleRenderers/ConsoleRenderer.cs
git show master:UI/ConsoleUI/ConsoleRenderers/HeaderFormatter.cs > ConsoleClient/UI/ConsoleRenderers/HeaderFormatter.cs
# ... и так далее для каждого файла
```

### Почему без изменений?
Потому что мы конвертируем DTO от сервера в `GameState` (внутренний объект), и рендереры работают как раньше. Они не знают, что данные пришли по сети.

---

## 🎯 Этап 3. Создаём DTO (объекты для передачи данных)

### Что такое DTO
Когда сервер и клиент общаются, они отправляют **JSON-строки**. DTO — это классы, которые описывают, как данные выглядят в JSON.

### Создаём файл
```
ConsoleClient/DTO/GameDto.cs
```

```csharp
using System.Text.Json.Serialization;

namespace ConsoleClient.DTO
{
    /// <summary>
    /// Статус игровой сессии, отправляемый сервером.
    /// </summary>
    public enum GameStatus
    {
        /// <summary>Игра идёт — змейка двигается, ввод обрабатывается</summary>
        Playing,

        /// <summary>Пауза — змейка не двигается до снятия паузы</summary>
        Paused,

        /// <summary>Игра окончена — змейка врезалась в стену или в себя</summary>
        GameOver,

        /// <summary>Победа — всё поле заполнено змейкой</summary>
        Win
    }

    /// <summary>
    /// Координата точки на игровом поле.
    /// Используется для позиций сегментов змейки и еды.
    /// </summary>
    public record PointDto(int X, int Y);

    /// <summary>
    /// Полное состояние игры, отправляемое сервером каждый кадр.
    /// </summary>
    public class GameStateDto
    {
        /// <summary>Текущий статус игры</summary>
        [JsonPropertyName("status")]
        public GameStatus Status { get; set; }

        /// <summary>Список координат всех сегментов змейки</summary>
        [JsonPropertyName("snake")]
        public List<PointDto> Snake { get; set; } = new();

        /// <summary>Координата еды на поле. Null — если еды нет</summary>
        [JsonPropertyName("food")]
        public PointDto? Food { get; set; }

        /// <summary>Текущий счёт игрока</summary>
        [JsonPropertyName("score")]
        public int Score { get; set; }

        /// <summary>Текущий уровень сложности</summary>
        [JsonPropertyName("level")]
        public int Level { get; set; }

        /// <summary>Количество оставшихся жизней</summary>
        [JsonPropertyName("lives")]
        public int Lives { get; set; }

        /// <summary>Текстовое сообщение для отображения поверх поля</summary>
        [JsonPropertyName("message")]
        public string? Message { get; set; }
    }

    /// <summary>
    /// Команда, отправляемая клиентом на сервер.
    /// </summary>
    public class ClientCommand
    {
        /// <summary>Тип команды: "move", "pause", "restart", "exit"</summary>
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        /// <summary>Направление движения (только для type="move")</summary>
        [JsonPropertyName("direction")]
        public string? Direction { get; set; }
    }
}
```

### Пример JSON от сервера
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

### Пример команды от клиента
```json
{"type":"move","direction":"up"}
```

---

## 🎯 Этап 4. Создаём конвертер DTO → GameState

### Зачем
Рендереры ожидают `GameState` (внутренний объект с `Snake`, `Food`, `Header` и т.д.). Сервер присылает `GameStateDto` (JSON-объект). Конвертер превращает одно в другое.

### Создаём файл
```
ConsoleClient/DTO/DtoToStateConverter.cs
```

```csharp
using gameSnake.Core.State;
using gameSnake.Models;
using gameSnake.Server.DTO;

namespace ConsoleClient.DTO
{
    /// <summary>
    /// Конвертирует DTO от сервера в GameState для переиспользования рендереров.
    /// </summary>
    public static class DtoToStateConverter
    {
        /// <summary>
        /// Преобразует полученное от сервера DTO во внутренний GameState.
        /// </summary>
        public static GameState Convert(GameStateDto serverState)
        {
            var field = new PlayingField(
                CalculateFieldWidth(serverState),
                CalculateFieldHeight(serverState));

            var snake = new Snake(
                serverState.Snake.Select(point => new Point(point.X, point.Y)));

            Point? foodPosition = serverState.Food is PointDto foodPoint
                ? new Point(foodPoint.X, foodPoint.Y)
                : null;
            var food = new Food(foodPosition, foodPosition != null);

            var header = new Header
            {
                Score = serverState.Score,
                Level = serverState.Level,
                Lives = serverState.Lives
            };

            var state = new GameState(header, field, snake, food);

            // Применяем статус к флагам
            switch (serverState.Status)
            {
                case GameStatus.Paused:
                    state.Flags.IsPaused = true;
                    state.ActiveMessage = GameMessageType.Pause;
                    break;
                case GameStatus.GameOver:
                    state.Flags.IsGameOver = true;
                    state.ActiveMessage = GameMessageType.GameOver;
                    break;
                case GameStatus.Win:
                    state.Flags.IsWin = true;
                    state.ActiveMessage = GameMessageType.Win;
                    break;
            }

            // Отображаем сообщение из сервера, если есть
            if (!string.IsNullOrEmpty(serverState.Message) && !state.ActiveMessage.HasValue)
            {
                state.Flags.IsPaused = true;
            }

            return state;
        }

        private static int CalculateFieldWidth(GameStateDto serverState)
        {
            int maximumCoordinate = 20;
            foreach (var segment in serverState.Snake)
                if (segment.X + 2 > maximumCoordinate) maximumCoordinate = segment.X + 2;

            if (serverState.Food is PointDto foodWidth && foodWidth.X + 2 > maximumCoordinate)
                maximumCoordinate = foodWidth.X + 2;

            return maximumCoordinate;
        }

        private static int CalculateFieldHeight(GameStateDto serverState)
        {
            int maximumCoordinate = 10;
            foreach (var segment in serverState.Snake)
                if (segment.Y + 2 > maximumCoordinate) maximumCoordinate = segment.Y + 2;

            if (serverState.Food is PointDto foodHeight && foodHeight.Y + 2 > maximumCoordinate)
                maximumCoordinate = foodHeight.Y + 2;

            return maximumCoordinate;
        }
    }
}
```

### Что здесь происходит
| Метод | Зачем |
|---|---|
| `Convert(serverState)` | Главная функция. Превращает `GameStateDto` → `GameState` |
| `CalculateFieldWidth/Height(serverState)` | Определяют размер поля по максимальным координатам змейки и еды |

### Цикл данных
```
Сервер → JSON → GameStateDto → DtoToStateConverter → GameState → Рендереры → Консоль
```

---

## 🎯 Этап 5. Создаём WebSocket-клиент

### Зачем
Класс, который подключается к серверу, отправляет команды и принимает состояние игры.

### Создаём файл
```
ConsoleClient/Network/GameClient.cs
```

```csharp
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using ConsoleClient.DTO;

namespace ConsoleClient.Network
{
    /// <summary>
    /// WebSocket-подключение к игровому серверу.
    /// </summary>
    public class GameClient : IDisposable
    {
        private readonly ClientWebSocket _webSocket = new();
        private readonly byte[] _buffer = new byte[4096];

        public WebSocketState State => _webSocket.State;

        /// <summary>Подключиться к серверу</summary>
        public async Task ConnectAsync(string url, CancellationToken ct)
        {
            await _webSocket.ConnectAsync(new Uri(url), ct);
        }

        /// <summary>Отправить команду серверу</summary>
        public async Task SendAsync(ClientCommand command, CancellationToken ct)
        {
            var json = JsonSerializer.Serialize(command);
            var bytes = Encoding.UTF8.GetBytes(json);
            await _webSocket.SendAsync(
                new ArraySegment<byte>(bytes),
                WebSocketMessageType.Text, true, ct);
        }

        /// <summary>Принять состояние игры от сервера</summary>
        public async Task<GameStateDto?> ReceiveStateAsync(CancellationToken ct)
        {
            var result = await _webSocket.ReceiveAsync(
                new ArraySegment<byte>(_buffer), ct);

            if (result.MessageType != WebSocketMessageType.Text || result.Count == 0)
                return null;

            var json = Encoding.UTF8.GetString(_buffer, 0, result.Count);
            return JsonSerializer.Deserialize<GameStateDto>(json);
        }

        public async Task CloseAsync(CancellationToken ct)
        {
            if (_webSocket.State == WebSocketState.Open)
                await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Bye", ct);
        }

        public void Dispose() => _webSocket.Dispose();
    }
}
```

### Что здесь происходит
| Метод | Зачем |
|---|---|
| `ConnectAsync(url)` | Подключиться к `ws://localhost:5000/ws` |
| `SendAsync(command)` | Превратить команду в JSON и отправить |
| `ReceiveStateAsync()` | Получить JSON от сервера и превратить в `GameStateDto` |
| `CloseAsync()` | Корректно отключиться |

### Цикл жизни
```
ConnectAsync() → SendAsync() / ReceiveStateAsync() → CloseAsync() → Dispose()
```

---

## 🎯 Этап 6. Адаптируем обработчик ввода

### Что было
`ConsoleInputHandler` из `master` использовал `IInputHandler` и локально менял `GameState`.

### Что делаем
Убираем интерфейс `IInputHandler`. Вместо этого — читаем клавиатуру и отправляем команду через WebSocket.

### Заменяем файл
```
ConsoleClient/UI/InputHandlers/ConsoleInputHandler.cs
```

```csharp
using ConsoleClient.DTO;
using ConsoleClient.Network;

namespace ConsoleClient.UI.InputHandlers
{
    /// <summary>
    /// Читает клавиатуру и отправляет команды серверу через WebSocket.
    /// </summary>
    public static class ConsoleInputHandler
    {
        /// <summary>
        /// Считывает нажатие и отправляет команду серверу.
        /// Возвращает true, если клиент запросил выход.
        /// </summary>
        public static async Task<bool> ProcessInputAsync(GameClient client, CancellationToken ct)
        {
            if (!Console.KeyAvailable) return false;

            var key = Console.ReadKey(intercept: true).Key;
            ClientCommand? command = MapKeyToCommand(key);

            if (command == null) return false;
            if (command.Type == "exit") return true;

            await client.SendAsync(command, ct);
            return false;
        }

        private static ClientCommand? MapKeyToCommand(ConsoleKey key)
        {
            return key switch
            {
                ConsoleKey.UpArrow    or ConsoleKey.W => new() { Type = "move", Direction = "up" },
                ConsoleKey.DownArrow  or ConsoleKey.S => new() { Type = "move", Direction = "down" },
                ConsoleKey.LeftArrow  or ConsoleKey.A => new() { Type = "move", Direction = "left" },
                ConsoleKey.RightArrow or ConsoleKey.D => new() { Type = "move", Direction = "right" },
                ConsoleKey.P or ConsoleKey.Spacebar   => new() { Type = "pause" },
                ConsoleKey.R                          => new() { Type = "restart" },
                ConsoleKey.Escape                     => new() { Type = "exit" },
                _ => null
            };
        }
    }
}
```

### Что здесь происходит
| Метод | Зачем |
|---|---|
| `ProcessInputAsync()` | Читает клавиатуру, отправляет команду, возвращает `true` если «выход» |
| `MapKeyToCommand()` | Превращает нажатую клавишу в команду для сервера |

### Клавиши
| Клавиша | Команда |
|---|---|
| `↑` / `W` | `{"type":"move","direction":"up"}` |
| `↓` / `S` | `{"type":"move","direction":"down"}` |
| `←` / `A` | `{"type":"move","direction":"left"}` |
| `→` / `D` | `{"type":"move","direction":"right"}` |
| `P` / `Space` | `{"type":"pause"}` |
| `R` | `{"type":"restart"}` |
| `Esc` | `{"type":"exit"}` |

---

## 🎯 Этап 7. Собираем всё в Program.cs

### Что делаем
Главный цикл: подключиться → получать состояние → конвертировать → рисовать → читать ввод → отправить команду.

### Создаём файл
```
ConsoleClient/Program.cs
```

```csharp
using ConsoleClient.DTO;
using ConsoleClient.Network;
using ConsoleClient.UI.InputHandlers;
using gameSnake.UI.ConsoleUI.ConsoleRenderers;

namespace ConsoleClient
{
    public class Program
    {
        private const string ServerUrl = "ws://localhost:5000/ws";

        public static async Task Main(string[] args)
        {
            Console.Title = "Snake — Console Client";
            Console.CursorVisible = false;

            Console.WriteLine("Connecting to server...");
            Console.WriteLine($"Server: {ServerUrl}");
            Console.WriteLine();
            Console.WriteLine("Controls: Arrows/WASD — Move | P/Space — Pause | R — Restart | Esc — Exit");
            Console.WriteLine();
            Console.WriteLine("Press any key to connect...");
            Console.ReadKey(true);

            using var client = new GameClient();
            using var cts = new CancellationTokenSource();

            try
            {
                // 1. Подключаемся к серверу
                await client.ConnectAsync(ServerUrl, cts.Token);
                Console.WriteLine("Connected! Starting game...");
                await Task.Delay(500, cts.Token);

                // 2. Создаём рендерер (из мастер-ветки, без изменений!)
                var renderer = new ConsoleRenderer();

                // 3. Главный цикл
                while (client.State == System.Net.WebSockets.WebSocketState.Open)
                {
                    // Получаем состояние от сервера
                    var dto = await client.ReceiveStateAsync(cts.Token);
                    if (dto == null) continue;

                    // Конвертируем DTO → GameState (рендереры работают с GameState без изменений)
                    var state = DtoToStateConverter.Convert(dto);

                    // Рисуем через восстановленные рендереры
                    renderer.Clear();
                    renderer.Render(state);

                    // Обрабатываем ввод → отправляем команды серверу
                    bool exit = await ConsoleInputHandler.ProcessInputAsync(client, cts.Token);
                    if (exit) break;

                    await Task.Delay(50, cts.Token);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Connection error: {ex.Message}");
                Console.ResetColor();
            }
            finally
            {
                cts.Cancel();
                Console.CursorVisible = true;
                Console.WriteLine();
                Console.WriteLine("Disconnected.");
            }
        }
    }
}
```

### Что здесь происходит
| Блок | Зачем |
|---|---|
| Подключение | `ConnectAsync()` → ждём ответа сервера |
| Цикл | Бесконечный, пока WebSocket открыт |
| `ReceiveStateAsync()` | Получаем JSON от сервера |
| `DtoToStateConverter.Convert()` | Превращаем DTO → GameState |
| `renderer.Render(state)` | Рисуем через старые рендереры |
| `ProcessInputAsync()` | Читаем клавиатуру и отправляем команду |
| `Task.Delay(50)` | 50мс задержка между кадрами (20 FPS) |

### Поток данных
```
┌────────────────────────────────────────────────────┐
│                  Главный цикл                       │
│                                                     │
│  Сервер → ReceiveStateAsync() → GameStateDto       │
│                           ↓                         │
│               DtoToStateConverter.Convert()         │
│                           ↓                         │
│                       GameState                     │
│                           ↓                         │
│                 renderer.Render(state)              │
│                           ↓                         │
│                    Консоль (экран)                  │
│                                                     │
│  Клавиатура → ConsoleInputHandler                  │
│           ↓                                         │
│    client.SendAsync() → Сервер                     │
└────────────────────────────────────────────────────┘
```

---

## 🎯 Этап 8. Запускаем и проверяем

### Запуск сервера (терминал 1)
```bash
dotnet run --project gameSnake.csproj
```

### Запуск клиента (терминал 2)
```bash
dotnet run --project ConsoleClient/ConsoleClient.csproj
```

### Что должно произойти
1. Клиент покажет: `Connecting to server...`
2. Нажми любую клавишу
3. Клиент подключится: `Connected! Starting game...`
4. Появится игра в консоли — змейка, еда, счёт
5. Управляй стрелками или WASD

### Управление
| Клавиша | Действие |
|---|---|
| `↑` / `W` | Вверх |
| `↓` / `S` | Вниз |
| `←` / `A` | Влево |
| `→` / `D` | Вправо |
| `P` / `Space` | Пауза |
| `R` | Перезапуск |
| `Esc` | Выход |

### Тест WebSocket (через браузерную консоль)

> **Как открыть консоль браузера:**
> 1. Открой `http://localhost:5000/` в браузере (Chrome, Firefox, Edge)
> 2. Нажми **F12** (или **Ctrl + Shift + I**)
> 3. Перейди на вкладку **Console** (Консоль)
>
> В строку ввода внизу консоли вставь код по одной строке:

```javascript
// 1. Подключаемся к серверу
const ws = new WebSocket("ws://localhost:5000/ws");

// 2. При подключении — отправляем команду движения
ws.onopen = () => ws.send(JSON.stringify({type:"move", direction:"right"}));

// 3. При получении данных — выводим в консоль
ws.onmessage = (e) => console.log(JSON.parse(e.data));
```

**Другие команды для теста:**
```javascript
ws.send(JSON.stringify({type:"pause"}));       // Пауза
ws.send(JSON.stringify({type:"move", direction:"up"}));    // Вверх
ws.send(JSON.stringify({type:"move", direction:"down"}));  // Вниз
ws.send(JSON.stringify({type:"move", direction:"left"}));  // Влево
ws.send(JSON.stringify({type:"move", direction:"right"})); // Вправо
ws.send(JSON.stringify({type:"restart"}));     // Перезапуск
ws.send(JSON.stringify({type:"exit"}));        // Выход
```

---

## 📂 Итоговая структура проекта

```
ConsoleClient/
├── ConsoleClient.csproj          ← Проект, ссылка на gameSnake
├── Program.cs                    ← Главный цикл
│
├── DTO/                          ← НОВОЕ
│   ├── GameDto.cs               ← Объекты для JSON
│   └── DtoToStateConverter.cs   ← Конвертер DTO → GameState
│
├── Network/                      ← НОВОЕ
│   └── GameClient.cs            ← WebSocket подключение
│
└── UI/                           ← Восстановлено из master:UI/ConsoleUI/
    ├── ConsoleRenderers/
    │   ├── ConsoleRenderer.cs
    │   ├── HeaderFormatter.cs
    │   ├── MessageStyleProvider.cs
    │   ├── RenderConstants.cs
    │   └── Renderers/
    │       ├── FieldRenderer.cs
    │       ├── FoodRenderer.cs
    │       ├── HeaderRenderer.cs
    │       ├── MessageRenderer.cs
    │       └── SnakeRenderer.cs
    └── InputHandlers/
        ├── ConsoleInputHandler.cs  ← Адаптирован (отправляет через WS)
        ├── InputReader.cs          ← Без изменений
        └── KeyActionProvider.cs    ← Без изменений
```

---

## 🧠 Ключевые выводы

1. **Не пиши с нуля то, что уже есть.** Рендереры из `master` работают без изменений — мы только добавили конвертер между DTO и GameState.

2. **Конвертер — это мост.** `DtoToStateConverter` превращает JSON-данные сервера во внутренние объекты. Рендереры ничего не замечают.

3. **DTO — общий язык.** И сервер, и клиент используют одинаковые DTO-классы. JSON — это контракт между ними.

4. **WebSocket — двусторонний канал.** Клиент отправляет команды (`SendAsync`) и получает состояние (`ReceiveStateAsync`).

5. **Минимальные изменения.** Из 13 файлов UI только `ConsoleInputHandler.cs` изменён. Остальные 11 — точные копии из `master`. `ConsoleWindowConfigurator.cs` не восстанавливается — он не нужен клиенту.
