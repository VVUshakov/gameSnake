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
                await client.ConnectAsync(ServerUrl, cts.Token);
                Console.WriteLine("Connected! Starting game...");
                await Task.Delay(500, cts.Token);

                var renderer = new ConsoleRenderer();

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
