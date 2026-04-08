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
