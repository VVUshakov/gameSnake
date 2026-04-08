using Microsoft.AspNetCore.Builder;
using System.Net.WebSockets;
using gameSnake.Server.Session;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "GameSnake WebSocket Server. Connect to /ws to play.");
app.MapGet("/stats", () => new { Players = SessionManager.ActiveCount });

app.UseWebSockets();

app.Map("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        Guid sessionId = SessionManager.Register(webSocket);

        // Ждём, пока клиент не отключится
        while (webSocket.State == WebSocketState.Open)
        {
            await Task.Delay(1000);
        }

        // Клиент отключился — удаляем сессию
        SessionManager.Unregister(sessionId);
    }
    else
    {
        context.Response.StatusCode = 400;
    }
});

app.Run();
