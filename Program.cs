using Microsoft.AspNetCore.Builder;
using System.Net.WebSockets;
using gameSnake.Server.Session;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "GameSnake WebSocket Server. Connect to /ws to play.");

app.UseWebSockets();

app.Map("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        using var session = new WebSocketGameSession(webSocket);
        session.Start();

        // Ждём, пока соединение не закроется
        while (webSocket.State == WebSocketState.Open)
        {
            await Task.Delay(1000);
        }
    }
    else
    {
        context.Response.StatusCode = 400;
    }
});

app.Run();
