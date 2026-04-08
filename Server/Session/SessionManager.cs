using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace gameSnake.Server.Session
{
    /// <summary>
    /// Реестр активных игровых сессий.
    /// Отслеживает подключения, корректно завершает сессии при отключении.
    /// </summary>
    public static class SessionManager
    {
        private static readonly ConcurrentDictionary<Guid, WebSocketGameSession> _sessions = new();

        /// <summary>
        /// Количество активных сессий.
        /// </summary>
        public static int ActiveCount => _sessions.Count;

        /// <summary>
        /// Регистрирует новую сессию и запускает игровой цикл.
        /// </summary>
        /// <param name="webSocket">WebSocket подключение клиента</param>
        /// <returns>Идентификатор сессии (для последующего удаления)</returns>
        public static Guid Register(WebSocket webSocket)
        {
            var session = new WebSocketGameSession(webSocket);
            if (_sessions.TryAdd(session.Id, session))
            {
                session.Start();
                return session.Id;
            }
            throw new InvalidOperationException("Не удалось зарегистривать сессию");
        }

        /// <summary>
        /// Удаляет сессию и освобождает ресурсы.
        /// </summary>
        public static void Unregister(Guid sessionId)
        {
            if (_sessions.TryRemove(sessionId, out var session))
                session.Dispose();
        }

        /// <summary>
        /// Корректно останавливает все сессии (при остановке сервера).
        /// </summary>
        public static void StopAll()
        {
            foreach (var id in _sessions.Keys.ToArray())
                Unregister(id);
        }
    }
}
